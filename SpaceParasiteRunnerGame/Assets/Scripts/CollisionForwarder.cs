using UnityEngine;
using System.Collections;

public class CollisionForwarder : MonoBehaviour {

	[SerializeField] private GameObject _target;

	void OnCollisionEnter(Collision collision)
	{
		_target.SendMessage ("OnCollisionEnter", collision, SendMessageOptions.DontRequireReceiver);
	}

	void OnCollisionStay(Collision collision)
	{
		_target.SendMessage ("OnCollisionStay", collision, SendMessageOptions.DontRequireReceiver);
	}

	void OnCollisionExit(Collision collision)
	{
		_target.SendMessage ("OnCollisionExit", collision, SendMessageOptions.DontRequireReceiver);
	}

	void OnTriggerEnter(Collider collider)
	{
		_target.SendMessage ("OnTriggerEnter", collider, SendMessageOptions.DontRequireReceiver);
	}

	void OnTriggerStay(Collider collider)
	{
		_target.SendMessage ("OnTriggerStay", collider, SendMessageOptions.DontRequireReceiver);
	}

	void OnTriggerExit(Collider collider)
	{
		_target.SendMessage ("OnTriggerExit", collider, SendMessageOptions.DontRequireReceiver);
	}

}
