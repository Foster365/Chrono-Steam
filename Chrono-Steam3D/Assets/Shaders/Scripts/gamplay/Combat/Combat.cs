using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour, IEnemyAtack
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask enemyLayers;
    protected Collider[] hitEnemies;

    public float AttackRange { get => attackRange; set => attackRange = value; }

    public virtual void DoDamage()
    {
        // Detect enemies in range of attack
        hitEnemies = Physics.OverlapSphere(attackPoint.position, AttackRange, enemyLayers);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, AttackRange);
    }
}
