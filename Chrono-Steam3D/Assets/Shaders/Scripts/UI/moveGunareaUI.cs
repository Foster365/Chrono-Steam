using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveGunareaUI : MonoBehaviour
{
    [SerializeField] private Transform CanvasCenter;
    [SerializeField] private Transform GunMaxdistance;
    [SerializeField] private float GunUIspeed;
    // Update is called once per frame
    void Update()
    {
        
        if (gameObject.activeSelf)
        {
            // calculo la distancia de el indicador de spawn al player
            var GunUIDistance = Vector3.Distance(transform.position.normalized, CanvasCenter.position.normalized);
            // calculo la distancia entre el player y la distancia maxima del special
            var GunUImaxDistance = Vector3.Distance(CanvasCenter.position.normalized, GunMaxdistance.position.normalized);
            //mientras la distancia del spawner sea menor a la distancia maxima
            if (GunUIDistance <= GunUImaxDistance)
            {
                // muevo en Y el spawner
                transform.Translate(0, GunUIspeed * Time.deltaTime, 0);
            }
        }

    }
}
