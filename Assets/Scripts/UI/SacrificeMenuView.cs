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

    public event Action<int> OnSacrifice;
    public event Action OnSacrificeFulfill;

    public void AddVillagerView(Villager villager, int index)
    {
        VillagerView villagerView = Instantiate(m_VillagerViewPrefab, Content).GetComponent<VillagerView>();
        villagerView.SetStats((int)villager.WoodProduction, (int)villager.FoodProduction, (int)villager.FaithProduction);
        villagerView.Index = index;
        villagerView.OnActionButton += Sacrifice;
    }

    public void SetSacrificeCount(int count)
    {
        m_CurrentSacrificeCount = 0;
        m_SacrificeCount = count;
    }

    void Sacrifice(int index)
    {
        m_CurrentSacrificeCount++;
        OnSacrifice?.Invoke(index);

        if (m_CurrentSacrificeCount == m_SacrificeCount)
            OnSacrificeFulfill?.Invoke();
    }

    public void CloseView()
    {
        Clear();
        OnSacrificeFulfill?.Invoke();
    }

    void Clear()
    {

    }
}
