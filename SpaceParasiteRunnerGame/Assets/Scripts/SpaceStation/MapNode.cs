using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapNode : MonoBehaviour
{
	public GameObject[] neighbors;
	Stack<Vector3> path = new Stack<Vector3>();

	public GameObject[] GetNeighbors()
	{
		return neighbors;
	}

	public Stack<Vector3> BuildPath(GameObject Destination)
	{
		return path;
	}

	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}
}
