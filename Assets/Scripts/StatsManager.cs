using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager
{
	public List<Villager> Villagers;

	public float Houses = 0.0f;
	public float FoodProduction = 0.0f;
	public float WoodProduction = 0.0f;
	public float FaithProduction = 0.0f;

	public float FoodModifier = 0.0f;
	public float WoodModifier = 0.0f;
	public float FaithModifier = 0.0f;
	public float TotalFaith = 0.0f;

	float m_TotalFood = 0.0f;
	float m_TotalWood = 0.0f;

	public StatsManager(){
		Villagers = new List<Villager>();
	}

	public void UpdateStats(){
		CalculateTotalStats();
		CalculateProductions();
		CalculateHouses();
	}

	void CalculateHouses(){
		Houses = Mathf.Floor(WoodProduction / 25);
	}

	void CalculateTotalStats(){
		for (int i = 0; i < Villagers.Count; i++){
			m_TotalFood  += Villagers[i].FoodProduction;
			m_TotalWood  += Villagers[i].WoodProduction;
			TotalFaith += Villagers[i].FaithProduction;
		}
	}

	void CalculateProductions(){
		FoodProduction  = m_TotalFood  * FoodModifier;
		WoodProduction  = m_TotalWood  * WoodModifier;
		FaithProduction = TotalFaith * FaithModifier;
	}
}
