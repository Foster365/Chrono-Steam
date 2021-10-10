using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossATCKControler : MonoBehaviour
{
    public GameObject teslaBall;
    [SerializeField]
    private GameObject smashObject;
    [SerializeField]
    private GameObject clapObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //prueba
        /*if (Input.GetKey(KeyCode.T))
        {
            TeslaAttack();
        }*/
    }
    public void SmashAttack()
    {
        Instantiate(smashObject, transform.position, Quaternion.Euler(-90,0,0));
    }
    public void ClapAttack()
    {
        Instantiate(clapObject, new Vector3(transform.position.x,transform.position.y+1,transform.position.z), transform.rotation);
    }
    public void TeslaAttack()
    {
        Instantiate(teslaBall, gameObject.transform.position, gameObject.transform.rotation);
    }
}