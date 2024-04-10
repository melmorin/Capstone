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
	
	private Camera mainCamera; 
	private float speed = 5f; 
	private float camSpeed = 25f; 
	private MapController mapController;
	
	// Calls before first frame updates
	void Start()
	{
		mapController = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<MapController>();
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 
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
		StartCoroutine(MoveCamera());

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

	// Moves Camera to new pos
	private IEnumerator MoveCamera()
	{
		while (mainCamera.transform.position != newCameraPos.transform.position) 
		{
			mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, newCameraPos.transform.position, camSpeed * Time.deltaTime);
			yield return null;
		}
	}
}