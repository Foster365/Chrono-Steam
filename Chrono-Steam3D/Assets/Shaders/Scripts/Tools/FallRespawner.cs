using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRespawner : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.position = respawnPoint.position;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}
