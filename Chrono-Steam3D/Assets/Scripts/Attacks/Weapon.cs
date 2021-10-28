using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class Weapon : MonoBehaviour, IComand
{
    [SerializeField]
    protected WeaponStats _weaponStats;
    [SerializeField]
    protected List<SkinnedMeshRenderer> weaponMaterials = new List<SkinnedMeshRenderer>();
    protected float _currentEspExeCd = 0;
    protected float _currentCD;
    protected int _currentDurability;
    public bool drawGizmos;
    private bool isDrop;
    private string weaponTag;
    private Rigidbody _rb;
    Animator _wpAnimator;
    protected HitCounter hitCounter;

    [SerializeField]
    protected List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    [SerializeField]
    protected List<ParticleSystem> espParticleSystems = new List<ParticleSystem>();

    //[SerializeField]
    //protected List<SkinnedMeshRenderer> weaponMaterials = new List<SkinnedMeshRenderer>();
    //[SerializeField]
    //protected SkinnedMeshRenderer weaponFresnelMaterial;

    public WeaponStats WeaponStats => _weaponStats;
    public float CurrentCD { get => _currentCD; }

    public List<ParticleSystem> ParticleSystems { get => particleSystems;}
    public Animator WpAnimator { get => _wpAnimator;}
    public float CurrentEspExeCd { get => _currentEspExeCd;}
    public string WeaponTag { get => weaponTag; }
    public bool IsDrop { get => isDrop; set => isDrop = true; }
    public Rigidbody Rb { get => _rb; set => _rb = value; }

    public virtual void Execute()
    {
        Debug.Log($"Hice {_weaponStats.AttDamage} de daño con {name} a rango melee de distancia");
    }

    public virtual void EspecialExecute()
    {
        // TODO Provisorio: Moví bloque de código de SpecialExecute aca
        
    }
    public virtual void CoolDown()
    {
        if (_currentCD > 0)
        {
            _currentCD -= Time.deltaTime;
        }
    }
    // Start is called before the first frame update

    public virtual void Start()
    {
        _currentCD = 0;
        _currentEspExeCd = WeaponStats.EspExeCd;
        _currentDurability = _weaponStats.Durability;
       /* _rb = this.gameObject.GetComponent<Rigidbody>();
        if (_rb = null)
             gameObject.AddComponent<Rigidbody>();*/
        Physics.IgnoreLayerCollision(11,10);
    }

    public void PlayParticleSystems()
    {
        foreach (var ps in particleSystems)
        {
            ps.Play();
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (_currentDurability <= 0 )
        {
            if (this.gameObject != null && !isDrop)
            {
                FindObjectOfType<AudioManager>().Play("BrokenWeapon");
                //Debug.Log("Se desprendió la weapon");
                TurnOffWeaponFresnel();
                if (!gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                {
                    Rigidbody rb = this.gameObject.AddComponent<Rigidbody>();
                    _rb = rb;
                }
                _rb?.AddExplosionForce(500f, transform.position, 6f);
                _rb?.AddTorque(transform.right * 1000f);

                transform.parent = null;
                GameManager.Instance.PlayerInstance.GetComponent<Player_Controler>().PlayerStats.Weapon = null;
                //GameObject weaponRef = GameManager.Instance.PlayerInstance.GetComponent<Player_Controler>().PlayerStats.Weapon;
                isDrop = true;
                Destroy(this.gameObject, 2f);
            }
        }
        else 
            isDrop = false;
        if (_currentEspExeCd < WeaponStats.EspExeCd)
        {
            _currentEspExeCd += Time.deltaTime;
        }
    }

    public void DestroyWeapon(SkinnedMeshRenderer m)
    {
        //foreach (var itemA in weaponMaterials)
        //{
        //    for (int i = 0; i < itemA.GetComponent<SkinnedMeshRenderer>().materials.Length; i++)
        //    {
        //        var itemB = itemA.GetComponent<SkinnedMeshRenderer>().materials[i];
        //        // si el material es un enemy fresnel...
        //        if (itemB.name != m.name)
        //        {
        //            //Debug.Log("material correcto");
        //            //...seteo fesnel scale para q ya no brile
        //            itemA.GetComponent<SkinnedMeshRenderer>().materials[i].SetFloat("_FresnelScale", 0);
        //        }
        //    }
        //}
    }
    public void TurnOffWeaponFresnel()
    {
        if (TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
        {
            for (int i = 0; i < GetComponent<MeshRenderer>().materials.Length; i++)
            {
                var b = GetComponent<MeshRenderer>().materials[i];

                //if (b.name == _weaponTag)
                b.SetFloat("_FresnelScale", 0);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
       // Debug.Log(collision.gameObject.name);
    }
}
