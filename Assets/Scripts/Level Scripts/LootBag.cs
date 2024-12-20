using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditorInternal; 

public class LootBag : MonoBehaviour
{
    [Header ("Dependancies")]
    [SerializeField] private GameObject droppedItemPrefab; 
    [SerializeField] private List<Loot> lootList = new List<Loot>();

    // Picks a random piece of loot from the list of possibilities 
    private Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<Loot> possibleItems = new List<Loot>();
        foreach (Loot item in lootList)
        {
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }
        if (possibleItems.Count > 0)
        {
            Loot droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }
        return null; 
    }

    // Instantiates the dropped loot in the scene 
    public void InstantiateLoot(Vector3 spawnpos)
    {
        Loot droppedItem = GetDroppedItem();
        if (droppedItem != null)
        {
            GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnpos, Quaternion.identity);
            lootGameObject.gameObject.tag = droppedItem.lootType; 
            lootGameObject.GetComponent<Animator>().runtimeAnimatorController = droppedItem.anim; 

            ParticleSystem particle = lootGameObject.GetComponent<ParticleSystem>();
            var ts = particle.textureSheetAnimation;
            ts.rowIndex = droppedItem.particleIndex;

            particle.Play();
        }
    }
}
