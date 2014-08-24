using UnityEngine;
using System.Collections;

public class HullArmorClass : ItemClass 
{
	
	float f_DamageResistance;
	float f_SpeedPenalty;
	// Use this for initialization
	
	void Start () 
	{
		type = PartType.HullArmor;
		f_DamageResistance = Random.Range(10.0f, 26.0f);
		f_SpeedPenalty = Random.Range(10.0f, 26.0f);
	}
}
