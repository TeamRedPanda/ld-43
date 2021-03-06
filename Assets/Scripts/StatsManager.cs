﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
	
	public List<Villager> SacrificesValues;

	float m_TotalFood = 0.0f;
	float m_TotalWood = 0.0f;
	float m_TotalFaith = 0.0f;

    VillagerGraphics m_VillagerGraphics;

    public event Action OnStatsChanged;

	public StatsManager(VillagerGraphics villagerGraphics)
	{
		Villagers = new List<Villager>();
        m_VillagerGraphics = villagerGraphics;

    }

    public void Recruit(Villager villager)
    {
        Villagers.Add(villager);
        m_VillagerGraphics.Spawn();
        UpdateStats();
        OnStatsChanged?.Invoke();
    }

    public void Sacrifice(Villager villager)
    {
		SacrificesValues.Add(villager);

        Villagers.Remove(villager);
        m_VillagerGraphics.Despawn();
        UpdateStats();
        OnStatsChanged?.Invoke();
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

					if (FoodModifier <= 0f) {
						FoodModifier = 0f;
						break;
					}

					break;
				case ResourceType.Wood:
					WoodModifier  += modifiers[i].Amount * 0.01f;

					if (WoodModifier <= 0f) {
						WoodModifier = 0f;
						break;
					}

					break;
				case ResourceType.Faith:
					FaithModifier += modifiers[i].Amount * 0.01f;

					if (FaithModifier <= 0f) {
						FaithModifier = 0f;
						break;
					}

					break;
			}
		}

		UpdateStats();
	}
}
