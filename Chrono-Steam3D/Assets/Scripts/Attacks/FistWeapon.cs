using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistWeapon : BladeWeapon
{
    public override void EspecialExecute()
    {
        if (currentDurability > 0)
        {
            currentDurability -= WeaponStats.DuravilitiDecres;
            if (specialOjcVfx != null)
            {
                if (specialOjcVfx.CompareTag("FloorCrack"))
                {
                    specialOjcVfx.GetComponent<BoxDamageArea>().Create(_weaponStats.EspDamage, Vector3.zero);
                }
                else
                    Debug.Log("objVFX tag error");
            }

            _currentEspExeCd = 0;
        }
    }
    public override void AreaAtack()
    {
        Collider[] Enemys = Physics.OverlapSphere(_player.transform.position + _player.transform.forward * _areaStats.MaxDistance, _areaStats.MaxAmplitude);
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
                        PlayParticles();
                    }
                    else
                        return;

                    Enemys[i].gameObject.GetComponent<Enemy>().Life_Controller.GetDamage(_weaponStats.AttDamage);

                }
            }
        }
    }

    void PlayParticles()
    {
        //for (int i = 0; i < particleSystems.Count; i++)
        //{

        //    particleSystems[i].Play();

        //}
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            if (_player != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(_player.transform.position, _player.transform.position + _player.transform.forward * _areaStats.MaxDistance);
                Gizmos.DrawWireSphere(_player.transform.position + _player.transform.forward * _areaStats.MaxDistance, _areaStats.MaxAmplitude);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_player.transform.position, _espAreaStats.MaxAmplitude);
                Gizmos.DrawWireSphere(_player.transform.position + _player.transform.forward * _espAreaStats.MaxDistance, _espAreaStats.MaxAmplitude);
            }
        }

    }
}
