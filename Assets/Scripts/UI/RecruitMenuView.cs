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

    void AddVillagerView(Villager villager, int index)
    {
        VillagerView villagerView = Instantiate(m_VillagerViewPrefab, Content).GetComponent<VillagerView>();
        villagerView.SetStats((int)villager.WoodProduction, (int)villager.FoodProduction, (int)villager.FaithProduction);
        villagerView.Index = index;
        villagerView.OnActionButton += (i) => { Recruit(villager); };
    }

    void Recruit(Villager villager)
    {
        OnRecruit?.Invoke(villager);
    }

    public void CloseView()
    {
        Clear();
        OnRecruitFulfill?.Invoke();
    }

    void Clear()
    {

    }
}
