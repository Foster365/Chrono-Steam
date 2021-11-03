using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash_controler : MonoBehaviour
{
    [SerializeField]
    private float damage;
    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = GameManager.Instance.PlayerInstance.GetComponent<Player_Controller>();
            if (!player.IsDashing)
            {
                player.Life_Controller.GetDamage(damage);
            }
        }
    }
    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
