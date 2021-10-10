using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXAnimEvents : MonoBehaviour
{

    public string sound;

    //Dictionary<string, object> sounds = new Dictionary<string, object>();

    public void PlaySound(string s)
    {

        FindObjectOfType<AudioManager>().Play(s);
    }

}
