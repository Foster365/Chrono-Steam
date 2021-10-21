using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassCapsule : MonoBehaviour
{

    void OnCollisionEnter (Collision other)
    {

        if (other.gameObject.CompareTag("Player"))
            FindObjectOfType<AudioManager>().Play("PlayerGlassCapsule");

    }
}

