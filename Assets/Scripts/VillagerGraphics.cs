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
        GameObject parent = EmptyPoints[index];
        EmptyPoints.RemoveAt(index);
        UsedPoints.Add(parent);

        int prefabIndex = Random.Range(0, VillagerPrefabs.Length);
        GameObject prefab = VillagerPrefabs[prefabIndex];

        Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        Instantiate(prefab, Vector3.zero, rotation, parent.transform);
    }

    public void Despawn()
    {
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
