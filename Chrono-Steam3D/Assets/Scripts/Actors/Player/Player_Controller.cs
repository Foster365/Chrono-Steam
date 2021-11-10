using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Player_Input))]
[RequireComponent(typeof(PlayerAnimations))]
public class Player_Controller : MonoBehaviour,ILive
{
    private Rigidbody _rb;
    private Player_Input _inputs;
    private Life_Controller _life_Controller;
    private PlayerAnimations _animations;
    private PlayerActions _actions;
    private HitCounter hitCounter;
    private float _currentDashCoolDown;
    private float _currentDashDuration;
    private float _currentPunchCD;
    private float _currentPunchDuration;
    private bool _isMoving;
    private bool _isDashing;
    private bool _isSpecial;
    private bool _isAttacking;
    private bool _isPunching;
    private bool _isleaving;
    bool isWeaponSlotNull;
    [SerializeField] private bool stunned = false;
    float timer = 0;

    [SerializeField]
    private GameObject trailEffect;
    [SerializeField]
    private PlayerStats _playerStats;
    [SerializeField]
    private Transform weaponSlot;

    [SerializeField]
    GameObject stunParticles;

    [SerializeField] UIIconsManager weaponsUIICons;

    public Rigidbody Rb => _rb;
    public float MaxLife => _playerStats.MaxLife;
    public Life_Controller Life_Controller => _life_Controller;
    public PlayerStats PlayerStats  => _playerStats;
    public Player_Input Inputs => _inputs;
    public PlayerAnimations Animations => _animations;
    public bool IsDashing => _isDashing;
    public float CurrentDashCoolDown => _currentDashCoolDown;
    public float CurrentPunchDuration => _currentPunchDuration;

    public bool Stunned { get => stunned; set => stunned = value; }
    public bool Isleaving { get => _isleaving; set => _isleaving = value; }
    public bool IsAttacking { get => _isAttacking; set => _isAttacking = value; }
    public bool IsWeaponSlotNull { get => isWeaponSlotNull; set => isWeaponSlotNull = true; }
    public bool IsSpecial { get => _isSpecial; set => _isSpecial = value; }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _rb = GetComponent<Rigidbody>();
        _inputs = GetComponent<Player_Input>();
        _animations = GetComponent<PlayerAnimations>();
        _actions = GetComponent<PlayerActions>();
        _life_Controller = new Life_Controller(_playerStats.MaxLife);
        _life_Controller.Dead.AddListener(OnDead);
        _life_Controller.Damaged.AddListener(Damaged);
        _playerStats.Weapon = null; _currentPunchCD = 0;
        _currentPunchDuration = PlayerStats.AbilitiStats.PunchDuration;
        _currentDashCoolDown = 0;
        _currentDashDuration = _playerStats.DashDuration;
        //weaponsUIICons = GetComponentInChildren<UIIconsManager>();
    }
    // Start is called before the first frame update
    void Start() {
       
        // _playerStats.SpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnpoint").transform;
       
        hitCounter = GameObject.FindGameObjectWithTag("hitCounter").GetComponent<HitCounter>();
        _life_Controller.Dead.AddListener(GameManager.Instance.GameOver);
        GameManager.Instance.PlayerInstance = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        _animations.MovingAnimation(_isMoving);
        _animations.Moving_X_YAnimation(_rb.velocity.x,_rb.velocity.z);
        if (!Life_Controller.isDead)
        {
            Regenerate();
        }

        if (_currentDashCoolDown>0)
        {
            _currentDashCoolDown -= Time.deltaTime;
            _isDashing = false;
        }
        if (_currentPunchCD>0)
        {
            _currentPunchCD -= Time.deltaTime;
            _isPunching = false;
        }
        Actions();
        //Debug.Log(Life_Controller.CurrentLife);
        if (_playerStats.Weapon != null)
        {
            Physics.IgnoreLayerCollision(8, 11);
            _playerStats.Weapon.GetComponent<Weapon>().CoolDown();
        }
        else
        { Physics.IgnoreLayerCollision(8, 11, false); }
        //Debug.Log("Player life" + Life_Controller.CurrentLife);
        //Debug.Log(_comboCounter);

        //Debug.Log($"IsDrop: {_playerStats.Weapon.GetComponent<Weapon>().IsDrop}");

        if (stunned)
        {
            stunParticles.SetActive(true);
            _animations.StunnedAnimation(true);
        }
        else
        {
            stunParticles.SetActive(false);
            _animations.StunnedAnimation(false);
        }

    }

    private void FixedUpdate()
    {
        if(!_isDashing && !_isAttacking && !_isleaving && !_isSpecial && !_isPunching)
        {
            Movement();
            //Debug.Log(_currentDashCoolDown);
            //Debug.Log(_currentPunchCD);
        }
       
        if (_isDashing)
        {            
            if (_currentDashDuration <= 0)
            {
                _isDashing = false;
                //Debug.Log("dashed");
                _currentDashCoolDown = _playerStats.DashCoolDown;
                _currentDashDuration = _playerStats.DashDuration;
                trailEffect.SetActive(false);
            }
            else
            {
                if (!Life_Controller.isDead && !stunned)
                {
                    _currentDashDuration -= Time.fixedDeltaTime;
                    var velocity = _playerStats.DashDistance / _playerStats.DashDuration;
                    float prevVelocityY = _rb.velocity.y;
                    _rb.velocity = new Vector3(Mathf.Round(_inputs.xMovement()) * velocity, prevVelocityY/4,
                                                Mathf.Round(_inputs.yMovement()) * velocity);
                }
            }
        }
        
        if (_isPunching)
        {
            if (_currentPunchCD <= 0)
            {
                if (_currentPunchDuration <= 0)
                {
                    _isPunching = false;
                    // Debug.Log("punched");
                    _currentPunchCD = PlayerStats.AbilitiStats.PunchCoolDown;
                    _currentPunchDuration = PlayerStats.AbilitiStats.PunchDuration;
                }
                else
                {
                    _isPunching = true;
                    _animations.PunchAnimation();
                    _currentPunchDuration -= Time.fixedDeltaTime;
                    var velocity = PlayerStats.AbilitiStats.PunchDistance / PlayerStats.AbilitiStats.PunchDuration;
                    float prevVelocityY = _rb.velocity.y;
                    Vector3 v = transform.forward * velocity;
                    v.y = prevVelocityY;
                    _rb.velocity = v;
                }
            }
            
        }
        if (_isSpecial)
        {
            //GetComponent<LookAtMouse>().enabled = false;
        }

        if (_isAttacking)
        {
            _rb.velocity = Vector3.zero;
            _isMoving = false;
        }

        if (_isleaving)
        {
            _isMoving = false;
        }
    }
    void Movement()
    {
        if ((_inputs.xMovement() != 0) && (!Life_Controller.isDead) && (!Stunned) || (_inputs.yMovement() != 0) && (!Life_Controller.isDead) && (!Stunned)) {
            float prevVelocityY = _rb.velocity.y;

            _rb.velocity = new Vector3(_inputs.xMovement() * _playerStats.Speed * Time.deltaTime, prevVelocityY,
                                       _inputs.yMovement() * _playerStats.Speed * Time.deltaTime);
            //_animations.RunningAnim();
            _isMoving = true;

            //FindObjectOfType<AudioManager>().Play("PlayerRunning");
            return;
        }
        else
        {/* _rb.velocity = Vector2.zero;*/ _isMoving = false; return; }
    }
    void Actions()
    {
        if(!stunned && !_life_Controller.isDead)
        _actions.Execute();
    }
    public void Dash()
    {
        _rb.velocity = Vector3.zero;
        _isDashing = true;
        trailEffect.SetActive(true);
    }
    public void ThrustingPunch()
    {
        _rb.velocity = Vector3.zero;
        _isPunching = true;
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("FloorWeapon"))
        {
            isWeaponSlotNull = false;
            //Debug.Log("hit Weapon");
            if(_playerStats.Weapon == null)
            {
                SetWeapon(other.gameObject.transform.GetChild(0).gameObject);
                EnableWeaponIcon(_playerStats.Weapon.tag);
                //_playerStats.Weapon.GetComponent<Weapon>().IsDrop = true;
                FindObjectOfType<AudioManager>().Play("PickUpWeapon");
                Destroy(other.gameObject);
            }
        }
    }
    private void Damaged()
    {
        if (Life_Controller.CurrentLife > 0)
        {
            _animations.DamagedAnimation();
            FindObjectOfType<AudioManager>().Play("PlayerHit");
            //hitCounter.HideHitCounter();
        }
    }
    void SetWeapon(GameObject @object)
    {
        //@object.GetComponent<Rigidbody>().isKinematic = true;
        @object.transform.position = weaponSlot.position;
        @object.transform.rotation = weaponSlot.rotation;
        @object.transform.parent = weaponSlot;
        _playerStats.Weapon = @object;
        //@object.GetComponent<Collider>().enabled = false;
    }

    public void WeaponExecute()
    {
        if(_playerStats.Weapon!=null)
            _playerStats.Weapon.GetComponent<Weapon>().Execute();
    }

    public void WeaponSpecialExecute()
    {
        //hacer que se aplique a todas las armas
        if (_playerStats.Weapon != null)
            _playerStats.Weapon.GetComponent<Weapon>().EspecialExecute();
    }
    // funcion para actiar particulas desde una animacion
    public void playParticle(GameObject particle)
    {
        if (particle != null)
        {
            Instantiate(particle, transform.position, transform.rotation);
            //Instantiate(particle, weaponSlot.transform.position, weaponSlot.transform.rotation);
            particle.GetComponent<ParticleSystem>().Play();
        }
        else
            Debug.Log("playParticles function particle not asigned");
    }
    public void PlaySound(string sound)
    {
        if(sound!=null)
        {
            FindObjectOfType<AudioManager>().Play(sound);
        }
        else
            Debug.Log("playSound function sound name not asigned");
    }

    public void Regenerate()
    {
        timer += 1 * Time.deltaTime;
        if (Life_Controller.CurrentLife < PlayerStats.MaxLife && timer >= 5f)
        {
            timer = 0f;
            Life_Controller.CurrentLife += 5f;
            if (Life_Controller.CurrentLife > PlayerStats.MaxLife)
            {
                Life_Controller.CurrentLife = PlayerStats.MaxLife;
            }
        }
    }

    public void ResetCombo()
    {
        _actions.ResetCombo();
        _isAttacking = false;
        _isSpecial = false;
        GetComponent<LookAtMouse>().enabled = true;
    }
    private void OnDead() {

        _animations.DamagedAnimation();
        _animations.DeathAnimation();
        FindObjectOfType<AudioManager>().Play("PlayerDeath");
    }

    void EnableWeaponIcon(string iconName)
    {
        switch (iconName)
        {
            case "Blade":
                weaponsUIICons.EnableIcon("Blade_Weapon_Icon");
                break;
            case "Claymore":
                weaponsUIICons.EnableIcon("Claymore_Weapon_Icon");
                break;
            case "Fist":
                weaponsUIICons.EnableIcon("Fist_Weapon_Icon");
                break;
            case "Gun":
                weaponsUIICons.EnableIcon("Gun_Weapon_Icon");
                break;
            case "Spear":
                weaponsUIICons.EnableIcon("Spear_Weapon_Icon");
                break;
        }
    }
}
