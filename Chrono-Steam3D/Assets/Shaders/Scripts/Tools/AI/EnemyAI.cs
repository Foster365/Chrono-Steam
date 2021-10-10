using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ELineOfSight), typeof(Seek), typeof(Flee))]
[RequireComponent(typeof(ObstacleAvoidance), typeof(Enemy), typeof(EnemyCombat))]
[RequireComponent(typeof(EnemyAnimations))]
public class EnemyAI : MonoBehaviour
{
    private Node initialNode;
    ELineOfSight sight;
    Seek seek;
    Flee flee;
    ObstacleAvoidance obstacleavoidance;
    private Enemy enemy;
    EnemyCombat combat;
    private bool attackTarget;
    private EnemyAnimations animations;
    [SerializeField] float attackRange;

    public Enemy Enemy { get => enemy; set => enemy = value; }
    public EnemyAnimations Animations { get => animations; set => animations = value; }

    private void Awake()
    {
        CreateDecisionTree();
    }

    private void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        sight = gameObject.GetComponent<ELineOfSight>();
        seek = gameObject.GetComponent<Seek>();
        flee = gameObject.GetComponent<Flee>();
        obstacleavoidance = gameObject.GetComponent<ObstacleAvoidance>();
        animations = GetComponent<EnemyAnimations>();
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
            seek.move = false;
            combat.attack = false;
            obstacleavoidance.move = false;
        }
    }
    private void CreateDecisionTree()
    {
        ActionNode Hit = new ActionNode(Attack);
        ActionNode Flee = new ActionNode(Fleeing);
        ActionNode Patrol = new ActionNode(Patroling);

        QuestionNode doIHaveTarget = new QuestionNode(() => sight.targetInSight && !enemy.Player.Life_Controller.isDead 
        || enemy.Hurt && !enemy.Player.Life_Controller.isDead, Hit, Patrol);

        QuestionNode doIHaveHealth = new QuestionNode(() => (enemy.Life_Controller.CurrentLife) <= 0f, Flee, doIHaveTarget);

        initialNode = doIHaveHealth;
    }

    private void Attack()
    {
        obstacleavoidance.move = false;
        flee.move = false;
        if (enemy.Life_Controller.CurrentLife > 0)
        {
            if (sight.Target != null)
            {
                if (Vector3.Distance(transform.position, sight.Target.position) > attackRange)
                {
                    if (!combat.attack)
                    {
                        if (gameObject.TryGetComponent<BullCharge>(out var bullCharge))
                        {
                            if (bullCharge.Charge1)
                            {
                                seek.move = false;
                            }
                            else
                            {
                                seek.move = true;
                            }
                            //animations.MovingAnimation(true);
                            animations.RunAnimation();
                        }
                        else
                        {
                            seek.move = true;
                            //animations.MovingAnimation(true);
                            animations.RunAnimation();
                        }
                    }
                }
                else
                {
                    seek.move = false;
                    //animations.MovingAnimation();
                    combat.attack = true;
                }
            }
        }
        else
        {
            seek.move = false;
            seek.move = false;
            combat.attack = false;
        }
    }

    private void Fleeing()
    {
        seek.move = false;
        obstacleavoidance.move = false;
        combat.attack = false;

        flee.move = false;
        if (enemy.Dead)
        {
            flee.move = false;
            seek.move = false;
            combat.attack = false;
            obstacleavoidance.move = false;
        }
    }

    private void Patroling()
    {
        flee.move = false;
        seek.move = false;
        combat.attack = false;
        obstacleavoidance.move = true;
        animations.MovingAnimation(true);
    }
    //se llama desde el animator 
    public void AttackOver()
    {
        combat.attack = false;
    }
}
