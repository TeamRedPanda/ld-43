using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	StatsManager StatsManagerObj;

	public float SacrificePeriod;
	public float NextArrival;
	public float StartTime;
	public float NextMonth;
	public float NextSacrifice;

	float m_VillagerLimit;
	float m_MaxSacrifices;

	void Start(){
		StatsManagerObj = new StatsManager();
		GetInitialState();

		StartTime = Time.deltaTime;
		NextMonth = StartTime + 60;

		StatsManagerObj.UpdateStats();
		CalculateSacrificePeriod();
		CalculateLimits();

		NextSacrifice = StartTime + SacrificePeriod;
		CheckVillageCapacity();
	}

	void Update(){
		if (Time.deltaTime == NextMonth){
			if(Time.deltaTime == NextSacrifice)
				Debug.Log("Open sacrifice UI");

			if(Time.deltaTime == NextArrival)
				Debug.Log("Open recruit UI")
		}
	}

	void GetInitialState(){
		Debug.Log("Open recruit UI");
	}

	void CalculateLimits(){
		m_VillagerLimit = Mathf.Min(StatsManagerObj.Houses * 4, Mathf.Floor(StatsManagerObj.FoodProduction / 15));
		m_MaxSacrifices = Mathf.Min(4, Mathf.Floor(StatsManagerObj.Villagers.Count / 4));
	}

	void CalculateSacrificePeriod(){
		Mathf.Min(12, 3 + Mathf.Floor(StatsManagerObj.TotalFaith / 30));
	}

	void CheckVillageCapacity(){
		if (StatsManagerObj.Villagers.Count < m_VillagerLimit)
			NextArrival = StartTime + 180;
		else
			NextArrival = 0;
	}

	public void Recruit(){
		Debug.Log("To be implemented after UI")
	}

	public void Sacrifice(int index){
		StatsManagerObj.Villagers.RemoveAt(index);
		Debug.Log("Needs to implement an option of more then one sacrifice")
	}
}
