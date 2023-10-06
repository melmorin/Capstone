using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove : MonoBehaviour
{

    [SerializeField] private int screenBuildIndex;

    // Moves the player to another scene based on an index 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") 
        { 
            SceneManager.LoadScene(screenBuildIndex, LoadSceneMode.Single);
        }
    }
}
