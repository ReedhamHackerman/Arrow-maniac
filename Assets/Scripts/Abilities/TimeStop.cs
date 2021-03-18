using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : Abilities
{
    public GameObject mapParticles;
    private List<ParticleSystem> worldParticles = new List<ParticleSystem>();
    private ParticleSystem particle;


    protected override void Initialize()
    {
        //worldParticles.AddRange(mapParticles.GetComponentsInChildren<ParticleSystem>());

        worldParticles.AddRange(FindObjectsOfType<ParticleSystem>());
    }

    protected override void Refresh()
    {

        
        if (Input.GetKey(KeyCode.R))
        {
            StopWorldParticles();
        }
        if (Input.GetKey(KeyCode.T))
        {
            ResumeWorldParticles();
        }


    }

    public void StopWorldParticles()
    {
        foreach (ParticleSystem particleSystem in worldParticles)
        {
            particleSystem.Pause();
        }
    }

    public void ResumeWorldParticles()
    {
        foreach (ParticleSystem particleSystem in worldParticles)
        {
            particleSystem.Play();
        }
    }

    
}
