using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjectsBetween : MonoBehaviour
{

    Material objectMat;
    [SerializeField] Transform target;
    [SerializeField] LayerMask wallLayerMask;
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
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(2,2,2), Quaternion.identity, wallLayerMask);
        //if (Physics.Raycast(transform.position, transform.forward, dist, 1 >> LayerMask.NameToLayer("wall")))
        //if(colliders.Length > 0)
        //{

            Debug.Log("Collision with wall");
            foreach(var w in colliders)
            {
            MeshRenderer r = w.GetComponent<MeshRenderer>();
            Color newColor = r.material.color;
            newColor.a = 0;
            foreach (var item in r.materials)
            {

                item.
                    color = newColor;
            }

                //Debug.Log("Wall Alpha" + w.GetComponent<MeshRenderer>().material.color.a);
            }
            //if (wallLayerMask.layer.ToString() == LayerMask.LayerToName(9))
            //{

            //    float a = wallLayerMask.GetComponent<Color>().a;
            //    float alphaColorPropierty = a;
            //    alphaColorPropierty = 0;

            //}

        //}

    }
}
