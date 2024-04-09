using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditorInternal; 

[CreateAssetMenu]
public class Loot : ScriptableObject
{
   public string lootType; 
   public int dropChance; 
   public RuntimeAnimatorController anim; 
   public int particleIndex;

    // Constructor for the Loot scriptable object 
    public Loot(string lootType, int dropChance, RuntimeAnimatorController anim, int particleIndex)
    {
        this.lootType = lootType;
        this.dropChance = dropChance; 
        this.anim = anim; 
        this.particleIndex = particleIndex; 
    }
}
