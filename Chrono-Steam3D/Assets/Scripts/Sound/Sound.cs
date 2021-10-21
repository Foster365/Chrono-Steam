using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    [SerializeField]
    string name;

    [Range(0f, 1f)]
    [SerializeField] float volume;

    [Range(.1f, 3f)]
    [SerializeField] float pitch;

    [SerializeField]
    bool loop;

    //[Range(0f, 1f)]
    //[SerializeField]
    //float dopplerLevel;

    [Range(0f, 1f)]
    [SerializeField]
    float spatialBlend;

    [SerializeField]
    AudioClip clip;

    [HideInInspector] AudioSource source;

    public string Name { get => name;}
    public float Volume { get => volume;}
    public float Pitch { get => pitch;}
    public bool Loop { get => loop;}
    public AudioClip Clip { get => clip;}
    public AudioSource Source { get => source; set => source = value; }
    //public float DopplerLevel { get => dopplerLevel; set => dopplerLevel = value; }
    public float SpatialBlend { get => spatialBlend; set => spatialBlend = value; }
}
