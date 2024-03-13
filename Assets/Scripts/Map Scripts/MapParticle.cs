using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles; 

    // Plays feet particle
    public IEnumerator PlayParticle()
    {
        particles.Play(); 
        yield return new WaitForSeconds(.1f);
        particles.Stop(); 
    }
}
