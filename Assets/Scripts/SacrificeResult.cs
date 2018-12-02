using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ResourceModifier
{
    public ResourceType Type;
    public int Amount;
}

[CreateAssetMenu(menuName = "Village Gods/Create Sacrifice Result")]
public class SacrificeResult : ScriptableObject
{
    public string Title;
    public Image Icon;

    [TextArea()]
    public string Text;
    public ResourceModifier[] Modifiers;
}
