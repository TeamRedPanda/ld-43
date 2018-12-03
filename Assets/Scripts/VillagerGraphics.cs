using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerGraphics : MonoBehaviour
{
    public GameObject[] VillagerPrefabs;

    [SerializeField]
    private List<GameObject> EmptyPoints;
    private List<GameObject> UsedPoints;

    // Use this for initialization
    void Start()
    {
        UsedPoints = new List<GameObject>();

        foreach(var point in EmptyPoints)
        {
            point.GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }

    public void Spawn()
    {
        int index = Random.Range(0, EmptyPoints.Count);
    }

    public void Despawn()
    {
        int index = Random.Range(0, EmptyPoints.Count);
    }
}
