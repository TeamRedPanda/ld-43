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

[CreateAssetMenu(menuName = "Village Gods/Create God's Response")]
public class GodsResponse : ScriptableObject
{
    public string Title;
    public Image Icon;

    [TextArea()]
    public string Text;
    public ResourceModifier[] Modifier;
}
