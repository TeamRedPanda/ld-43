using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField]
	StatsManager StatsManagerObj;

	public float SecondsPerMonth = 60;
	private float m_CurrentMonthTimeRemaining = 60;
	private int m_CurrentMonth = 0;
    private int m_CurrentYear = 1501;
    private int m_TotalMonths = 0;

	public float SacrificePeriodBase = 6;
    private float m_SacrificePeriod;
    private float m_NextSacrifice;

	public float ArrivalPeriod = 3;
	private float m_NextArrival;

	public GameObject GameOver;
    public GameObject RecruitMenu;
	public GameObject SacrificeMenu;
	public GameObject SacrificeResult;
	public Transform Canvas;
	
	private SacrificeOutcome m_SacrificeManaging;

    [SerializeField]
    private ResourceListView m_ResourceListView;

	private bool m_IsPaused {
        get {
            return m_WindowsOpen != 0;
        }
    }

    private int m_WindowsOpen = 0;

	float m_VillagerLimit;
	float m_MaxSacrifices;

	void Start()
	{
		m_SacrificeManaging = this.GetComponent<SacrificeOutcome>();

		StatsManagerObj = new StatsManager();
        StatsManagerObj.OnStatsChanged += UpdateResourceView;
		GetInitialState();

		StatsManagerObj.UpdateStats();
		CalculateSacrificePeriod();
		CalculateLimits();
		CalculateNextArrival();
        UpdateResourceView();

        m_NextArrival = ArrivalPeriod;
		m_NextSacrifice = (m_CurrentMonth + m_SacrificePeriod) % 12;

        m_CurrentMonthTimeRemaining = SecondsPerMonth;
	}

    void UpdateResourceView()
    {
        m_ResourceListView.UpdateWood((int)StatsManagerObj.WoodProduction, StatsManagerObj.WoodModifier);
        m_ResourceListView.UpdateFood((int)StatsManagerObj.FoodProduction, StatsManagerObj.FoodModifier);
        m_ResourceListView.UpdateFaith((int)StatsManagerObj.FaithProduction, StatsManagerObj.FaithModifier);
        m_ResourceListView.UpdatePopulation((int)StatsManagerObj.Villagers.Count, (int)m_VillagerLimit); // TODO: Wtf.
	}

	void Update()
	{
        if (m_IsPaused)
            return;

		m_CurrentMonthTimeRemaining -= Time.deltaTime;
		if (m_CurrentMonthTimeRemaining <= 0) {
            if (m_CurrentMonth == 11)
                m_CurrentYear++;

			m_CurrentMonth = (m_CurrentMonth + 1) % 12;
            m_TotalMonths++;
            m_ResourceListView.UpdateDate(m_CurrentMonth, m_CurrentYear); // TODO: WE NEED YEARS!!!!
			m_CurrentMonthTimeRemaining = SecondsPerMonth;

			if (StatsManagerObj.Villagers.Count <= 0) {
				Instantiate(GameOver, Canvas).GetComponent<GameOverView>();
				m_WindowsOpen++;
			}
        }

		if (m_CurrentMonth == m_NextArrival) {
            m_WindowsOpen++;

            Villager[] arrivals = GenerateVillagers();

			RecruitMenuView recruitView = Instantiate(RecruitMenu, Canvas).GetComponent<RecruitMenuView>();
			recruitView.SetRecruitCount((int)m_VillagerLimit - (int)StatsManagerObj.Villagers.Count);
			recruitView.AddVillagerViews(arrivals);
			recruitView.OnRecruit += StatsManagerObj.Recruit;
            recruitView.OnRecruitFulfill += () => { CalculateNextArrival(); 
			CalculateLimits();
			CalculateSacrificePeriod();
			UpdateResourceView(); 
			m_WindowsOpen--; };
            return;
		}

		if (m_CurrentMonth == m_NextSacrifice) {
			m_WindowsOpen++;

			SacrificeMenuView sacrificeView = Instantiate(SacrificeMenu, Canvas).GetComponent<SacrificeMenuView>();

            // Only send the first 8 villagers (UI limitation).
			for (int i = 0; i < Mathf.Min(StatsManagerObj.Villagers.Count, 8); i++) {
				sacrificeView.AddVillagerView(StatsManagerObj.Villagers[i], i);
			}

			sacrificeView.SetSacrificeCount((int)m_MaxSacrifices);
			sacrificeView.OnSacrifice += StatsManagerObj.Sacrifice;
            sacrificeView.OnSacrificeFulfill += () => {
                m_NextSacrifice = (m_CurrentMonth + m_SacrificePeriod) % 12;
                m_WindowsOpen--;
				SacrificeResult sacrificeResult = CalculateSacrificeOutcome();
				StatsManagerObj.ApplyModifier(sacrificeResult.Modifiers);
				CalculateSacrificePeriod();
				CalculateLimits();
				UpdateResourceView();
				SacrificeResultView resultView = Instantiate(SacrificeResult, Canvas).GetComponent<SacrificeResultView>();
				m_WindowsOpen++;
				resultView.Initialize(sacrificeResult.Title, sacrificeResult.Icon, sacrificeResult.Text);
				resultView.OnWindowClose += () => m_WindowsOpen--;
            };
		}
	}

	void GetInitialState()
	{
		Villager villager1 = new Villager();
		Villager villager2 = new Villager();

		villager1.FoodProduction  = 5;
		villager1.WoodProduction  = 5;
		villager1.FaithProduction = 5;

		villager2.FoodProduction  = 6;
		villager2.WoodProduction  = 6;
		villager2.FaithProduction = 6;

		StatsManagerObj.Villagers.Add(villager1);
		StatsManagerObj.Villagers.Add(villager2);

		StatsManagerObj.SacrificesValues = new List<Villager>();
	}

	Villager[] GenerateVillagers()
	{
		int arrivalCount = Random.Range(2, 6);
		List<Villager> arrivalsChoices = new List<Villager>();

		for (int i = 0; i < arrivalCount; i++) {
			arrivalsChoices.Add(CreateRandomVillager());
		}

		return arrivalsChoices.ToArray();
	}

	Villager CreateRandomVillager()
	{
        Villager villager = new Villager();
		villager.FoodProduction = Mathf.Round(Random.Range(
			         Mathf.Min(0 + Mathf.Floor(100 / StatsManagerObj.FoodProduction), 5), 
		             Mathf.Max(10 - Mathf.Floor(StatsManagerObj.FoodProduction / 100), 5)
					 ));
		
		villager.WoodProduction = Mathf.Round(Random.Range(
			         Mathf.Min(0 + Mathf.Floor(100 / StatsManagerObj.WoodProduction), 5), 
		             Mathf.Max(10 - Mathf.Floor(StatsManagerObj.WoodProduction / 100), 5)
					 ));
		
		villager.FaithProduction = Mathf.Round(Random.Range(
			         Mathf.Min(0 + Mathf.Floor(100 / StatsManagerObj.FaithProduction), 5), 
		             Mathf.Max(10 - Mathf.Floor(StatsManagerObj.FaithProduction / 100), 5)
					 ));

        return villager;
	}

	void CalculateLimits()
	{
		m_VillagerLimit = Mathf.Min(StatsManagerObj.Houses * 4, Mathf.Floor(StatsManagerObj.FoodProduction * 3f / 8f));
		m_MaxSacrifices = Mathf.Max(1, Mathf.Min(8, 1 + Mathf.Floor(StatsManagerObj.Villagers.Count / 4f) + Mathf.Floor((float)m_TotalMonths / 24f)));
        
	}

	void CalculateSacrificePeriod()
	{
		m_SacrificePeriod = Mathf.Min(12, SacrificePeriodBase + Mathf.Floor(StatsManagerObj.FaithProduction / 30f));
	}

	void CalculateNextArrival()
	{
		m_NextArrival = (m_CurrentMonth + ArrivalPeriod) % 12;
	}

	float[] CalculateSacrificeWorth(List<Villager> sacrifices)
	{
		float[] values = new float[3];

		for (int i = 0; i < sacrifices.Count; i++) {
			values[0] += Mathf.Floor(sacrifices[i].FoodProduction  - 6f);
			values[1] += Mathf.Floor(sacrifices[i].WoodProduction  - 6f);
			values[2] += Mathf.Floor(sacrifices[i].FaithProduction - 6f);
		}

		return values;
	}

	SacrificeResult CalculateSacrificeOutcome()
	{
		int index = 0;
		float temp = 0f;
		float[] sacrificesValues = new float[3];

		sacrificesValues = CalculateSacrificeWorth(StatsManagerObj.SacrificesValues);

		List<SacrificeResult> possibleResults = new List<SacrificeResult>();

		for (int i = 0; i < sacrificesValues.Length; i++) {
			if (temp <= Mathf.Abs(sacrificesValues[i])){
				index = i;
				temp = Mathf.Abs(sacrificesValues[i]);
			}
		}

		switch (index)
		{
			case 0:
				if (sacrificesValues[index] <= 0)
					possibleResults = m_SacrificeManaging.FoodNegativeEffects;
				else
					possibleResults = m_SacrificeManaging.FoodPositiveEffects;
				break;
			case 1:
				if (sacrificesValues[index] <= 0)
					possibleResults = m_SacrificeManaging.WoodNegativeEffects;
				else
					possibleResults = m_SacrificeManaging.WoodPositiveEffects;
				break;
			case 2:
				if (sacrificesValues[index] <= 0)
					possibleResults = m_SacrificeManaging.FaithNegativeEffects;
				else
					possibleResults = m_SacrificeManaging.FaithPositiveEffects;
				break;
		}

        StatsManagerObj.SacrificesValues.Clear();

		int resultIndex = Random.Range(0, possibleResults.Count);

		return possibleResults[resultIndex];
	}
}
