using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseParticles : MonoBehaviour
{
    [SerializeField] Texture2D cursorEnter; 

    // Runs before first frame 
    void Start()
    {
        Vector2 hotSpot = new Vector2(cursorEnter.width / 2f, cursorEnter.height / 2f); 
        Cursor.SetCursor(cursorEnter, hotSpot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        transform.position = ray.GetPoint(10) ;
    }
}
