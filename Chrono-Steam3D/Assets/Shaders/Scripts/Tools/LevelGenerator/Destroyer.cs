using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField]
    private bool destroyParent;
    private void OnTriggerEnter(Collider other)
    {
        if (other!= null)
        {
            if (other.gameObject.CompareTag("SpawnPoint") || other.gameObject.CompareTag("CloseDoors"))
            {
                if (destroyParent)
                {
                    GameObject parent = other.gameObject.GetComponentInParent<Transform>().gameObject;
                    Destroy(other.gameObject);
                    Destroy(parent, 0.1f);
                }
                else
                    Destroy(other.gameObject);
            }
        }
    }
}
