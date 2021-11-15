using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : Combat
{
    private Enemy _enemy;
    public bool attack = false;
    //private Animator animator;
    private void Start()
    {
        _enemy = gameObject.GetComponent<Enemy>();
        //hitEnemies = new List<Collider>();
        //hitEnemies.Add( GameManager.Instance.PlayerInstance.GetComponent<Collider>() );
        //animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    attack = true;
        //}
        //else attack = false;
        if (attack)
        {
            Attack();
        }
    }

    public virtual void Attack()
    {
        if(gameObject.TryGetComponent<BossAI>(out var bossAI))
        {
            bossAI.Animations.AttackAnimation();
        }
        else
        {
            _enemy.Animations.AttackAnimation();
        }
    }

    public virtual void OnAttack()
    {
        DoDamage();
    }

    public override void DoDamage()
    {
        base.DoDamage();

        // Damage them
        foreach (Collider Player in hitEnemies)
        {
            if (gameObject.TryGetComponent<BossAI>(out var bossAI))
            {
                bossAI.Animations.AttackAnimation();
                if (GameManager.Instance.PlayerInstance != null)
                    Player.gameObject.GetComponent<Player_Controller>().Life_Controller.GetDamage(bossAI.Enemy.Stats.MeleeDamage);
            }
            else
            {
                if (GameManager.Instance.PlayerInstance != null)
                    Player.gameObject.GetComponent<Player_Controller>().Life_Controller.GetDamage(_enemy.Stats.MeleeDamage);
            }
        }
    }
}
