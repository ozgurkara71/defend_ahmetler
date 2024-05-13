using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    // If time can set to 0, while may loop forever.
    [SerializeField] [Range(0.1f, 30f)] float spawnTimer = 1f;

    // Object pool
    [SerializeField] [Range(0, 50)] int poolSize = 5;
    [SerializeField] GameObject[] pool;

    void Awake()
    {
        PopulatePool();    
    }

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTimer);
        }
        
    }

    void PopulatePool()
    {
        // That method generates enough enemy units at start and game uses them everytime. 
        // Destroy and create object is payload to system.

        pool = new GameObject[poolSize];
        for(int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(enemyPrefab, gameObject.transform);
            pool[i].SetActive(false);
        }
    }

    void EnableObjectInPool()
    {
        // Set active first deactive object at hierarch and return.
        for(int i = 0; i < pool.Length; i++)
        {
            if (pool[i].activeInHierarchy == false)
            {
                pool[i].SetActive(true);
                return;
            }
        }
    }
}
