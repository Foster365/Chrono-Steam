using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearWeapon : BladeWeapon
{
    public override void EspecialExecute()
    {
        
        if (_currentDurability > 0)
        {
            _currentDurability -= WeaponStats.DuravilitiDecres;
            for (int i = 0; i < espParticleSystems.Count; i++)
            {
                #region debugcomprobation
                // Debug.Log("Entered in SPS for");

                /*if (EspParticleSystems == null) Debug.Log("Special Particle System is null!");
                else Debug.Log("Special Particle System not null");*/
                #endregion
                espParticleSystems[i].Play();
            }

            Collider[] Enemys = Physics.OverlapCapsule(_player.transform.position, _player.transform.position + _player.transform.forward * _espAreaStats.MaxDistance, _espAreaStats.MaxAmplitude);
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
            _currentEspExeCd = 0;
        }
        
    }
}
