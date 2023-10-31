using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corner : MonoBehaviour {

	[Header("Corner Destination")]
	[SerializeField] private GameObject destination;
	[SerializeField] private string direction; 

	[Header("Corner Destination Reverse")]
	[SerializeField] private GameObject destinationReverse;
	[SerializeField] private string directionReverse; 

	private GameObject player;
	private MapController mapController;
	private bool flip = false;
	private float speed = 3f;  

	// Start is called before the first frame 
	void Start()
	{
		mapController = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<MapController>();
		player = GameObject.FindGameObjectWithTag("Player");
	}

	// Update is called once per frame
	void Update () 
	{
		if (transform.position == player.transform.position) 
		{
			StartCoroutine (DoFollow ());
		}
	}

	// Function that moves the player towards a destination 
	IEnumerator DoFollow()
	{
		yield return new WaitForSeconds (1/60);
		if (!flip)
		{
			if (mapController.facingLeft && direction == "Right") mapController.Flip();
			if (!mapController.facingLeft && direction == "Left") mapController.Flip();

			mapController.Animate(direction); 
			while (player.transform.position != destination.transform.position)
			{
				player.transform.position = Vector3.MoveTowards (player.transform.position, destination.transform.position, speed * Time.deltaTime);
				yield return null;
			}
			mapController.Animate("Stop"); 
			flip = true; 			
		}
		else
		{
			if (mapController.facingLeft && directionReverse == "Right") mapController.Flip();
			if (!mapController.facingLeft && directionReverse == "Left") mapController.Flip();

			mapController.Animate(directionReverse); 
			while (player.transform.position != destinationReverse.transform.position)
			{
				player.transform.position = Vector3.MoveTowards (player.transform.position, destinationReverse.transform.position, speed * Time.deltaTime);
				yield return null;
			}
			mapController.Animate("Stop"); 
			flip = false; 
		}
	}
}
