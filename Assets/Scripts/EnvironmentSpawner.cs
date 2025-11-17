using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{

    public GameObject cloudPrefab;
    public List<GameObject> spawnedPrefabs = new List<GameObject>();


    [Header("Position Settings")]
    public Vector3 StartPos;
    public Vector3 EndPos;

    [Header("Cloud Settings")]
    public float cloudSpacing = 5f;
    public float minYCloud = 2f;
    public float maxYCloud = 5f;

    void Awake()
    {
        spawnedPrefabs = new List<GameObject>();
    }


    public void spawnEnvironment()
    {

        ClearEnvironment();
        SpawnClouds();
    }


    void SpawnClouds()
    {
        float xStart = StartPos.x;
        float xEnd = EndPos.x;

        for (float x = xStart; x <= xEnd; x += cloudSpacing)
        {
            float y = Random.Range(minYCloud, maxYCloud);
            Vector3 pos = new Vector3(x, y, StartPos.z);
            GameObject cloud = Instantiate(cloudPrefab, pos, Quaternion.identity);
            cloud.transform.localScale = Vector3.one * Random.Range(0.6f, 1.2f);
            spawnedPrefabs.Add(cloud);
        }
    }

    public void ClearEnvironment()
    {
        foreach (GameObject obj in spawnedPrefabs)
        {
            if (obj != null && obj.scene.IsValid())
                Destroy(obj);
        }
        spawnedPrefabs.Clear();
    }

}
