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
    void Awake()
    {
        UsedPoints = new List<GameObject>();

        foreach(var point in EmptyPoints)
        {
            point.GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }

    public void Spawn()
    {
        if (EmptyPoints.Count == 0)
            return;

        int index = Random.Range(0, EmptyPoints.Count);
        GameObject parent = EmptyPoints[index];
        EmptyPoints.RemoveAt(index);
        UsedPoints.Add(parent);

        int prefabIndex = Random.Range(0, VillagerPrefabs.Length);
        GameObject prefab = VillagerPrefabs[prefabIndex];

        Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        Instantiate(prefab, parent.transform.position, rotation, parent.transform);
    }

    public void Despawn()
    {
        if (UsedPoints.Count == 0)
            return;

        int index = Random.Range(0, UsedPoints.Count);

        // Remove the villager
        foreach(Transform child in UsedPoints[index].transform)
        {
            Destroy(child.gameObject);
        }

        EmptyPoints.Add(UsedPoints[index]);
        UsedPoints.RemoveAt(index);
    }
}
