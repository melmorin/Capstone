using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corner : MonoBehaviour {

	[Header("Corner Destination")]
	public GameObject destination;

	[Header("Direction the destination is in:")]
	public bool up;
	public bool down;
	public bool left;
	public bool right;

	[Header("Tick this for all ''_Reverse'' corners")]
	public bool accessible;

	[Header("Dependencies")]
	public GameObject player;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position == player.transform.position) {
			
			StartCoroutine (DoFollow ());
	}
}
	IEnumerator DoFollow()
	{
		yield return new WaitForSeconds (1/60);
		while (player.transform.position != destination.transform.position)
		{
			player.transform.position = Vector3.MoveTowards (player.transform.position, destination.transform.position, 8f * Time.deltaTime);
			yield return null;
		}
	}
}
