using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour {

	[Header("Warp Destination")]
	public GameObject warpDestination;

	[Header("Destination After")]
	public GameObject destination;
	public string direction; 

	[Header("Dependencies")]
	public GameObject player;
	public GameObject newCameraPos; 
	
	private float speed = 5f; 
	private MapController mapController;
	private GameManager gameManager; 
	
	// Calls before first frame updates
	void Start()
	{
		gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>();
		mapController = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<MapController>();
	}

	// Update is called once per frame
	void Update () 
	{
		if (transform.position == player.transform.position && !mapController.hasWarped) 
		{
			mapController.hasWarped = true; 
			StartCoroutine(Teleport());
		}
	}

	// Teleports the player to their destination 
	private IEnumerator Teleport () 
	{
		StartCoroutine(gameManager.MoveCamera(newCameraPos, false));

		yield return new WaitForSeconds(.25f);
		player.transform.position = warpDestination.transform.position;
		
		if (direction == "Left") mapController.Animate("Left");
		else if (direction == "Right") mapController.Animate("Right");
		else if (direction == "Up") mapController.Animate("Up");
		else if (direction == "Down") mapController.Animate("Down");

		yield return new WaitForSeconds(1/60);

		while (player.transform.position != destination.transform.position) 
		{
			player.transform.position = Vector3.MoveTowards(player.transform.position, destination.transform.position, speed * Time.deltaTime);
			yield return null;
		}

		mapController.Animate("Stop");	
	}
}