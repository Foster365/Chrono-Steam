//using System;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //Enemy SpawnPoints
    GameObject[] spawnPoints;
    //
    //Enemy Abstract Factory
    EnemySpawner enemySpawner;
    Transform _transform;
    //

    [SerializeField] GameObject[] prefabs;
    GameObject prefab;
    float spawn = .5f;
    float timer = 0;
    [SerializeField]
    int maxEnemyQuantity = 10;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Start()
    {
        enemySpawner = new EnemySpawner();

        spawnPoints= GameObject.FindGameObjectsWithTag("SpawnPoint");


    }

    private void Update()
    {
        timer += Time.deltaTime;//Update, si se cumple la condicion entra al if
        if (timer >= spawn) CreateEnemies();
    }

    #region Enemy_Abstract_Factory
    void CreateEnemies()
    {
        GameObject prefab;

        foreach (var p in prefabs)
        {
            prefab = p;
            foreach (var sp in spawnPoints)
            {
                if(maxEnemyQuantity>=0)
                {
                    prefab = prefabs[Random.Range(0, prefabs.Length - 1)];
                    enemySpawner.CreateEnemy(prefab);
                    prefab.transform.position = sp.transform.position;
                }

                    maxEnemyQuantity--;
            }

            //Debug.Log("Enemy clones" + maxEnemyQuantity);
        }

    }
    #endregion
}
