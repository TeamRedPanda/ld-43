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

	public void UpdateWood(int amount)
    {
        WoodText.text = amount.ToString();
    }

    public void UpdateFood(int amount)
    {
        FoodText.text = amount.ToString();
    }

    public void UpdateFaith(int amount)
    {
        FaithText.text = amount.ToString();
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
