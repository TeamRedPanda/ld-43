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

	public float SacrificePeriodBase = 6;
    private float SacrificePeriod;
    private float m_NextSacrifice;

	public float ArrivalPeriod = 3;
	private float m_NextArrival;

    public GameObject RecruitMenu;
	public GameObject SacrificeMenu;
	public Transform Canvas;

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
		StatsManagerObj = new StatsManager();
		GetInitialState();

		StatsManagerObj.UpdateStats();
		CalculateSacrificePeriod();
		CalculateLimits();
		CheckVillageCapacity();
        UpdateResourceView();

        m_NextArrival = ArrivalPeriod;
		m_NextSacrifice = (m_CurrentMonth + SacrificePeriod) % 12;

        m_CurrentMonthTimeRemaining = SecondsPerMonth;
	}

    void UpdateResourceView()
    {
        m_ResourceListView.UpdateWood((int)StatsManagerObj.WoodProduction);
        m_ResourceListView.UpdateFood((int)StatsManagerObj.FoodProduction);
        m_ResourceListView.UpdateFaith((int)StatsManagerObj.FaithProduction);
        m_ResourceListView.UpdatePopulation((int)StatsManagerObj.Villagers.Count, (int)m_VillagerLimit); // TODO: Wtf.
    }

	void Update()
	{
        if (m_IsPaused)
            return;

		m_CurrentMonthTimeRemaining -= Time.deltaTime;
		if (m_CurrentMonthTimeRemaining <= 0) {
			m_CurrentMonth = (m_CurrentMonth + 1) % 12;
            m_ResourceListView.UpdateDate(m_CurrentMonth, 1501); // TODO: WE NEED YEARS!!!!
			m_CurrentMonthTimeRemaining = SecondsPerMonth;

			if (m_CurrentMonth == m_NextSacrifice) {
				m_WindowsOpen++;

				SacrificeMenuView sacrificeView = Instantiate(SacrificeMenu, Canvas).GetComponent<SacrificeMenuView>();

				for (int i = 0; i < StatsManagerObj.Villagers.Count; i++) {
					sacrificeView.AddVillagerView(StatsManagerObj.Villagers[i], i);
				}

				sacrificeView.SetSacrificeCount((int)m_MaxSacrifices);
				sacrificeView.OnSacrifice += Sacrifice;
                sacrificeView.OnSacrificeFulfill += () => {
                    Pause(false);
                    CalculateSacrificePeriod();
                    m_NextSacrifice = (m_CurrentMonth + SacrificePeriod) % 12;
                    m_WindowsOpen--;
                };
			}

			if (m_CurrentMonth == m_NextArrival) {
                m_WindowsOpen++;

                Villager[] arrivals = GenerateVillagers();

				RecruitMenuView recruitView = Instantiate(RecruitMenu, Canvas).GetComponent<RecruitMenuView>();
				recruitView.AddVillagerViews(arrivals);
				recruitView.OnRecruit += Recruit;
                recruitView.OnRecruitFulfill += () => { Pause(false); CheckVillageCapacity(); m_WindowsOpen--; };
			}
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
	}

	Villager[] GenerateVillagers()
	{
		int arrivalCount = Random.Range(1, 3);
		List<Villager> arrivalsChoices = new List<Villager>();

		for (int i = 0; i < arrivalCount; i++) {
			arrivalsChoices.Add(CreateRandomVillager());
		}

		return arrivalsChoices.ToArray();
	}

	Villager CreateRandomVillager()
	{
        Villager villager = new Villager();
		villager.FoodProduction = Random.Range(
			         Mathf.Min(0 + Mathf.Floor(100 / StatsManagerObj.FoodProduction), 5), 
		             Mathf.Max(10 - Mathf.Floor(StatsManagerObj.FoodProduction / 100), 5)
					 );
		
		villager.WoodProduction = Random.Range(
			         Mathf.Min(0 + Mathf.Floor(100 / StatsManagerObj.WoodProduction), 5), 
		             Mathf.Max(10 - Mathf.Floor(StatsManagerObj.WoodProduction / 100), 5)
					 );
		
		villager.FaithProduction = Random.Range(
			         Mathf.Min(0 + Mathf.Floor(100 / StatsManagerObj.FaithProduction), 5), 
		             Mathf.Max(10 - Mathf.Floor(StatsManagerObj.FaithProduction / 100), 5)
					 );

        return villager;
	}

	void CalculateLimits()
	{
		m_VillagerLimit = Mathf.Min(StatsManagerObj.Houses * 4, Mathf.Floor(StatsManagerObj.FoodProduction / 15));
		m_MaxSacrifices = Mathf.Max(1, Mathf.Min(8, 1 + Mathf.Floor(StatsManagerObj.Villagers.Count / 4)));
        
	}

	void CalculateSacrificePeriod()
	{
		SacrificePeriod = Mathf.Min(12, SacrificePeriodBase + Mathf.Floor(StatsManagerObj.FaithProduction / 30));
	}

	void CheckVillageCapacity()
	{
		//if (StatsManagerObj.Villagers.Count < m_VillagerLimit)
			m_NextArrival = (m_CurrentMonth + ArrivalPeriod) % 12;
		//else
		//	m_NextArrival = 0;
	}

	void Recruit(Villager villager)
	{
		StatsManagerObj.Recruit(villager);

		CheckVillageCapacity();
        UpdateResourceView();
    }

	void Sacrifice(Villager villager)
	{
		StatsManagerObj.Sacrifice(villager);

		CalculateSacrificePeriod();
        UpdateResourceView();
    }

	public void Pause(bool value){
		//m_IsPaused = value;
	}
}
