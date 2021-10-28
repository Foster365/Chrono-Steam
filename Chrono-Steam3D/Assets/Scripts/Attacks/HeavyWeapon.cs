using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyWeapon : BladeWeapon
{
    
    public override void Execute()
    {
        base.Execute();
        #region debugcomprobation
        //for (int i = 0; i < EspParticleSystems.Count; i++)
        //{
        //    Debug.Log("Entered in SPS for");

        //    if (EspParticleSystems == null) Debug.Log("Special Particle System is null!");
        //    else Debug.Log("Special Particle System not null");

        //    EspParticleSystems[i].Play();
        //}
        #endregion
    }
    public override void EspecialExecute()
    {

        //Debug.Log("Entered in Heavy Weapon SE");
        if (_currentDurability > 0)
        {
            _currentDurability -= WeaponStats.DuravilitiDecres;

            if (_currentEspExeCd >= WeaponStats.EspExeCd)
            {
                for (int i = 0; i < espParticleSystems.Count; i++)
                {
                    #region debugcomprobation
                    //  Debug.Log("Entered in SPS for");

                    /*if (EspParticleSystems == null) Debug.Log("Special Particle System is null!");
                    else Debug.Log("Special Particle System not null");*/
                    #endregion
                    espParticleSystems[i].Play();
                }
                if (specialOjcVfx != null)
                    Instantiate(specialOjcVfx, _player.transform.position + _player.transform.forward * _espAreaStats.MaxDistance, Quaternion.identity);
                else
                    Debug.Log("objVFX = NULL");

                Collider[] Enemys = Physics.OverlapSphere(_player.transform.position + _player.transform.forward * _espAreaStats.MaxDistance, _espAreaStats.MaxAmplitude);
                for (int i = Enemys.Length - 1; i >= 0; i--)
                {
                    if (Enemys[i].gameObject != null)
                    {
                        if (Enemys[i].gameObject.CompareTag("Enemy"))
                        {
                            Enemys[i].gameObject.GetComponent<Enemy>().Life_Controller.GetDamage(WeaponStats.EspDamage);
                        }
                    }
                }

                _currentEspExeCd = 0;
            }
        }
    }
    public override void Update()
    {
        for (int i = 0; i < ParticleSystems.Count-1; i++)
        {
            if (i<=1)
            {
                ParticleSystems[i].Play();
            }
        }
        base.Update();
    }
    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            if (_player != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(_player.transform.position, _player.transform.position + _player.transform.forward * _areaStats.MaxDistance);
                Gizmos.DrawWireSphere(_player.transform.position, _areaStats.MaxAmplitude);
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(_player.transform.position + _player.transform.forward * _espAreaStats.MaxDistance, _espAreaStats.MaxAmplitude);

            }
        }

    }
}
