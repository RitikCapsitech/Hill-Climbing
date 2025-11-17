using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Coin Prefabs")]
    public GameObject coin5Prefab;
    public GameObject coin10Prefab;
    public GameObject coin20Prefab;

    [Header("Settings")]
    public int coinCount = 20;
    public float heightOffset = 0.3f;
    public float minSpacing = 2f;

    private EdgeCollider2D edge;
    private List<Vector3> placedCoins = new List<Vector3>();

    void Awake()
    {
        edge = GetComponent<EdgeCollider2D>();
    }

    void Start()
    {
        SpawnCoins();
    }

    void SpawnCoins()
    {
        if (edge == null) edge = GetComponent<EdgeCollider2D>();

        placedCoins.Clear();

        Vector2[] pts = edge.points;
        Vector3 worldOffset = transform.position;

        int count = 0;
        int attempts = 0;

        while (count < coinCount && attempts < coinCount * 10)
        {
            attempts++;

          
            int idx = Random.Range(0, pts.Length - 1);
            Vector2 a = pts[idx];
            Vector2 b = pts[idx + 1];

            float t = Random.Range(0f, 1f);
            Vector2 pos = Vector2.Lerp(a, b, t) + Vector2.up * heightOffset;
            Vector3 spawnPos = (Vector3)pos + worldOffset;

           
            bool close = false;
            foreach (var p in placedCoins)
            {
                if (Vector3.Distance(p, spawnPos) < minSpacing)
                {
                    close = true;
                    break;
                }
            }

            if (close) continue;

           
            GameObject chosenCoin = PickCoinPrefab();
            Instantiate(chosenCoin, spawnPos, Quaternion.identity);

            placedCoins.Add(spawnPos);
            count++;
        }
    }

    GameObject PickCoinPrefab()
    {
        int r = Random.Range(0, 3);
        switch (r)
        {
            case 0: return coin5Prefab;
            case 1: return coin10Prefab;
            default: return coin20Prefab;
        }
    }

    public void RespawnCoins()
    {
        
        foreach (var coin in GameObject.FindGameObjectsWithTag("Coin"))
        {
            Destroy(coin);
        }

        StartCoroutine(RespawnNextFrame());
    }

    IEnumerator RespawnNextFrame()
    {
        yield return null;  
        SpawnCoins();
    }
}
