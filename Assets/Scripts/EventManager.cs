using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	StatsManager StatsManagerObj;

	public float SacrificePeriod;
	public float NextArrival;
	public float ArrivalPeriod = 3;
	public float StartTime = 60;
	public float PassageTime = 60;
	public float ActualMonth = 0;
	public float NextSacrifice;

	public GameObject RecruitMenu;
	public GameObject SacrificeMenu;
	public Transform Canvas;

	bool pause = false;

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

        NextArrival = ArrivalPeriod;
		NextSacrifice = (ActualMonth + SacrificePeriod) % 12;

        StartTime = PassageTime;
	}

	void Update()
	{	
		if (pause == false) {
			StartTime -= Time.deltaTime;
			if (StartTime <= 0) {
				ActualMonth = (ActualMonth + 1) % 12;
				StartTime = PassageTime;

				if (ActualMonth == NextSacrifice) {
					pause = true;

					SacrificeMenuView sacrificeView = Instantiate(SacrificeMenu, Canvas).GetComponent<SacrificeMenuView>();

					for (int i = 0; i < StatsManagerObj.Villagers.Count; i++) {
						sacrificeView.AddVillagerView(StatsManagerObj.Villagers[i], i);
					}

					sacrificeView.SetSacrificeCount((int)m_MaxSacrifices);
					sacrificeView.OnSacrifice += Sacrifice;
					sacrificeView.OnSacrificeFulfill += () => Pause(false);
				}

				if (ActualMonth == NextArrival) {
					pause = true;

					Villager[] arrivals = GenerateVillagers();

					RecruitMenuView recruitView = Instantiate(RecruitMenu, Canvas).GetComponent<RecruitMenuView>();
					recruitView.AddVillagerViews(arrivals);
					recruitView.OnRecruit += Recruit;
					recruitView.OnRecruitFulfill += () => Pause(false);
				}
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
		m_MaxSacrifices = Mathf.Min(4, Mathf.Floor(StatsManagerObj.Villagers.Count / 4));
	}

	void CalculateSacrificePeriod()
	{
		Mathf.Min(12, 3 + Mathf.Floor(StatsManagerObj.FaithProduction / 30));
	}

	void CheckVillageCapacity()
	{
		if (StatsManagerObj.Villagers.Count < m_VillagerLimit)
			NextArrival = (ActualMonth + ArrivalPeriod) % 12;
		else
			NextArrival = 0;
	}

	void Recruit(Villager villager)
	{
		StatsManagerObj.Villagers.Add(villager);

		CheckVillageCapacity();
	}

	void Sacrifice(int index)
	{
		StatsManagerObj.Villagers.RemoveAt(index);

		CalculateSacrificePeriod();
	}

	public void Pause(bool value){
		pause = value;
	}
}
