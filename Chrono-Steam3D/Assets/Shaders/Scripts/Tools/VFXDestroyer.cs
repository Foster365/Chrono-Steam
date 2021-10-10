using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXDestroyer : MonoBehaviour
{
    private ParticleSystem Selfparticle;
    void Start()
    {
        Selfparticle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, 1);
    }
}
