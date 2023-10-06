using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
   public Sprite lootSprite; 
   public string lootType; 
   public int dropChance; 

    // Constructor for the Loot scriptable object 
    public Loot(string lootType, int dropChance)
    {
        this.lootType = lootType;
        this.dropChance = dropChance; 
    }
}
