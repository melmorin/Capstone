using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour {

	[Header("Level Info")] 
	public string levelName;
	[SerializeField] private int levelNumber;
	[SerializeField] private int winsNeeded; 

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
	private float speed = 3f; 
	private SceneController sceneManager;

	// Use this for initialization
	void Start() 
	{
		gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>();
		mapController = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<MapController>();
		sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneController>(); 
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update() 
	{
		if (transform.position == player.transform.position) 
		{
			currentNode = true;
			mapController.hasWarped = false; 
			if (!mapController.facingLeft) mapController.Flip();
		} 
		
		else 
		{
			currentNode = false;
		}

		if (currentNode) 
		{
			// Activates menu of level if available 
			if (levelName == "" || sceneManager.isLoading)
			{
				mapController.SetMenuActive(false);
				mapController.SetLockedMenu(false); 
			}

			else 
			{	
				if (sceneManager.LevelsWon() < winsNeeded)
				{
					mapController.SetLockedMenu(true, winsNeeded); 
				}

				else 
				{
					int index = -1; 
					for (int i = 0; i < sceneManager.levelInfo.Count; i++)
					{ 
						if (sceneManager.levelInfo[i].levelName == levelName) index = i; 
					}

					if (index != -1)
					{
						mapController.SetMenuActive(true, levelName, levelNumber, sceneManager.levelInfo[index].hasWon, sceneManager.levelInfo[index].maxCoins, sceneManager.levelInfo[index].coinsFound, sceneManager.levelInfo[index].endItem);
					}
					else mapController.SetMenuActive(true, levelName, levelNumber); 
				}
			}

			// Checks to see if any destination is null 
			if (!gameManager.IsGamePaused() && !sceneManager.isLoading)
			{

				dirX = Input.GetAxisRaw("Horizontal");
				dirY = Input.GetAxisRaw("Vertical");

				if (dirY > 0) 
				{
					if (upDestination != null) 
					{
						currentNode = false;
						mapController.SetMenuActive(false);
						mapController.SetLockedMenu(false); 
						mapController.lastDirX = "Down"; 
						StartCoroutine(DoUp()); 
					}
				} 

				else if (dirY < 0) 
				{
					if (downDestination != null) 
					{
						currentNode = false;
						mapController.SetMenuActive(false);
						mapController.SetLockedMenu(false); 
						mapController.lastDirX = "Up"; 
						StartCoroutine(DoDown()); 
					}
				} 

				else if (dirX < 0) 
				{
					if (leftDestination != null) 
					{
						currentNode = false;
						if (!mapController.facingLeft) mapController.Flip();
						mapController.SetMenuActive(false);
						mapController.SetLockedMenu(false); 
						mapController.lastDirX = "Right"; 
						StartCoroutine(DoLeft()); 
					}
				} 

				else if (dirX > 0) 
				{
					if (rightDestination != null) 
					{
						currentNode = false;
						if (mapController.facingLeft) mapController.Flip();
						mapController.SetMenuActive(false);
						mapController.SetLockedMenu(false); 
						mapController.lastDirX = "Left"; 
						StartCoroutine(DoRight()); 
					}
				}								
			}
		}	
	}

	// Moves the player to the up destination 
	IEnumerator DoUp()
	{
		mapController.Animate("Up");
		yield return new WaitForSeconds(1/60);
		while (player.transform.position != upDestination.transform.position) 
		{
			player.transform.position = Vector3.MoveTowards(player.transform.position, upDestination.transform.position, speed * Time.deltaTime);
			yield return null;
		}
		mapController.Animate("Stop");
	}

	// Moves the player to the down destination 
	IEnumerator DoDown()
	{
		mapController.Animate("Down");
		yield return new WaitForSeconds(1/60);
		while (player.transform.position != downDestination.transform.position) 
		{
			player.transform.position = Vector3.MoveTowards(player.transform.position, downDestination.transform.position, speed * Time.deltaTime);
			yield return null;
		}
		mapController.Animate("Stop");
	}

	// Moves the player to the left destination 
	IEnumerator DoLeft()
	{
		mapController.Animate("Right");
		yield return new WaitForSeconds(1/60);
		while (player.transform.position != leftDestination.transform.position) 
		{
			player.transform.position = Vector3.MoveTowards(player.transform.position, leftDestination.transform.position, speed * Time.deltaTime);
			yield return null;
		}
		mapController.Animate("Stop");
	}

	// Moves the player to the right destination 
	IEnumerator DoRight()
	{	
		mapController.Animate("Left");
		yield return new WaitForSeconds(1/60);
		while (player.transform.position != rightDestination.transform.position) 
		{
			player.transform.position = Vector3.MoveTowards(player.transform.position, rightDestination.transform.position, speed * Time.deltaTime);
			yield return null;
		}
		mapController.Animate("Stop");		
	}
}

