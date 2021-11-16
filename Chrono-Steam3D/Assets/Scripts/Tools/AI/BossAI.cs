using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ELineOfSight), typeof(Seek), typeof(Flee))]
[RequireComponent(typeof(ObstacleAvoidance), typeof(Enemy), typeof(EnemyCombat))]
[RequireComponent(typeof(EnemyAnimations))]
public class BossAI : MonoBehaviour
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
    Roulette _roulette;
    ActionNode tBall ;
    ActionNode clap ;
    ActionNode sGround;
    ActionNode charge;
    ActionNode rouletteAction;
    Dictionary<Node, int> _rouletteNodes = new Dictionary<Node, int>();
    public float defaultAttackTime = 10f;
    float currentAttackTime = 0;

    public Enemy Enemy { get => enemy; set => enemy = value; }
    public EnemyAnimations Animations { get => animations; set => animations = value; }

    private void Start()
    {
        sight = gameObject.GetComponent<ELineOfSight>();
        seek = gameObject.GetComponent<Seek>();
        flee = gameObject.GetComponent<Flee>();
        obstacleavoidance = gameObject.GetComponent<ObstacleAvoidance>();
        animations = GetComponent<EnemyAnimations>();
        enemy = gameObject.GetComponent<Enemy>();
        combat = gameObject.GetComponent<EnemyCombat>();
        RouletteWheel();
        CreateDecisionTree();
        GameManager.Instance.LvlManager.GetComponent<LevelManager>().BossInstance = enemy;
    }

    private void Update()
    {
        if (!enemy.Dead)
        {
            initialNode.Execute();
            currentAttackTime += 1 * Time.deltaTime;
        }
    }

    private void CreateDecisionTree()
    {
        ActionNode Ability = new ActionNode(Abilities);
        ActionNode Hit = new ActionNode(Attack);
        ActionNode Flee = new ActionNode(Fleeing);
        ActionNode Patrol = new ActionNode(Patroling);

        QuestionNode doIHaveAbility = new QuestionNode(() => currentAttackTime >= defaultAttackTime, Ability, Hit);

        QuestionNode doIHaveTarget = new QuestionNode(() => sight.targetInSight && !enemy.Player.Life_Controller.isDead 
        || enemy.Hurt && !enemy.Player.Life_Controller.isDead, doIHaveAbility, Patrol);

        QuestionNode doIHaveHealth = new QuestionNode(() => (enemy.Life_Controller.CurrentLife) <= 0f, Flee, doIHaveTarget);

        initialNode = doIHaveHealth;
    }

    private void Abilities()
    {
        flee.move = false;
        seek.move = false;
        obstacleavoidance.move = false;
        gameObject.GetComponent<BigEnemyAI>().BossCharge = false;

        if (currentAttackTime >= defaultAttackTime)
        {
            RouletteAction();
            currentAttackTime = 0;
        }
    }

    private void Attack()
    {
        obstacleavoidance.move = false;
        flee.move = false;
        currentAttackTime += 1 * Time.deltaTime;

        if (Vector3.Distance(transform.position, sight.Target.position) > attackRange )
        {
            if (gameObject.TryGetComponent<BigEnemyAI>(out var bullCharge))
            {
                /*if (bullCharge.Charge1)
                {
                    seek.move = false;
                }
                else
                {
                    seek.move = true;
                }*/
                 animations.RunAnimation();
            }else
            {
                seek.move = true;
                //animations.MovingAnimation(true);
                animations.RunAnimation();
            }
            //animations.MovingAnimation(true);
        }
        else
        {
            seek.move = false;
            //animations.MovingAnimation();
        }
    }

    private void Fleeing()
    {
        seek.move = false;
        obstacleavoidance.move = false;

        flee.move = false;
        if (enemy.Dead)
        {
            flee.move = false;
        }
    }

    private void Patroling()
    {
        flee.move = false;
        seek.move = false;
        obstacleavoidance.move = true;
        animations.MovingAnimation(true);
    }

    private void TeslaBall()
    {
        flee.move = false;
        seek.move = false;
        animations.Attack2Animation();
        Debug.Log("TeslaBall");
    }

    private void Clap()
    {
        flee.move = false;
        seek.move = false;
        animations.AttackAnimation();
        Debug.Log("Clap");
    }

    private void SmashGround()
    {
        flee.move = false;
        seek.move = false;
        animations.Attack3Animation();
        Debug.Log("SmashGround");
    }
    private void Charge()
    {
        flee.move = false;
        seek.move = false;
        gameObject.GetComponent<BigEnemyAI>().BossCharge = true;
        Debug.Log("Charge");
    }

    //se llama desde el animator 
    public void Attacking()
    {
        seek.move = false;
    }
    public void RouletteWheel()
    {
        _roulette = new Roulette();

        tBall = new ActionNode(TeslaBall);
        clap = new ActionNode(Clap);
        sGround = new ActionNode(SmashGround);
        charge = new ActionNode(Charge);
        rouletteAction = new ActionNode(RouletteAction);
    }

    public void RouletteAction()
    {
        // Debug.Log("Entered in roulette");
        _rouletteNodes.Clear();
        if (enemy.Life_Controller.CurrentLife > enemy.Stats.MaxHealth * 0.66)
        {
            if (!_rouletteNodes.ContainsKey(clap) && !_rouletteNodes.ContainsKey(sGround))
            {
                if (Vector3.Distance(transform.position, sight.Target.position) > attackRange)
                {
                    _rouletteNodes.Add(clap, 75);
                    _rouletteNodes.Add(sGround, 25);
                }else
                {
                    _rouletteNodes.Add(clap, 25);
                    _rouletteNodes.Add(sGround, 75);
                }
            }
        }
        else if (enemy.Life_Controller.CurrentLife> enemy.Stats.MaxHealth * 0.33)
        {
            if (!_rouletteNodes.ContainsKey(charge))
            {
                if (Vector3.Distance(transform.position, sight.Target.position) > attackRange)
                {
                    _rouletteNodes.Add(sGround, 27);
                    _rouletteNodes.Add(clap, 33);
                    _rouletteNodes.Add(charge, 40);
                }
                else
                {
                    _rouletteNodes.Add(clap, 27);
                    _rouletteNodes.Add(charge, 33);
                    _rouletteNodes.Add(sGround, 40);
                }
            }
        }
        else 
        {
            if(!_rouletteNodes.ContainsKey(tBall))
            {
                if (Vector3.Distance(transform.position, sight.Target.position) > attackRange)
                {
                    _rouletteNodes.Add(sGround, 10);
                    _rouletteNodes.Add(clap, 30);
                    _rouletteNodes.Add(charge, 15);
                    _rouletteNodes.Add(tBall, 45);
                }
                else
                {
                    _rouletteNodes.Add(clap, 10);
                    _rouletteNodes.Add(charge, 30);
                    _rouletteNodes.Add(sGround, 45);
                    _rouletteNodes.Add(tBall, 15);
                }
            }
        }
        Debug.Log(_rouletteNodes.Count);
        Node nodeRoulette = _roulette.Run(_rouletteNodes);

        nodeRoulette.Execute();
    }
}
