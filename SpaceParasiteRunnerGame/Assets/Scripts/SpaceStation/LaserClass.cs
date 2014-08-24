using UnityEngine;
using System.Collections;

public class LaserClass : ItemClass 
{
	float f_OverheatMax;
	float f_OverheatRate;
	float f_FireRate;
	
	void Start()
	{
		type = PartType.Laser;
		f_OverheatMax = Random.Range(10.0f, 26.0f);
		f_OverheatRate = Random.Range(10.0f, 26.0f);
		f_FireRate = Random.Range(10.0f, 26.0f);
	}
}
