using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTorretBullet : MonoBehaviour
{
    public float bulletDamage = 20;
    private void Start()
    {
        Physics.IgnoreLayerCollision(10, 12);
    }
    void Update()
    {
        transform.Translate(0.02f, 0, 0);
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player_Controller>().Life_Controller.GetDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
}
