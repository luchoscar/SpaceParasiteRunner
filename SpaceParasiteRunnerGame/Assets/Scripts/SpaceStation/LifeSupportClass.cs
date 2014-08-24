using UnityEngine;
using System.Collections;

public class LifeSupportClass : ItemClass 
{
	[SerializeField]
	public float f_HealRate {private set; get;}
	
	void Start()
	{
		type = PartType.LifeSupport;
		f_HealRate = Random.Range(10.0f, 25.1f);
	}
}