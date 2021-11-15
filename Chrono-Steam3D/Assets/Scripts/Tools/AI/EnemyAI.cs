using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ELineOfSight), typeof(Seek), typeof(Flee))]
[RequireComponent(typeof(ObstacleAvoidance), typeof(Enemy), typeof(EnemyCombat))]
public class EnemyAI : MonoBehaviour
{
    private Node initialNode;
    ELineOfSight sight;
    Seek _seek;
    Flee flee;
    ObstacleAvoidance obstacleavoidance;
    private Enemy enemy;
    EnemyCombat combat;
    private bool attackTarget;
    [SerializeField] float attackRange;

    public Enemy Enemy { get => enemy; set => enemy = value; }

    private void Awake()
    {
        CreateDecisionTree();
    }

    private void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        sight = gameObject.GetComponent<ELineOfSight>();
        _seek = gameObject.GetComponent<Seek>();
        flee = gameObject.GetComponent<Flee>();
        obstacleavoidance = gameObject.GetComponent<ObstacleAvoidance>();
        combat = gameObject.GetComponent<EnemyCombat>();
        CreateDecisionTree();
    }

    private void Update()
    {
        if (!enemy.Dead)
        {
            initialNode.Execute();
        }else
        {
            flee.move = false;
            _seek.move = false;
            combat.attack = false;
            obstacleavoidance.move = false;
        }
    }
    private void CreateDecisionTree()
    {
        ActionNode Hit = new ActionNode(Attack);
        ActionNode Patrol = new ActionNode(Patroling);
        ActionNode seek = new ActionNode(Seeking);

        QuestionNode inAttackRange = new QuestionNode(() => (Vector3.Distance(transform.position, sight.Target.position)) < attackRange, Hit, seek);

        QuestionNode doIHaveTarget = new QuestionNode(() => (sight.targetInSight) || (enemy.Hurt), inAttackRange, Patrol);

        QuestionNode playerAlive = new QuestionNode(() => (enemy.Player.Life_Controller.isDead), doIHaveTarget, Patrol);

        QuestionNode doIHaveHealth = new QuestionNode(() => (enemy.Life_Controller.CurrentLife) <= 0f, playerAlive, doIHaveTarget);

        initialNode = doIHaveHealth;
    }

    private void Attack()
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
            _seek.move = false;
            combat.attack = false;
        }
    }

    private void Patroling()
    {
        _seek.move = false;
        combat.attack = false;
        obstacleavoidance.move = true;
        enemy.Animations.MovingAnimation(true);
    }
    private void Seeking()
    {
        if (!combat.attack)
        {
            _seek.move = true;
            combat.attack = false;
            obstacleavoidance.move = false;
            enemy.Animations.MovingAnimation(true);
        }
    }
    //se llama desde el animator 
    public void AttackOver()
    {
        combat.attack = false;
    }
}
