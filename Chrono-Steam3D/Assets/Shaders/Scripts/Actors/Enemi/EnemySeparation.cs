using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeparation : MonoBehaviour
{
    GameObject[] enemies;
    [SerializeField] float spaceBetween = 1f;

    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void Update()
    {
        foreach(GameObject go in enemies)
        {
            if (go != null)
            {
                if (go != gameObject)
                {
                    float distance = Vector3.Distance(go.transform.position, this.transform.position);

                    if (distance <= spaceBetween)
                    {
                        Vector3 direction = transform.position - go.transform.position;
                        direction.y = 0;
                        transform.Translate(direction * Time.deltaTime);
                        //Debug.Log("Separando Enemigos");
                    }
                }
            }
        }
    }
}
