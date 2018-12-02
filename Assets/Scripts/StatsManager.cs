using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatsManager
{
	public List<Villager> Villagers;

	public int Houses = 0;
	public float FoodProduction = 0.0f;
	public float WoodProduction = 0.0f;
	public float FaithProduction = 0.0f;

	public float FoodModifier = 1.0f;
	public float WoodModifier = 1.0f;
	public float FaithModifier = 1.0f;

	float m_TotalFood = 0.0f;
	float m_TotalWood = 0.0f;
	float m_TotalFaith = 0.0f;

	public StatsManager()
	{
		Villagers = new List<Villager>();
	}

    public void Recruit(Villager villager)
    {
        Villagers.Add(villager);
        UpdateStats();
    }

    public void Sacrifice(Villager villager)
    {
        Villagers.Remove(villager);
        UpdateStats();
    }

	public void UpdateStats()
	{
		CalculateTotalStats();
		CalculateProductions();
		CalculateHouses();
	}

	void CalculateHouses()
	{
		Houses = Mathf.CeilToInt(WoodProduction / 25f);
	}

	void CalculateTotalStats()
	{
        m_TotalWood  = 0;
        m_TotalFood  = 0;
        m_TotalFaith = 0;

		for (int i = 0; i < Villagers.Count; i++) {
			m_TotalFood  += Villagers[i].FoodProduction;
			m_TotalWood  += Villagers[i].WoodProduction;
			m_TotalFaith += Villagers[i].FaithProduction;
		}
	}

	void CalculateProductions()
	{
		FoodProduction  = m_TotalFood  * FoodModifier;
		WoodProduction  = m_TotalWood  * WoodModifier;
		FaithProduction = m_TotalFaith * FaithModifier;
	}

	public void ApplyModifier(ResourceModifier[] modifiers)
	{
		for (int i = 0; i < modifiers.Length; i++) {
			switch (modifiers[i].Type)
			{
				case ResourceType.Food:
					FoodModifier  += modifiers[i].Amount * 0.01f;
					break;
				case ResourceType.Wood:
					WoodModifier  += modifiers[i].Amount * 0.01f;
					break;
				case ResourceType.Faith:
					FaithModifier += modifiers[i].Amount * 0.01f;
					break;
			}
		}

		UpdateStats();
	}
}
