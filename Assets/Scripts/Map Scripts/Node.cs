using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour {
	[Header("Level Info")]
	public string levelName;
	public int levelNumber;


	[Header("Node Destinations")]
	public GameObject upDestination;
	public GameObject downDestination;
	public GameObject leftDestination;
	public GameObject rightDestination;


	[Header("If this is the first level in your map, tick this:")]
	public bool accessible;


	[Header("Dependencies")]
	public GameObject player;
	private bool currentNode;
	public Text levelNameText;
	public GameObject levelMenu;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.position == player.transform.position) {
			currentNode = true;
	
		} else {
			currentNode = false;
			levelMenu.SetActive (false);
		}

		if (currentNode) 
		{
			levelNameText.text = levelName;
			levelMenu.SetActive (true);
		}

		if (upDestination != null) 
		{
			if (upDestination.GetComponent<Node>()) 
			{
				if (upDestination.GetComponent<Node> ().accessible == false) 
				{
					upDestination.SetActive (false);
				}
			}

			else if (upDestination.GetComponent<Corner>()) 
			{
				if (upDestination.GetComponent<Corner> ().accessible == false) 
				{
					upDestination.SetActive (false);
				}
			}

			else if (upDestination.GetComponent<Warp>()) 
			{
				if (upDestination.GetComponent<Warp> ().accessible == false) 
				{
					upDestination.SetActive (false);
				}
			}
		}

		if (downDestination != null) 
		{
			if (downDestination.GetComponent<Node>()) 
			{
				if (downDestination.GetComponent<Node> ().accessible == false) 
				{
					downDestination.SetActive (false);
				}
			} 

			else if (downDestination.GetComponent<Corner>()) 
			{
				if (downDestination.GetComponent<Corner> ().accessible == false) 
				{
					downDestination.SetActive (false);
				}
			} 

			else if (downDestination.GetComponent<Warp>()) 
			{
				if (downDestination.GetComponent<Warp> ().accessible == false) 
				{
					downDestination.SetActive (false);
				}

			}
		}

		if (leftDestination != null) 
		{
			if (leftDestination.GetComponent<Node>()) 
			{
				if (leftDestination.GetComponent<Node> ().accessible == false) 
				{
					leftDestination.SetActive (false);
				}
			} 

			else if (leftDestination.GetComponent<Corner>()) 
			{
				if (leftDestination.GetComponent<Corner> ().accessible == false) 
				{
					leftDestination.SetActive (false);
				}
			} 

			else if (leftDestination.GetComponent<Warp>()) 
			{
				if (leftDestination.GetComponent<Warp> ().accessible == false)
				{
					leftDestination.SetActive (false);
				}
			}
		}

		if (rightDestination != null) 
		{
			if (rightDestination.GetComponent<Node>()) 
			{
				if (rightDestination.GetComponent<Node> ().accessible == false) 
				{
					rightDestination.SetActive (false);
				}
			} 

			else if (rightDestination.GetComponent<Corner>()) 
			{		
				if (rightDestination.GetComponent<Corner> ().accessible == false) 
				{
					rightDestination.SetActive (false);
				}
			} 

			else if (rightDestination.GetComponent<Warp>()) 
			{
				if (rightDestination.GetComponent<Warp> ().accessible == false) 
				{
					rightDestination.SetActive (false);
				}
			}
		}
				
		else
		{
			accessible = true;
			if (upDestination != null) 
			{
				upDestination.SetActive (true);
				if (upDestination.GetComponent<Node> ()) 
				{
					upDestination.GetComponent<Node> ().accessible = true;
				} 
				else if (upDestination.GetComponent<Corner> ()) 
				{
					upDestination.GetComponent<Corner> ().accessible = true;
				} 
				else if (upDestination.GetComponent<Warp> ()) 
				{
					upDestination.GetComponent<Warp> ().accessible = true;
				}
			}

			if (downDestination != null) 
			{
				downDestination.SetActive (true);
				if (downDestination.GetComponent<Node> ()) 
				{
					downDestination.GetComponent<Node> ().accessible = true;
				} 
				else if (downDestination.GetComponent<Corner> ()) 
				{
					downDestination.GetComponent<Corner> ().accessible = true;
				} 
				else if (downDestination.GetComponent<Warp> ()) 
				{
					downDestination.GetComponent<Warp> ().accessible = true;
				}
			}

			if (leftDestination != null) 
			{
				leftDestination.SetActive (true);
				if (leftDestination.GetComponent<Node> ()) 
				{
					leftDestination.GetComponent<Node> ().accessible = true;
				} 
				else if (leftDestination.GetComponent<Corner> ()) 
				{
					leftDestination.GetComponent<Corner> ().accessible = true;
				}
				else if (leftDestination.GetComponent<Warp> ()) 
				{
					leftDestination.GetComponent<Warp> ().accessible = true;
				}
			}

			if (rightDestination != null) 
			{
				rightDestination.SetActive (true);
				if (rightDestination.GetComponent<Node> ()) 
				{
					rightDestination.GetComponent<Node> ().accessible = true;
				} 
				else if (rightDestination.GetComponent<Corner> ()) 
				{
					rightDestination.GetComponent<Corner> ().accessible = true;
				} 
				else if (rightDestination.GetComponent<Warp> ()) 
				{
					rightDestination.GetComponent<Warp> ().accessible = true;
				}
			}				
		}

		if (Input.GetKeyDown (KeyCode.UpArrow)) 
		{
			if (upDestination != null) 
			{
				if (upDestination.activeInHierarchy) 
				{
					currentNode = false;
					Invoke ("PlayButtonDisable", 1 / 6f);
					StartCoroutine (DoUp ()); 
				}
			}
		} 

		else {
			if (Input.GetKeyDown (KeyCode.DownArrow)) 
			{
				if (downDestination != null) 
				{
					if (downDestination.activeInHierarchy)
					{
						currentNode = false;
						Invoke ("PlayButtonDisable", 1 / 6f);
						StartCoroutine (DoDown ()); 
					}
				}
			} 

			else {
				if (Input.GetKeyDown (KeyCode.LeftArrow)) 
				{
					if (leftDestination != null) 
					{
						if (leftDestination.activeInHierarchy) 
						{
							currentNode = false;
							Invoke ("PlayButtonDisable", 1 / 6f);
							StartCoroutine (DoLeft ()); 
						}
					}
				} 

				else {
					if (Input.GetKeyDown (KeyCode.RightArrow)) 
					{
						if (rightDestination != null) 
						{
							if (rightDestination.activeInHierarchy) 
							{
								currentNode = false;
								Invoke ("PlayButtonDisable", 1 / 6f);
								StartCoroutine (DoRight ()); 
							}
						}
					}
				}
			}
		}
	}


	IEnumerator DoUp()
	{
		yield return new WaitForSeconds (1/60);
		while (player.transform.position != upDestination.transform.position) 
		{
			player.transform.position = Vector3.MoveTowards (player.transform.position, upDestination.transform.position, 8f * Time.deltaTime);
			yield return null;
		}
	}
	IEnumerator DoDown()
	{
		yield return new WaitForSeconds (1/60);
		while (player.transform.position != downDestination.transform.position) 
		{
			player.transform.position = Vector3.MoveTowards (player.transform.position, downDestination.transform.position, 8f * Time.deltaTime);
			yield return null;
		}
	}
	IEnumerator DoLeft()
	{
		yield return new WaitForSeconds (1/60);
		while (player.transform.position != leftDestination.transform.position) 
		{
			player.transform.position = Vector3.MoveTowards (player.transform.position, leftDestination.transform.position, 8f * Time.deltaTime);
			yield return null;
		}
	}
	IEnumerator DoRight()
	{	
		yield return new WaitForSeconds (1/60);
		while (player.transform.position != rightDestination.transform.position) 
		{
			player.transform.position = Vector3.MoveTowards (player.transform.position, rightDestination.transform.position, 8f * Time.deltaTime);
			yield return null;
		}		
	}
}

