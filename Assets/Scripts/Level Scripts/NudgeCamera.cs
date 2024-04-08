using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NudgeCamera : MonoBehaviour
{
    private GameObject player; 
    private float nudgeDistance = 1f; 
    private GameManager gameManager; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameManager.gameOver) transform.position = GetTargetPosition(); 
    }

    // Finds the direction to nudge 
    Vector3 GetNudgeDirection () {

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector2 inputPos = Camera.main.ScreenToWorldPoint(mousePos);

        return (inputPos - (Vector2)player.transform.position).normalized;
    }

    // Finds the point where the camera should look 
    Vector3 GetTargetPosition () {
        return player.transform.position + (GetNudgeDirection() * nudgeDistance);
    }
}