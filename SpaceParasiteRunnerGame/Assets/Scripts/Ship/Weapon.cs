using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	public GameObject g_ShotPrefab;
	public Transform t_Weapon;
	public float f_RefireDelay;
	[HideInInspector]
	public float f_LastFired;
	public float f_WeaponDamage;
	public float f_ShotSpeed;
	public float f_OverheatValue;

	// Use this for initialization
	void Awake()
	{
		f_LastFired = 0;
		t_Weapon = transform;
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}
}
