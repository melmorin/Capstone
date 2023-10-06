using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour {

	[Header("Level Info")] 
	[SerializeField] private string levelName;
	[SerializeField] private int levelNumber;

	[Header("Node Destinations")]
	[SerializeField] private GameObject upDestination;
	[SerializeField] private GameObject downDestination;
	[SerializeField] private GameObject leftDestination;
	[SerializeField] private GameObject rightDestination;

	private GameObject player; 
	private GameManager gameManager; 
	private MapController mapController;  
	private bool currentNode;
	private float dirX; 
	private float dirY; 
	private float speed = 5f; 

	// Use this for initialization
	void Start() 
	{
		gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>();
		mapController = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<MapController>();
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update() 
	{
		if (transform.position == player.transform.position) 
		{
			currentNode = true;
			if (!mapController.facingRight) mapController.Flip();
		} 
		
		else 
		{
			currentNode = false;
		}

		if (currentNode) 
		{
			// Activates menu if level if available 
			if (levelName == "")
			{
				mapController.SetMenuActive(false);
			}
			else 
			{
				mapController.SetMenuActive(true, levelName, levelNumber);
			}

			// Checks to see if any destination is null 
			if (!gameManager.IsGamePaused())
			{

				dirX = Input.GetAxisRaw("Horizontal");
				dirY = Input.GetAxisRaw("Vertical");

				if (dirY > 0) 
				{
					if (upDestination != null) 
					{
						currentNode = false;
						mapController.SetMenuActive(false);
						StartCoroutine(DoUp()); 
					}
				} 

				else if (dirY < 0) 
				{
					if (downDestination != null) 
					{
						currentNode = false;
						mapController.SetMenuActive(false);
						StartCoroutine(DoDown()); 
					}
				} 

				else if (dirX < 0) 
				{
					if (leftDestination != null) 
					{
						currentNode = false;
						if (mapController.facingRight) mapController.Flip();
						mapController.SetMenuActive(false);
						StartCoroutine(DoLeft()); 
					}
				} 

				else if (dirX > 0) 
				{
					if (rightDestination != null) 
					{
						currentNode = false;
						if (!mapController.facingRight) mapController.Flip();
						mapController.SetMenuActive(false);
						StartCoroutine(DoRight()); 
					}
				}								
			}
		}	
	}

	// Moves the player to the up destination 
	IEnumerator DoUp()
	{
		yield return new WaitForSeconds(1/60);
		while (player.transform.position != upDestination.transform.position) 
		{
			player.transform.position = Vector3.MoveTowards(player.transform.position, upDestination.transform.position, speed * Time.deltaTime);
			yield return null;
		}
	}

	// Moves the player to the down destination 
	IEnumerator DoDown()
	{
		yield return new WaitForSeconds(1/60);
		while (player.transform.position != downDestination.transform.position) 
		{
			player.transform.position = Vector3.MoveTowards(player.transform.position, downDestination.transform.position, speed * Time.deltaTime);
			yield return null;
		}
	}

	// Moves the player to the left destination 
	IEnumerator DoLeft()
	{
		yield return new WaitForSeconds(1/60);
		while (player.transform.position != leftDestination.transform.position) 
		{
			player.transform.position = Vector3.MoveTowards(player.transform.position, leftDestination.transform.position, speed * Time.deltaTime);
			yield return null;
		}
	}

	// Moves the player to the right destination 
	IEnumerator DoRight()
	{	
		yield return new WaitForSeconds(1/60);
		while (player.transform.position != rightDestination.transform.position) 
		{
			player.transform.position = Vector3.MoveTowards(player.transform.position, rightDestination.transform.position, speed * Time.deltaTime);
			yield return null;
		}		
	}
}

