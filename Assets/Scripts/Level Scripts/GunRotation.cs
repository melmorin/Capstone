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
    public float rotZ; 

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
            mousePos = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

            Vector3 rotation = mousePos - transform.position; 
            rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg; 

            if (rotZ < 0)
            {
                rotZ += 360; 
            }
            
            currentPoint = FindPoint(rotZ, points);
        }      
    }

    // Finds point with closest rotation 
    private Transform FindPoint(float rotZ, Transform[] points)
    { 
        Transform tMin = null; 
        float minDist = Mathf.Infinity; 

        float upPoint = 0; 
        float midPoint = 0;
        float downPoint = 0; 

        foreach (Transform t in points)
        {
            float tRot = t.transform.eulerAngles.z;

            if (tRot > 300) downPoint = tRot; 
            else if (tRot > 15) upPoint = tRot;
            else midPoint = tRot; 
        }

        int count = 0; 
        int point = 0; 
        foreach (Transform t in points)
        {
            count++; 
            float tRot = t.transform.eulerAngles.z;

            if (!playerController.facingRight)
            {
                if (tRot == midPoint) tRot = 180; 
                if (tRot == upPoint) tRot = 135; 
                if (tRot == downPoint) tRot = 210;
            }

            if (rotZ > 270)
            {
                if (tRot == midPoint)
                {
                    tRot = 360; 
                }
            }

            else if (rotZ > 135 && rotZ < 225)
            {
                if (tRot == midPoint)
                {
                    tRot = 180; 
                }
            }

            float dist = Mathf.Abs(tRot - rotZ);

            if (dist < minDist)
            {
                point = count; 
                tMin = t; 
                minDist = dist; 
            }
        }
        playerController.rotZ = point; 
        return tMin; 
    }
}
