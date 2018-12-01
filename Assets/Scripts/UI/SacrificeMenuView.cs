using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SacrificeMenuView : MonoBehaviour
{
    public Text SacrificeCountText;
    public Transform Content;

    [SerializeField]
    private GameObject m_VillagerViewPrefab;

    private int m_CurrentSacrificeCount;
    private int m_SacrificeCount;

    public event Action<Villager> OnSacrifice;
    public event Action OnSacrificeFulfill;

    public void AddVillagerView(Villager villager, int index)
    {
        VillagerView villagerView = Instantiate(m_VillagerViewPrefab, Content).GetComponent<VillagerView>();
        villagerView.SetStats((int)villager.WoodProduction, (int)villager.FoodProduction, (int)villager.FaithProduction);
        villagerView.Index = index;
        villagerView.OnActionButton += (i) => { Sacrifice(villager); } ;
    }

    public void SetSacrificeCount(int count)
    {
        m_CurrentSacrificeCount = 0;
        m_SacrificeCount = count;
    }

    void Sacrifice(Villager villager)
    {
        m_CurrentSacrificeCount++;
        OnSacrifice?.Invoke(villager);

        if (m_CurrentSacrificeCount == m_SacrificeCount)
        {
            CloseView();
        }
    }

    public void CloseView()
    {
        OnSacrificeFulfill?.Invoke();
        Destroy(this.gameObject);
    }
}
