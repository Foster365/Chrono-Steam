using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyAI : EnemyAI
{
    Vector3 previousPlayerPos;
    Vector3 direction;
    float playerDistance;
    [SerializeField] float chargeSpeed;
    [SerializeField] float rotSpeed;
    [SerializeField] float chargeRange;
    [SerializeField] float chargeCooldown;
    [SerializeField] float stunDuration;
    bool doingCharge;
    bool collisionWithPlayer;
    private bool bossCharge;
    private float _currentChargeCD;
    float _currentStunDuration;

    public bool BossCharge { get => bossCharge; set => bossCharge = value; }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _currentChargeCD = chargeCooldown;
    }
    // Update is called once per frame
    public override void Update()
    {
        if (!enemy.Dead)
        {
            initialNode.Execute();
            playerDistance = Vector3.Distance(transform.position, enemy.Player.transform.position);
        }
        else
        {
            _seek.move = false;
            combat.attack = false;
            obstacleavoidance.move = false;
        }
        if (collisionWithPlayer)
        {
            collisionWithPlayer = false;
            enemy.Player.Stunned = true;
            _currentStunDuration = stunDuration;
        }
        if (enemy.Player.Stunned)
        {
            if (_currentStunDuration > 0)
                _currentStunDuration -= Time.deltaTime;
            else
                enemy.Player.Stunned = false;
        }
        
        
    }
    protected override void CreateDecisionTree()
    {
        ActionNode Hit = new ActionNode(base.Attack);
        ActionNode Patrol = new ActionNode(Patroling);
        ActionNode seek = new ActionNode(Seeking);
        ActionNode Charge = new ActionNode(Charging);
        ActionNode dead = new ActionNode(base.die);

        QuestionNode inAttackRange = new QuestionNode(() => (playerDistance < combat.AttackRange) && !doingCharge, Hit, seek);

        QuestionNode inChargeRange = new QuestionNode(() => (playerDistance > chargeRange) || (doingCharge), Charge, inAttackRange);

        QuestionNode doIHaveTarget = new QuestionNode(() => (sight.targetInSight), inChargeRange, Patrol);

        QuestionNode playerAlive = new QuestionNode(() => !(enemy.Player.Life_Controller.isDead), doIHaveTarget, Patrol);

        QuestionNode doIHaveHealth = new QuestionNode(() => !(enemy.Life_Controller.isDead), playerAlive, dead);

        initialNode = doIHaveHealth;
    }
    protected override void Attack()
    {
        doingCharge = false;
        base.Attack();
    }
    protected override void Patroling()
    {
        Debug.Log("patrol");
        doingCharge = false;
        _seek.move = false;
        combat.attack = false;
        obstacleavoidance.move = true;
        enemy.Animations.MovingAnimation(true);
    }
    protected override void Seeking()    
    {
        if (!combat.attack && !doingCharge)
        {
            Debug.Log("seek");
            doingCharge = false;
            _seek.move = true;
            combat.attack = false;
            obstacleavoidance.move = false;
            enemy.Animations.MovingAnimation(true);
        }
    }
    void Charging()
    {
        if (_currentChargeCD <= 0)
        {
            Debug.Log("Charge del bull");
            doingCharge = true;
            _seek.move = false;
            combat.attack = false;
            obstacleavoidance.move = false;

            //Consigo el vector entre el objetivo y mi posición
            Vector3 deltaVector = (previousPlayerPos - transform.position).normalized;
            deltaVector.y = 0;
            //Me guardo la dirección unicamente.
            direction = deltaVector;

            //Roto mi objeto hacia la dirección obtenida
            transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * rotSpeed);
            //Muevo mi objeto
            transform.position += transform.forward * chargeSpeed * Time.deltaTime;

            if (Vector3.Distance(previousPlayerPos, transform.position) <= 1f || collisionWithPlayer)
            {
                _currentChargeCD = chargeCooldown;
            }
        }
        else
        {
            _currentChargeCD -= Time.deltaTime;
            if (enemy.Player != null)
            {
                previousPlayerPos = enemy.Player.transform.position;
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" && doingCharge)
        {
            Debug.Log("colishon player");
            collisionWithPlayer = true;
            doingCharge = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, chargeRange);
    }
}
