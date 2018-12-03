using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class OverPopulationView : MonoBehaviour
{
    public int Count {
        set {
            UpdateFlavourText(value);
        }
    }

    public Text FlavourText;

    public List<string> FlavourTexts;

    public event Action OnWindowClose;

    void UpdateFlavourText(int count)
    {
        int index = UnityEngine.Random.Range(0, FlavourTexts.Count);
        FlavourText.text = string.Format(FlavourTexts[index], count);
    }

    public void CloseView()
    {
        OnWindowClose?.Invoke();
        Destroy(this.gameObject);
    }
}
