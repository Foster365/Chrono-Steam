using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyAnimations))]
public class Enemy : Actor
{
    private EnemyAI AI;
    private Life_Controller _life_Controller;
    private EnemyAnimations animations;
    [SerializeField] bool dead;
    Player_Controller _player;
    private bool hurt;
    float timer;
    private bool _itemDroped;
    private Roulette roulette;
    private Dictionary<GameObject, int> dropNodes = new Dictionary<GameObject, int>();
    [SerializeField] List<GameObject> drops;
    [SerializeField] List<int> rates;

    [SerializeField]
    protected List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    [SerializeField]
    private List<GameObject> bodyParts = new List<GameObject>();

    public bool Dead { get => dead; set => dead = value; }
    public bool Hurt { get => hurt; set => hurt = value; }
    public Player_Controller Player { get => _player; set => _player = value; }
    public Life_Controller Life_Controller  => _life_Controller;
    public EnemyAnimations Animations => animations;
    protected virtual void Awake()
    {
        //_life_Controller = new Life_Controller(Stats.MaxHealth);
        _life_Controller = new Life_Controller(Stats.LifeRange[Random.Range(0, Stats.LifeRange.Count)]);
        animations = GetComponent<EnemyAnimations>();

        roulette = new Roulette();
        _itemDroped = false;
    }
    protected virtual void Start()
    {
        //animator = GetComponent<Animator>();
        _life_Controller.isDead = false;
        AI = GetComponent<EnemyAI>();
        _life_Controller.Dead.AddListener(Die);
        _life_Controller.Damaged.AddListener(OnDamaged);
        for (int i = 0; i < drops.Count; i++)
        {
            dropNodes.Add(drops[i], rates[i]);
        }
        _player = GameManager.Instance.PlayerInstance.GetComponent<Player_Controller>();
    }

    protected virtual void Update()
    {
        if (Hurt)
        {
            timer += Time.deltaTime;
            if (timer >= 2f)
            {
                Hurt = false;
                timer = 0f;
            }
        }
        //Debug.Log("enemy life" + Life_Controller.CurrentLife);
    }

    void OnDamaged()
    {
        if (!Life_Controller.isDead)
        {
            Hurt = true;
            if (gameObject.TryGetComponent<BossAI>(out var bossAI))
            {
                bossAI.Animations.DamagedAnimation();
            }
            else
            {
                animations.DamagedAnimation();
            }

            for (int i = 0; i < particleSystems.Count; i++)
            {
                particleSystems[i].Play();
            }
        }

    }
    public void PlayParticle(GameObject particle)
    {
        if (particle != null)
        {
            Instantiate(particle, transform.position, transform.rotation);
            particle.GetComponent<ParticleSystem>().Play();
        }
        else
            Debug.Log("playParticles function particle not asigned");
    }

    void Die()
    {
        if (gameObject.TryGetComponent<BossAI>(out var bossAI))
        {
            foreach (var itemA in bodyParts)
            {
                //Debug.Log("recorriendo parte del cuerpo");
                //...Busco los materiales del skin mesh renderer
                for (int i = 0; i < itemA.GetComponent<SkinnedMeshRenderer>()?.materials.Length; i++)
                {
                    //  Debug.Log("recorriendo materiales");
                    var itemB = itemA.GetComponent<SkinnedMeshRenderer>().materials[i];
                    // si el material es un enemy fresnel...
                    if (itemB.name != "Monster")
                    {
                        //Debug.Log("material correcto");
                        //...seteo fesnel scale para q ya no brile
                        itemA.GetComponent<SkinnedMeshRenderer>().materials[i].SetFloat("_FresnelScale", 0);
                    }
                }
            }
            bossAI.Animations.DeathAnimation();

            //busco el evento de BossDying
            var Event = GameManager.Instance.LvlManager.GetComponent<LevelManager>().BossDying;
            //encolo el evento para su invoke
            GameManager.Instance.EventQueue.Add(Event); 
        }
        else
        {
            animations.DeathAnimation();
            //encada una de mis partes del cuerpo...
            foreach (var itemA in bodyParts)
            {
                //Debug.Log("recorriendo parte del cuerpo");
                //...Busco los materiales del skin mesh renderer
                for (int i = 0; i < itemA.GetComponent<SkinnedMeshRenderer>().materials.Length; i++)
                {
                  //  Debug.Log("recorriendo materiales");
                    var itemB = itemA.GetComponent<SkinnedMeshRenderer>().materials[i];
                    // si el material es un enemy fresnel...
                    if (itemB.name != "SkeletonOutlaw00")
                    {
                        //Debug.Log("material correcto");
                        //...seteo fesnel scale para q ya no brile
                       itemA.GetComponent<SkinnedMeshRenderer>().materials[i].SetFloat("_FresnelScale",0) ;
                    }
                }
            }
            if (!_itemDroped)
            {
                ExecuteRoulette();
                _itemDroped = true;
            }
        }
        //Debug.Log("Enemey died!");
        Dead = true;
        GetComponent<CapsuleCollider>().enabled = false;
        Destroy(gameObject, 1.5f);

    }
    void ExecuteRoulette()
    {
        GameObject item = roulette.Run(dropNodes);
        if (item!=null)
        {
            Instantiate(item, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.identity);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Weapon"))
        {
            // Debug.Log("ouch"+ Life_Controller.CurrentLife);
            if (gameObject.TryGetComponent<BossAI>(out var bossAI))
            {
                bossAI.Animations.DamagedAnimation();
                Life_Controller.GetDamage(other.gameObject.GetComponent<Weapon>().WeaponStats.AttDamage);
            }
            else
            {
                animations.DamagedAnimation();
                Life_Controller.GetDamage(other.gameObject.GetComponent<Weapon>().WeaponStats.AttDamage);
            }
        }
    }
}
