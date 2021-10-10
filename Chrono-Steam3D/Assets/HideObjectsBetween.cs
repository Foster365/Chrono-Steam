using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjectsBetween : MonoBehaviour
{

    Material objectMat;
    [SerializeField] Transform target;
    GameObject wallLayerMask;
    // Start is called before the first frame update
    void Start()
    {

        //target = FindObjectOfType<PlayerController>().transform;

    }

    // Update is called once per frame
    void Update()
    {
        LowerObjectMaterialAlpha();   
    }

    void LowerObjectMaterialAlpha()
    {

        float dist = Vector3.Distance(transform.position, target.position);

        if (Physics.Raycast(transform.position, transform.forward, dist, 1 >> LayerMask.NameToLayer("wall")))
        {

            if (wallLayerMask.layer.ToString() == LayerMask.LayerToName(9))
            {

                float a = wallLayerMask.GetComponent<Color>().a;
                float alphaColorPropierty = a;
                alphaColorPropierty = 0;

            }

        }

    }
}
