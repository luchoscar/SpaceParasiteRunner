using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour
{
	float f_Damage;
	//GameObject g_Ship;
	//Ship s_Ship;
	// Use this for initialization
	void Start()
	{
		//g_Ship = GameObject.FindGameObjectWithTag("Player");
		//s_Ship = g_Ship.GetComponent<Ship>();
		//f_Damage = s_Ship.GetWeaponDamage();
		//rigidbody.AddForce(g_Ship.transform.forward * 20, ForceMode.VelocityChange);
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

	public void SetDamage(float damage)
	{
		f_Damage = damage;
	}

	void OnCollisionEnter(Collision other)
	{
		// Damage whatever is hit, as applicable
	}
}