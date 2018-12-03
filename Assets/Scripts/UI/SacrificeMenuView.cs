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

    private int m_CurrentSacrificeCount = 0;
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
        /* if (m_CurrentSacrificeCount == 0)
            this.GetComponentInChildren<Button>().interactable = false;
        */  
        m_SacrificeCount = count;
        SacrificeCountText.text = string.Format("{0}/{1}", m_CurrentSacrificeCount, m_SacrificeCount);
    }

    public int GetSacrificeCountLeft()
    {
        return m_SacrificeCount - m_CurrentSacrificeCount;
    }

    void Sacrifice(Villager villager)
    {
        m_CurrentSacrificeCount++;
        SacrificeCountText.text = string.Format("{0}/{1}", m_CurrentSacrificeCount, m_SacrificeCount);

        OnSacrifice?.Invoke(villager);

        this.GetComponentInChildren<Button>().interactable = true;

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
