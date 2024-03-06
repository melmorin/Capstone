using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateParticle : MonoBehaviour
{
    [Header ("Dependancies")]
    [SerializeField] private GameObject particlePrefab; 

    // Instantiates the particle
    public void MakeParticle(Vector3 spawnPos, GameObject droppedItem)
    {
        GameObject particleGameObject = Instantiate(particlePrefab, spawnPos, Quaternion.identity);

        ParticleSystem particle = particleGameObject.GetComponent<ParticleSystem>();
        ParticleSystem pastParticle = droppedItem.GetComponent<ParticleSystem>();

        var ts = particle.textureSheetAnimation;
        var pastTs = pastParticle.textureSheetAnimation;

        ts.rowIndex = pastTs.rowIndex;
        var main = particle.main;

        particle.Play(); 
        StartCoroutine(DestoryObject(main.duration, particleGameObject));
    }

    // Destroys gameObject after duration is up 
    private IEnumerator DestoryObject(float duration, GameObject particles)
    {   
        yield return new WaitForSeconds(duration + 1);
        Destroy(particles); 
    }
}
