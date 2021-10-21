using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereDamageArea : MonoBehaviour
{
    private float animCd = 3;           // colocar lo que dure la animación
    private float animActualCd;
    protected float damageActualCd;
    protected int damage;
    [SerializeField]protected float distance;
    [SerializeField] private float areaRadius;
    [SerializeField] protected float duration = 10;        // duración del napalm
    [SerializeField] protected float damageCd = 2;         // intervalo del daño


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
        Destroy(gameObject, duration);

        if(damageActualCd >= damageCd )
        {
            Collider[] enemis = Area();
            foreach (var enemy in enemis)
            {
                if (enemy != null&& enemy.gameObject.CompareTag("Enemy"))
                    enemy.gameObject.GetComponent<Enemy>().Life_Controller.GetDamage(damage);
                damageActualCd = 0;
            }
        }
        else damageActualCd += Time.deltaTime;

        if (animActualCd < animCd)
        {
            //Debug.Log("Animación en curso");
        }
    }
    public virtual Collider[] Area()
    {
        var Area = Physics.OverlapSphere(transform.position, areaRadius);
        return Area;  
    }
    public virtual void Create(int damage, Vector3 position)
    {
        //Debug.Log("NapalmInstantie");
        this.damage = damage;
        var _player = GameManager.Instance.PlayerInstance;
        Instantiate(gameObject,position, Quaternion.identity);
    }
    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, areaRadius);
    }
}
