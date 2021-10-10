using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{

    //public static VFXManager instance;

    [SerializeField]
    VFX[] vfxPrefabs;

    private void Awake()
    {
        foreach (VFX ps in vfxPrefabs)
        {
            //ps.Source = gameObject.GetComponent<ParticleSystem>();
            ps.Source = ps.ParticleSyst;
            ps.Source.name = ps.VFXName;
        }

        //if (instance == null) instance = this;
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(gameObject);

        //foreach (ParticleSystem s in sounds)
        //{
        //s.Source = gameObject.AddComponent<AudioSource>();
        //s.Source.clip = s.Clip;

        //s.Source.volume = s.Volume;
        //s.Source.pitch = s.Pitch;
        //s.Source.loop = s.Loop;
        ////s.Source.dopplerLevel = s.DopplerLevel;
        //s.Source.spatialBlend = s.SpatialBlend;

        //}
    }

    //public void PlayVFXArray()
    //{
    //    foreach (var vfx in vfxPrefabs)
    //    {
    //        PlayVFX(vfx.GetComponent<Transform>());
    //    }
    //}

    public void PlayVFXTHIS(string vfxPrefabName)
    {
        VFX vfx = Array.Find(vfxPrefabs, vfxPartSystem => vfxPartSystem.VFXName == vfxPrefabName);
        if (vfx == null)
        {
            return;
        }
        vfx.Source.Play();

        //if (vfxPref.GetComponent<ParticleSystem>() != null)
        //{
        //    Debug.Log("Particle System Anim Event not null");
        //    vfxPref.Play();
        //}
        //else
        //{

        //    Debug.Log("Particle System Anim Event null");

        //}
        
    }

    //public List<ParticleSystem> GetParticleSystems(Transform prefab, List<ParticleSystem> particles = null)
    //{
    //    //if (particles == null) particles = new List<ParticleSystem>();

    //    foreach (Transform ps in prefab)
    //    {

    //        if (ps.GetComponent<ParticleSystem>() != null) particles.Add(ps.GetComponent<ParticleSystem>());
    //        else Debug.Log("ps is null");

    //    }

    //    return particles;
    //}

    //public void Play(string name, ParticleSystem[] particleSystems)
    //{
        //ParticleSystem s = particleSystems.Find(particleSystems, s => s.name == name);
        //if (s == null)
        //{
        //    return;
        //}
        //s.Source.Play();
    //}

}
