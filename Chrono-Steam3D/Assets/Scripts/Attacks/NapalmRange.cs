using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NapalmRange : MonoBehaviour
{
    private float damageActualCd;
    private float damageCd = 2;         // intervalo del daño
    private float napalmDuration = 10;  // duración del napalm
    private float animCd = 3;           // colocar lo que dure la animación
    private float animActualCd;
    private float distance;
    [SerializeField] private int damage;
    [SerializeField] private float area;


    // Tras instanciar, esperar a que "termine la animación" y luego hacer "X" daño a los enemigos cada "Y" tiempo durante "Z" tiempo
   /* private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && damageActualCd >= damageCd && animActualCd >= animCd)
        {
            other.GetComponent<Enemy>().Life_Controller.GetDamage(damage);
            damageActualCd = 0;
        }
        else damageActualCd += Time.deltaTime;
    }*/

    private void Update()
    {
        animActualCd += Time.deltaTime;
        Destroy(gameObject, napalmDuration + animCd);

        if(damageActualCd >= damageCd && animActualCd >= animCd)
        {
           Collider[] enemis = Physics.OverlapSphere(transform.position,area);
            foreach (var enemy in enemis)
            {
                if (enemy != null&& enemy.gameObject.CompareTag("Enemy"))
                    enemy.gameObject.GetComponent<Enemy>().Life_Controller.GetDamage(damage);
            }
        }
        else damageActualCd += Time.deltaTime;

        if (animActualCd < animCd)
        {
            //Debug.Log("Animación en curso");
        }
    }

    public void Create(int damage, float distance)
    {
        //Debug.Log("NapalmInstantie");
        this.damage = damage;
        this.distance = distance;
        var _player = GameManager.Instance.PlayerInstance;
        Instantiate(gameObject,_player.transform.position+_player.transform.forward*distance, Quaternion.identity);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, area);
    }
}
