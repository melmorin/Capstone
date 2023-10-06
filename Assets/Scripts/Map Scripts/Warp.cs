using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour {

	[Header("Warp Destination")]
	public GameObject warpDestination;

	[Header("Destination After")]
	public GameObject destination;

	[Header("Dependencies")]
	public GameObject player;

	private float speed = 5f; 
	
	// Update is called once per frame
	void Update () 
	{
		if (transform.position == player.transform.position) 
		{
			StartCoroutine(Teleport());
		}
	}

	// Teleports the player to their destination 
	private IEnumerator Teleport () 
	{
		yield return new WaitForSeconds(1);
		player.transform.position = warpDestination.transform.position;

		while (player.transform.position != destination.transform.position)
		{
			player.transform.position = Vector3.MoveTowards (player.transform.position, destination.transform.position, speed * Time.deltaTime);
			yield return null;
		}
	}
}
