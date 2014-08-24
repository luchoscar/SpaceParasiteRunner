using UnityEngine;
using System.Collections;

public class TestShipMov : MonoBehaviour {
	public Vector3 desiredDirection = Vector3.zero;
	public Transform ship;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += 30.0f * transform.forward * Time.deltaTime;
		ship.forward = transform.forward;
	}
}
