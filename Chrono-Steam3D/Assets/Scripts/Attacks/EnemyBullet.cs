using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletDamage = 20;
    private void Start()
    {
        Physics.IgnoreLayerCollision(10,12);
    }
    void Update()
    {
        transform.Translate(0, 0, 0.2f);
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player_Controler>().Life_Controller.GetDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
}
