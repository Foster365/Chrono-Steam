using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class RangeWeapon : Weapon
{
    [SerializeField] private rangeWeaponStats _rangeStats;
    [SerializeField] private AreaStats _areaStats;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _bulletSpawner;
    //[SerializeField] private Transform _napalmSpawn;
    private float _currentReloadTime;
    private int bullets;
    public GameObject nRange;
    public rangeWeaponStats RangeStats { get => _rangeStats; }
    public GameObject BulletSpawner { get => _bulletSpawner; }
    public AreaStats AreaStats { get => _areaStats;}

    //public Transform NapalmSpawn { get => _napalmSpawn;}

    public override void Start()
    {
        base.Start();
        bullets = _rangeStats.maxBullets;
        _currentReloadTime = _rangeStats.reloadTime;
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    public override void Execute()
    {
       // Debug.Log("asdadasd");
            if (_currentDurability > 0)
            {
                _currentDurability -= _weaponStats.DuravilitiDecres;
                foreach (var item in ParticleSystems)
                {
                    item.Play();
                }
                //Instantiate(_rangeStats.bulletPrefab, BulletSpawner.transform.position, BulletSpawner.transform.rotation);
                Collider[] Enemys = Physics.OverlapCapsule(_player.transform.position, _player.transform.position + _player.transform.forward*_areaStats.MaxDistance,_areaStats.MaxAmplitude);
                for (int i = Enemys.Length - 1; i >= 0; i--)
                {
                    if (Enemys[i].gameObject != null)
                    {
                        if (Enemys[i].gameObject.CompareTag("Enemy"))
                        {
                            if (hitCounter != null && !Enemys[i].gameObject.GetComponent<Enemy>().Life_Controller.isDead)
                            {
                                hitCounter.AddHitCounter();
                                FindObjectOfType<AudioManager>().Play("PlayerSwordHit");
                            }

                            Enemys[i].gameObject.GetComponent<Enemy>().Life_Controller.GetDamage(_weaponStats.EspDamage);

                        }
                    }
                }
                // Debug.Log($"Quedan {bullets} en el cargador");
                //Debug.Log($"Hice {_weaponStats.AttDamage} de daño con {name}");
            }
        _currentCD = _weaponStats.CoolDown;
    }

    public override void EspecialExecute()
    {
        #region debug comprobation
        //Debug.Log("Entered in Heavy Weapon SE");

        //for (int i = 0; i < EspParticleSystems.Count; i++)
        //{
        //    Debug.Log("Entered in SPS for");

        //    if (EspParticleSystems == null) Debug.Log("Special Particle System is null!");
        //    else Debug.Log("Special Particle System not null");

        //    EspParticleSystems[i].Play();
        //}
        #endregion
        if (_currentDurability > 0)
        {
            _currentDurability -= WeaponStats.DuravilitiDecres;
            
            for (int i = 0; i < espParticleSystems.Count; i++)
            {
                espParticleSystems[i].Play();
            }

            nRange.GetComponent<SphereDamageArea>().Create(WeaponStats.EspDamage, _player.GetComponent<PlayerActions>().GunUIarea.transform.position);
            _player.GetComponent<Player_Controler>().IsAttacking = false;
            _currentEspExeCd = 0;
            
        }
    }

    public override void Update()
    {
        base.Update();
        if(bullets <= 0 || _currentReloadTime>0)
        {
            // Debug.Log("Recargando");
            _currentReloadTime -= Time.deltaTime;
            bullets = _rangeStats.maxBullets;
        }
    }
    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            if (_player != null)
            {
                Gizmos.DrawWireSphere(_player.transform.position, _areaStats.MaxAmplitude);
                Gizmos.DrawWireSphere(_player.transform.position + _player.transform.forward * _areaStats.MaxDistance, _areaStats.MaxAmplitude);
            }
        }
    }
}
