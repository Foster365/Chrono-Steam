using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VFX
{
    [SerializeField]
    string vfxName;

    [SerializeField]
    ParticleSystem particleSyst;

    [HideInInspector]
    ParticleSystem source;

    public string VFXName { get => vfxName; set => vfxName = value; }
    public ParticleSystem ParticleSyst { get => particleSyst; set => particleSyst = value; }
    public ParticleSystem Source { get => source; set => source = value; }
}
