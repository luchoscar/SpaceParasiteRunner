using UnityEngine;
using System.Collections;

public class TestShipMov : MonoBehaviour {
	public Vector3 desiredDirection;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += 1.0f * transform.forward * Time.deltaTime;
	}
}
