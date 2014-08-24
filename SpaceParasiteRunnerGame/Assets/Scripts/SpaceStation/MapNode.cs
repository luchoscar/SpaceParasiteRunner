using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapNode : MonoBehaviour
{
	public GameObject[] neighbors;

	public GameObject[] GetNeighbors()
	{
		return neighbors;
	}

	public Stack<Vector3> BuildPath(GameObject Start, GameObject Destination)
	{
		Stack<Vector3> path = new Stack<Vector3>();
		if(Destination == Start)
		{
			path.Push(Destination.transform.position);
			return path;
		}
		else
		{

		}
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
