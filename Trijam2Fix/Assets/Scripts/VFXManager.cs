using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager instance;

    private static Transform particleParent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        particleParent = new GameObject().transform;
        particleParent.name = "ParticleParent";
    }

    public ParticleSystem hitVFX;
    public ParticleSystem missVFX;
    public ParticleSystem dropVFX;
    public ParticleSystem clickEmptyVFX;

    public static void SpawnParticleOneshot(ParticleSystem particle, Vector2 position)
    {
        if(particle != null)
        {
            ParticleSystem p = Instantiate(particle, position, Quaternion.identity, particleParent);
            var main = p.main;
            main.loop = false;
            main.stopAction = ParticleSystemStopAction.Destroy;
            p.Play();
        }


    }

    public static void DestroyAllParticles()
    {
        Destroy(particleParent.gameObject);
        particleParent = new GameObject().transform;
        particleParent.name = "ParticleParent";
    }
}
