using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal; 

[CreateAssetMenu]
public class Loot : ScriptableObject
{
   public string lootType; 
   public int dropChance; 
   public UnityEditor.Animations.AnimatorController anim; 

    // Constructor for the Loot scriptable object 
    public Loot(string lootType, int dropChance, UnityEditor.Animations.AnimatorController anim)
    {
        this.lootType = lootType;
        this.dropChance = dropChance; 
        this.anim = anim; 
    }
}
