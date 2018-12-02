using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceListView : MonoBehaviour
{
    public Text WoodText;
    public Text FoodText;
    public Text FaithText;
    public Text PopulationText;

    public Text DateText;

    static string[] s_MonthList = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

	public void UpdateWood(int amount, float modifier = 0)
    {
        WoodText.text = string.Format("{0} ({1}%)", amount, Mathf.RoundToInt((modifier - 1) * 100));
    }

    public void UpdateFood(int amount, float modifier = 0)
    {
        FoodText.text = string.Format("{0} ({1}%)", amount, Mathf.RoundToInt((modifier - 1) * 100));
    }

    public void UpdateFaith(int amount, float modifier = 0)
    {
        FaithText.text = string.Format("{0} ({1}%)", amount, Mathf.RoundToInt((modifier - 1) * 100));
    }

    public void UpdatePopulation(int current, int max)
    {
        PopulationText.text = string.Format("{0}/{1}", current, max);
    }

    public void UpdateDate(int month, int year)
    {
        DateText.text = string.Format("{0}, {1} D.C.", s_MonthList[month], year);
    }
}
