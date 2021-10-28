using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Player_Controler))]
public class PlayerActions : MonoBehaviour,IComand
{
    private Player_Controler _player;
    private int _comboCounter;
    [Header("Ability UIs")]
    [SerializeField] private Image _gunUIarea; // la imagen q voy a mover
    [SerializeField] private Image _gunUIdistance;// la imagen q marca la distancia
    [SerializeField] private Transform _canvasCenter;
    float t;

    public Image GunUIarea { get => _gunUIarea;}

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Player_Controler>();
        _comboCounter = 0;
        t = 0;

    }
    public void Execute()
    {

        //float t = 0;
        t += Time.deltaTime;

        //Debug.Log($"t = {t}");

        #region normal click
        if (_player.Inputs.Action1())
        {
            if (_player.PlayerStats.Weapon != null)
            {
                _comboCounter += 1;
                _player.IsAttacking = true;
                Debug.Log(_comboCounter);
                if (_player.PlayerStats.Weapon.GetComponent<Weapon>().CurrentCD <= 0)
                {
                    if (_comboCounter!=0)
                    {
                        if (_comboCounter == 1)
                        {
                            if(_player.PlayerStats.Weapon.CompareTag("Blade"))
                            { 
                                _player.Animations.AttackAnimation();
                                _player.PlayerStats.Weapon.GetComponent<Weapon>().ParticleSystems[0].Play();
                            }
                            if (_player.PlayerStats.Weapon.CompareTag("Claimore"))
                            {
                                //_player.Animations.AttackAnimation();
                                _player.Animations.ClaymoreAttackAnimation();
                                _player.PlayerStats.Weapon.GetComponent<Weapon>().ParticleSystems[2].Play();
                            }
                            if (_player.PlayerStats.Weapon.CompareTag("Gun"))
                            {
                                _player.Animations.GunAttackAnimation();
                            }
                            if (_player.PlayerStats.Weapon.CompareTag("Fist"))
                            {
                                _player.Animations.FistAttackAnimation();
                                //_player.PlayerStats.Weapon.GetComponent<Weapon>().PlayParticleSystems();
                            }
                            if (_player.PlayerStats.Weapon.CompareTag("Spear"))
                            {
                                _player.Animations.SpearAttackAnimation();
                                //_player.PlayerStats.Weapon.GetComponent<Weapon>().ParticleSystems[0].Play();
                                _player.PlayerStats.Weapon.GetComponent<Weapon>().PlayParticleSystems();
                            }
                        }
                        if (_comboCounter == 2)
                        {
                            if (_player.PlayerStats.Weapon.CompareTag("Blade"))
                            {
                                _player.Animations.AttackAnimation2();
                                _player.PlayerStats.Weapon.GetComponent<Weapon>().ParticleSystems[0].Play();
                            }
                            if (_player.PlayerStats.Weapon.CompareTag("Claimore"))
                            {
                                //_player.Animations.AttackAnimation2();
                                 _player.Animations.ClaymoreAttackAnimation2();
                                _player.PlayerStats.Weapon.GetComponent<Weapon>().ParticleSystems[2].Play();

                            }
                            if (_player.PlayerStats.Weapon.CompareTag("Gun"))
                            {
                                _player.Animations.GunAttackAnimation();
                                // _player.PlayerStats.Weapon.GetComponent<Weapon>().ParticleSystems[0].Play();
                            }
                            if (_player.PlayerStats.Weapon.CompareTag("Fist"))
                            {
                                _player.Animations.FistAttackAnimation2();
                                //_player.PlayerStats.Weapon.GetComponent<Weapon>().ParticleSystems[0].Play();
                            }
                            if (_player.PlayerStats.Weapon.CompareTag("Spear"))
                            {
                                _player.Animations.SpearAttackAnimation2();
                                _player.PlayerStats.Weapon.GetComponent<Weapon>().ParticleSystems[0].Play();
                            }
                        }
                        if (_comboCounter == 3)
                        {
                            if (_player.PlayerStats.Weapon.CompareTag("Blade"))
                            {
                                _player.Animations.AttackAnimation3();
                                _player.PlayerStats.Weapon.GetComponent<Weapon>().ParticleSystems[0].Play();
                            }
                            if (_player.PlayerStats.Weapon.CompareTag("Claimore"))
                            {
                                //_player.Animations.AttackAnimation3();
                                _player.Animations.ClaymoreAttackAnimation3();
                                _player.PlayerStats.Weapon.GetComponent<Weapon>().ParticleSystems[2].Play();
                            }
                            if (_player.PlayerStats.Weapon.CompareTag("Gun"))
                            {
                                _player.Animations.GunAttackAnimation();
                            }
                            if (_player.PlayerStats.Weapon.CompareTag("Fist"))
                            {
                                _player.Animations.FistAttackAnimation3();
                                //_player.PlayerStats.Weapon.GetComponent<Weapon>().ParticleSystems[0].Play();
                            }
                            if (_player.PlayerStats.Weapon.CompareTag("Spear"))
                            {
                                _player.Animations.SpearAttackAnimation3();
                                _player.PlayerStats.Weapon.GetComponent<Weapon>().ParticleSystems[0].Play();
                            }
                        }
                        if (_comboCounter >= 3)
                        {
                            _comboCounter = 0;
                        }
                    }else
                        _player.PlayerStats.Weapon.GetComponent<Weapon>().ParticleSystems[0].Stop();
                    // Debug.Log("_playerStats.Weapon.Atack()");
                }
            }
        }
        #endregion
        #region prease R-click
        if (_player.Inputs.Action2())
        {
            if (_player.PlayerStats.Weapon != null)
            {
                if (_player.PlayerStats.Weapon.CompareTag("Blade"))
                {
                    //_player.WeaponSpecialExecute();
                }
                if (_player.PlayerStats.Weapon.CompareTag("Claimore"))
                {
                    //_player.WeaponSpecialExecute();
                }
                if (_player.PlayerStats.Weapon.CompareTag("Gun"))
                {
                    if (_gunUIarea != null && _gunUIdistance != null)
                    {
                        if (_player.PlayerStats.Weapon.GetComponent<Weapon>().CurrentEspExeCd >= _player.PlayerStats.Weapon.GetComponent<Weapon>().WeaponStats.EspExeCd)
                        {
                            _gunUIarea.gameObject.SetActive(true); // activo las UI
                            _gunUIdistance.gameObject.SetActive(true);
                        }
                    }
                    else
                        Debug.Log("GunUI NOT Asigned");
                }
                if (_player.PlayerStats.Weapon.CompareTag("Fist"))
                {

                }
                if (_player.PlayerStats.Weapon.CompareTag("Spear"))
                {

                }
                // _player.WeaponSpecialExecute();
            }
        }
        #endregion
        #region release R-click
        if (_player.Inputs.Action02())
        {
            if (_player.PlayerStats.Weapon != null)
            {
                if (_player.PlayerStats.Weapon.GetComponent<Weapon>().CurrentEspExeCd >= _player.PlayerStats.Weapon.GetComponent<Weapon>().WeaponStats.EspExeCd)
                {
                    _player.IsAttacking = true;
                    if (_player.PlayerStats.Weapon.CompareTag("Blade"))
                    {
                        _player.Animations.SpecialAttackAnimation();
                    }
                    if (_player.PlayerStats.Weapon.CompareTag("Claimore"))
                    {
                        _player.Animations.SpecialClaymoreAttackAnimation();
                    }
                    if (_player.PlayerStats.Weapon.CompareTag("Gun"))
                    {
                        if (_gunUIarea.gameObject.activeSelf && _gunUIdistance.gameObject.activeSelf)
                        {
                            _player.WeaponSpecialExecute();
                            _gunUIarea.transform.position = _canvasCenter.position;
                            _gunUIarea.gameObject.SetActive(false);
                            _gunUIdistance.gameObject.SetActive(false);
                        }
                    }
                    if (_player.PlayerStats.Weapon.CompareTag("Fist"))
                    {
                        _player.Animations.SpecialFistAttackAnimation();
                        FindObjectOfType<AudioManager>().Play("Gun_Weapon_Ultimate_A");
                        FindObjectOfType<AudioManager>().Play("Gun_Weapon_Ultimate_B");
                    }
                    if (_player.PlayerStats.Weapon.CompareTag("Spear"))
                    {
                        _player.Animations.SpecialSpearAttackAnimation();
                    }
                }
                
                // _player.WeaponSpecialExecute();
            }
        }
        #endregion
        if (_player.Inputs.Action3())
        {
            if (_player.PlayerStats.Weapon != null)
            {
                _player.IsWeaponSlotNull = true;
                FindObjectOfType<AudioManager>().Play("BrokenWeapon");
                _player.PlayerStats.Weapon.GetComponent<Weapon>().TurnOffWeaponFresnel();
                Debug.Log("Se desprendió la weapon");
                if (!_player.PlayerStats.Weapon.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                {
                    Rigidbody rb = _player.PlayerStats.Weapon.AddComponent<Rigidbody>();
                    _player.PlayerStats.Weapon.GetComponent<Weapon>().Rb = rb;
                }
                GameObject WeaponRef = _player.PlayerStats.Weapon;
                _player.PlayerStats.Weapon.GetComponent<Weapon>().Rb?.AddExplosionForce(500f, transform.position, 10f);
                _player.PlayerStats.Weapon.GetComponent<Weapon>().Rb?.AddTorque(_player.PlayerStats.Weapon.transform.right * 1000f);
                _player.PlayerStats.Weapon.transform.parent = null;
                _player.PlayerStats.Weapon = null;
                Destroy(WeaponRef, 2f);
            }
        }
        if (_player.Inputs.Action4() && !_player.IsDashing && _player.CurrentDashCoolDown <= 0)
        {
            _player.Dash();
        }
        if (_player.Inputs.Action5())
        {
            _player.Animations.PunchAnimation();
            _player.thrustingPunch();
        }
    }
    public void ResetCombo()
    {
        _comboCounter = 0;
    }
    //se llama desde el animator punch animation event
    public void PunchDetection()
    {
        Collider[] enemies = Physics.OverlapSphere(_player.transform.position,_player.PlayerStats.AbilitiStats.PunchArea);
        foreach (var enemy in enemies)
        {
            if (enemy != null && enemy.gameObject.CompareTag("Enemy"))
                enemy.gameObject.GetComponent<Enemy>().Life_Controller.GetDamage(_player.PlayerStats.AbilitiStats.PunchDamage);
        }
    }

    #region WeaponAbilities
    void ThrowSpear()
    {

        //Debug.Log($"t = {t}");

        //if (t >= spearThrowMaxTimer)
        //{

            _player.PlayerStats.Weapon.GetComponent<Transform>().position = _player.PlayerStats.Weapon.GetComponent<Transform>().position
                                                                            + new Vector3(_player.PlayerStats.Weapon.GetComponent<SpearWeapon>().AreaStats.MaxDistance,
                                                                            0, _player.PlayerStats.Weapon.GetComponent<SpearWeapon>().AreaStats.MaxDistance);

            Debug.Log($"Spear new pos at {_player.PlayerStats.Weapon.GetComponent<Transform>().position}");

            //t = 0;

        //}
    }

    void RetrieveSpear()
    {
        //_player.PlayerStats.Weapon.GetComponent<Transform>().position = _player.PlayerStats.Weapon.GetComponent<Transform>().position
        //                                                                - new Vector3(_player.PlayerStats.Weapon.GetComponent<SpearWeapon>().AreaStats.MaxDistance,
        //                                                                0, _player.PlayerStats.Weapon.GetComponent<SpearWeapon>().AreaStats.MaxDistance);


        _player.PlayerStats.Weapon.GetComponent<Transform>().position = _player.GetComponent<Transform>().position;

    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
       // Gizmos.DrawWireSphere(transform.position,_player.PlayerStats.AbilitiStats.PunchArea);
    }
}
