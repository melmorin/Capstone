using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
    private Camera mainCam; 
    private Vector3 mousePos; 
    private GameManager gameManager; 
    private PlayerController playerController;
    [SerializeField] private Transform[] points;  
    public Transform currentPoint; 

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 
        gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameOver && playerController.isCharging)
        {
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            Vector3 rotation = mousePos - transform.position; 
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg; 
            
            currentPoint = FindPoint(rotZ, points);
            Debug.Log(currentPoint);
        }      
    }

    // Finds point with closest rotation 
    private Transform FindPoint(float rotZ, Transform[] points)
    { 
        Transform tMin = null; 
        float minDist = Mathf.Infinity; 

        Debug.Log(rotZ);  

        if (rotZ > 90 && rotZ <= 135)
        {
            rotZ -= 45; 
        }
        else if (rotZ > 135 && rotZ <= 180)
        {
            rotZ -= 90; 
        }
        else if (rotZ > 180 && rotZ <= 225)
        {
             rotZ += 90;
        }
        else if (rotZ > 225 && rotZ <= 270)
        {
             rotZ += 45;
        }

        if (rotZ < 0)
        {
            rotZ += 360; 
        }
        else if (rotZ >= 360)
        {
            rotZ -= 360;
        }

        Debug.Log(rotZ); 

        foreach (Transform t in points)
        {
            float tRot = t.transform.eulerAngles.z;

            if (tRot > 90 && tRot < 270)
            {
                tRot -= 180;
            }
            if (tRot < 0)
            {
                tRot += 360; 
            }

            if (rotZ > 270)
            {
                if (tRot == 0)
                {
                    tRot = 360; 
                }
            }

            float dist = Mathf.Abs(tRot - rotZ);

            Debug.Log(rotZ);
            Debug.Log(tRot);
            Debug.Log(dist);

            if (dist < minDist)
            {
                tMin = t; 
                minDist = dist; 
            }
        }
        return tMin; 
    }
}
