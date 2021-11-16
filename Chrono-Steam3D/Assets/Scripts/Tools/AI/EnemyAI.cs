using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ELineOfSight), typeof(Seek))]
[RequireComponent(typeof(ObstacleAvoidance), typeof(Enemy), typeof(EnemyCombat))]
public class EnemyAI : MonoBehaviour
{
    protected Node initialNode;
    protected ELineOfSight sight;
    protected Seek _seek;
    protected ObstacleAvoidance obstacleavoidance;
    protected Enemy enemy;
    protected EnemyCombat combat;
    protected bool attackTarget;

    public virtual void Awake()
    {
        CreateDecisionTree();
    }

    public virtual void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        sight = gameObject.GetComponent<ELineOfSight>();
        _seek = gameObject.GetComponent<Seek>();
        obstacleavoidance = gameObject.GetComponent<ObstacleAvoidance>();
        combat = gameObject.GetComponent<EnemyCombat>();
    }

    public virtual void Update()
    {
        if (!enemy.Dead)
        {
            initialNode.Execute();
        }else
        {
            
        }
    }
    protected virtual void CreateDecisionTree()
    {
        ActionNode Hit = new ActionNode(Attack);
        ActionNode Patrol = new ActionNode(Patroling);
        ActionNode seek = new ActionNode(Seeking);
        ActionNode dead = new ActionNode(die);

        QuestionNode inAttackRange = new QuestionNode(() => (Vector3.Distance(transform.position, sight.Target.position)) < combat.AttackRange, Hit, seek);

        QuestionNode doIHaveTarget = new QuestionNode(() => (sight.targetInSight) || (enemy.Hurt), inAttackRange, Patrol);

        QuestionNode playerAlive = new QuestionNode(() => !(enemy.Player.Life_Controller.isDead), doIHaveTarget, Patrol);

        QuestionNode doIHaveHealth = new QuestionNode(() => !(enemy.Life_Controller.isDead) , playerAlive, dead);

        initialNode = doIHaveHealth;
    }

    protected virtual void Attack()
    {
        obstacleavoidance.move = false;
        _seek.move = false;

        if (enemy.Life_Controller.CurrentLife > 0)
        {
            if (sight.Target != null)
            {
                //animations.MovingAnimation();
                combat.attack = true;
            }
        }
        else
        {
            combat.attack = false;
        }
    }

    protected virtual void Patroling()
    {
        Debug.Log("patrol");
        _seek.move = false;
        combat.attack = false;
        obstacleavoidance.move = true;
        enemy.Animations.MovingAnimation(true);
    }
    protected virtual void Seeking()
    {
        Debug.Log("seek");
        if (!combat.attack)
        {
            _seek.move = true;
            combat.attack = false;
            obstacleavoidance.move = false;
            enemy.Animations.MovingAnimation(true);
        }
    }
    protected virtual void die()
    {
        _seek.move = false;
        combat.attack = false;
        obstacleavoidance.move = false;
    }
    //se llama desde el animator 
    public void AttackOver()
    {
        combat.attack = false;

        Debug.Log("atack over");
    }
}
