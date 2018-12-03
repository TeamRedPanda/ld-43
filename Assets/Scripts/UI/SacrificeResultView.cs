using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SacrificeResultView : MonoBehaviour
{
    public Text Title;
    public Image Icon;
    public Text Description;

    public event Action OnWindowClose;

    public void Initialize(string title, Sprite icon, string text)
    {
        Title.text = title;
        Icon.sprite = icon;
        Description.text = text;
    }

    public void CloseView()
    {
        OnWindowClose?.Invoke();
        Destroy(this.gameObject);
    }
}
