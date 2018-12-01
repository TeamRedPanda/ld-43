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

    public event Action<Villager> OnRecruit;
    public event Action OnRecruitFulfill;

    public void AddVillagerView(Villager villager)
    {
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
    }

    public void CloseView()
    {
        OnRecruitFulfill?.Invoke();
        Destroy(this.gameObject);
    }
}
