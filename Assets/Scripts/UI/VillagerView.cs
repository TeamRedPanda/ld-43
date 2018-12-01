using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class VillagerView : MonoBehaviour
{
    public Text WoodText;
    public Text FoodText;
    public Text FaithText;

    public int Index;

    public event Action<int> OnActionButton;

    public void SetStats(int wood, int food, int faith)
    {
        WoodText.text = wood.ToString();
        FoodText.text = food.ToString();
        FaithText.text = faith.ToString();
    }

    public void Action()
    {
        OnActionButton?.Invoke(Index);
    }
}
