using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour 
{	 
	private List<GameObject> instantiatedObstacles = new List<GameObject>();
	private float respawnTime = 6f;

	void Start () 
	{
		GameObject[] loadedObstacles = Resources.LoadAll<GameObject>("Obstacles");

		foreach(GameObject obstacle in loadedObstacles)
		{
			GameObject instantiatedObstacle = Instantiate(obstacle, transform) as GameObject;
			instantiatedObstacle.transform.position = transform.position;
			instantiatedObstacle.SetActive(false);
			instantiatedObstacles.Add(instantiatedObstacle);
		}
		StartCoroutine(SpawnObstacles());
	}

	IEnumerator SpawnObstacles()
	{
		int obstacleToSpawn = Random.Range(0,4);

		while(instantiatedObstacles[obstacleToSpawn].activeInHierarchy)
		{
			obstacleToSpawn++;
			if(obstacleToSpawn > instantiatedObstacles.Count - 1)
			{
				obstacleToSpawn = 0;
			}
		}

		instantiatedObstacles[obstacleToSpawn].SetActive(true);

		yield return new WaitForSeconds(respawnTime);
		StartCoroutine(SpawnObstacles());
	}
}
