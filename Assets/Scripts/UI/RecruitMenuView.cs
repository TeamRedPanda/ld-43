using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RecruitMenuView : MonoBehaviour
{
    public Transform Content;

    [SerializeField]
    private GameObject m_VillagerViewPrefab;

    private int m_Count;

    public event Action<Villager> OnRecruit;
    public event Action OnRecruitFulfill;

    public void AddVillagerView(Villager villager)
    {
        m_Count++;
        VillagerView villagerView = Instantiate(m_VillagerViewPrefab, Content).GetComponent<VillagerView>();
        villagerView.SetStats((int)villager.WoodProduction, (int)villager.FoodProduction, (int)villager.FaithProduction);
        villagerView.OnActionButton += (i) => { Recruit(villager); };
    }

    public void AddVillagerViews(Villager[] villagers)
    {
        foreach (var villager in villagers)
            AddVillagerView(villager);
    }

    void Recruit(Villager villager)
    {
        OnRecruit?.Invoke(villager);
        m_Count--;

        if (m_Count == 0)
            CloseView();
    }

    public void CloseView()
    {
        OnRecruitFulfill?.Invoke();
        Destroy(this.gameObject);
    }
}
