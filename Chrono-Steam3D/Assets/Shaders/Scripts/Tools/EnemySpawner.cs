using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    public GameObject CreateEnemy(GameObject enemyPrefab/*, Weapon weaponPrefab, Transform enemyHandPosition*/)
    {

        GameObject enemy = Create(enemyPrefab).GetComponent<GameObject>();
        Debug.Log("Enemy Instance created");

        return enemy;
    }
}
