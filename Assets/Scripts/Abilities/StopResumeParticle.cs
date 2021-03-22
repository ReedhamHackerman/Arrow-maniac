using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopResumeParticle : MonoBehaviour, IFreezable
{

    private ParticleSystem particle;
    

    // Start is called before the first frame update
    void Start()
    {
        particle = gameObject.GetComponent<ParticleSystem>();
    }

    
    public void Freeze()
    {
        particle.Pause();
    }

    public void UnFreeze()
    {
        particle.Play();
    }
}
