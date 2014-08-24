using UnityEngine;
using System.Collections;

public class Mineral : MonoBehaviour {
	Currency currency = Currency.Instance;
	[SerializeField] int value;
	Vector3 randomAxis;
	[SerializeField] float rotationSpeed;

	// Use this for initialization
	void Start () {
		transform.rotation = Random.rotation;
		value = Random.Range(25, 101);
		randomAxis = Random.insideUnitSphere.normalized;
		rotationSpeed = Random.Range(3.0f, 30.0f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(transform.position, randomAxis, rotationSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			currency.CollectMineral(value);
		}
	}


}
