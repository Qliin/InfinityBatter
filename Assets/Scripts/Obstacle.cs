using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour 
{	
	private Transform respawnPoint;
	private float moveSpeed = 5.5f;

	void Start()
	{
		respawnPoint = GameObject.FindWithTag("RespawnPoint").transform;

	}

	void Update () 
	{
		transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

		if(transform.position.x < respawnPoint.position.x)
		{
			transform.position = transform.parent.position;
			gameObject.SetActive(false);
		}
	}
}
