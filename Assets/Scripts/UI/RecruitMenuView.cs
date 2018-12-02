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

    private int m_RecruitCount;
    private int m_CurrentRecruitCount;

    public event Action<Villager> OnRecruit;
    public event Action OnRecruitFulfill;

    public void AddVillagerView(Villager villager)
    {
        m_Count++;
        VillagerView villagerView = Instantiate(m_VillagerViewPrefab, Content).GetComponent<VillagerView>();
        villagerView.SetStats((int)villager.WoodProduction, (int)villager.FoodProduction, (int)villager.FaithProduction);
        villagerView.OnActionButton += (i) => { Recruit(villager); };

        if (m_RecruitCount == 0)
            villagerView.DisableActionButton();
    }

    public void AddVillagerViews(Villager[] villagers)
    {
        foreach (var villager in villagers)
            AddVillagerView(villager);
    }

    public void SetRecruitCount(int count)
    {
        m_CurrentRecruitCount = 0;
        m_RecruitCount = count;
    }

    void Recruit(Villager villager)
    {
        OnRecruit?.Invoke(villager);
        m_Count--;
        m_CurrentRecruitCount++;

        if (m_Count == 0 || m_CurrentRecruitCount == m_RecruitCount)
            CloseView();
    }

    public void CloseView()
    {
        OnRecruitFulfill?.Invoke();
        Destroy(this.gameObject);
    }
}
