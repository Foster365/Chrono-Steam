using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullCharge : MonoBehaviour
{
    ELineOfSight lineOfSight;
    Seek seek;
    Enemy enemy;
    Vector3 previousPlayerPos;
    Vector3 direction;
    [SerializeField] float chargeSpeed;
    [SerializeField] float rotSpeed;
    [SerializeField] float chargeRange;
    [SerializeField] float chargeCooldown;
    bool charge;
    bool chargeFirstTime;
    bool collisionWithPlayer;
    private bool bossCharge;
    float timer;
    float timer2;

    public bool Charge1 { get => charge; set => charge = value; }
    public bool BossCharge { get => bossCharge; set => bossCharge = value; }

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        lineOfSight = gameObject.GetComponent<ELineOfSight>();
        seek = gameObject.GetComponent<Seek>();
        chargeFirstTime = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (collisionWithPlayer)
        {
            timer2 += 1 * Time.deltaTime;
            enemy.Player.Stunned = true;
            if(timer2 >= 3f)
            {
                enemy.Player.Stunned = false;
                collisionWithPlayer = false;
            }
        }
        else
        {
            timer2 = 0;
        }

        if ((Vector3.Distance(transform.position, previousPlayerPos) < chargeRange) && (lineOfSight.Target) && (timer >= chargeCooldown) && (!enemy.Player.Life_Controller.isDead) && (!enemy.Life_Controller.isDead))
        {
            if (gameObject.TryGetComponent<BossAI>(out var bossAI))
            {
                if (bossCharge)
                {
                    Charge();
                }
            }
            else
                Charge();
        }
        else
        {
            if (chargeFirstTime)
            {
                timer = chargeCooldown;
            }
            else
            {
                timer += 1 * Time.deltaTime;
            }

            Charge1 = false;
            if (enemy.Player!=null)
            previousPlayerPos = enemy.Player.transform.position;
        }
    }

    void Charge()
    {
        Charge1 = true;
        chargeFirstTime = false;
        seek.move = false;

        Vector3 deltaVector = (previousPlayerPos - transform.position).normalized;
        deltaVector.y = 0;

        direction = deltaVector;

        transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * rotSpeed);

        transform.position += transform.forward * chargeSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, previousPlayerPos) <= 1f || collisionWithPlayer)
        {
            timer = 0f;
            //Debug.Log("Charge del bull");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" && Charge1)
        {
            collisionWithPlayer = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, chargeRange);
    }
}
