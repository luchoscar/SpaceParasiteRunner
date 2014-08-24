using UnityEngine;
using System.Collections;

public class InTrackRedirection : MonoBehaviour {
	public Transform target;
	private Vector3 direction;

	// Use this for initialization
	void Start () {
		direction = (target.position - transform.position).normalized;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player") && other.transform.parent == null) {
			direction = (target.position - other.transform.position).normalized;
			other.transform.GetComponent<TestShipMov>().desiredDirection = direction;
			other.transform.forward = direction;
			other.transform.position = transform.position;
			Destroy(gameObject);
		}
	}
}
