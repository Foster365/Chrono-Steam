using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyCombat : EnemyCombat
{
    [SerializeField] private GameObject _enemyBulletPrefab;
    [SerializeField] private float attPerSec = 3;
    private float coolDown = 0;

    public GameObject EnemyBulletPrefab { get => _enemyBulletPrefab;}
    public override void OnAttack()    
    {
        if (attack && coolDown <= 0)
        {
           // Debug.Log("enemigo atacando");
            Instantiate(EnemyBulletPrefab, attackPoint.transform.position, attackPoint.transform.rotation);
            coolDown = attPerSec;
        }
        else coolDown -= Time.deltaTime;
    }
}