using UnityEngine;
using System.Collections;

public class EngineClass : ItemClass
{

	
	public float f_Thrust;
	public float f_FuelUsage;//consumption rate
	public float f_FuelStorageMax;
	public float f_FuelCurrent;
	
	void Start()
	{
		type = PartType.Engine;
		f_Thrust = Random.Range(10.0f, 26.0f);
		f_FuelUsage = Random.Range(10.0f, 26.0f);
		f_FuelStorageMax = Random.Range(10.0f, 26.0f);
		f_FuelCurrent = Random.Range(10.0f, 26.0f);
	}
}
