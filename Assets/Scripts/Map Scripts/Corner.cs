using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corner : MonoBehaviour {

	[Header("Corner Destination")]
	[SerializeField] private GameObject destination;

	[Header("Corner Destination Reverse")]
	[SerializeField] private GameObject destinationReverse;

	private GameObject player;
	private bool flip = false;
	private float speed = 5f;  

	void Start()
	{
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

	IEnumerator DoFollow()
	{
		yield return new WaitForSeconds (1/60);
		if (!flip)
		{
			while (player.transform.position != destination.transform.position)
			{
				player.transform.position = Vector3.MoveTowards (player.transform.position, destination.transform.position, speed * Time.deltaTime);
				yield return null;
			}
			flip = true; 			
		}
		else
		{
			while (player.transform.position != destinationReverse.transform.position)
			{
				player.transform.position = Vector3.MoveTowards (player.transform.position, destinationReverse.transform.position, speed * Time.deltaTime);
				yield return null;
			}
			flip = false; 
		}
	}
}
