﻿using UnityEngine;
using System.Collections;

public class ItemClass : MonoBehaviour
{
	 
	public enum PartType
	{
		None,
		Engine,
		Laser,
		HullArmor,
		LifeSupport
	}
	public GameObject model;
	public PartType type = PartType.None;
	public float f_StorePrice = 0.0f;
	bool b_IsHighlighted;
	
	// Use this for initialization
	void Start () 
	{
		b_IsHighlighted = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//needed?
	}
	/// <summary>
	/// Sets shop inventory type based on shop's ShopType.
	/// </summary>
	/// <param name="ShopType">Shop type.</param>
	public void Initiate(StoreClass.ShopType ShopType)
	{
		switch(ShopType)
		{
			case StoreClass.ShopType.Bar:
				this.tag = "BarItem";
			break;
			
			case StoreClass.ShopType.LifeSupport:
				this.tag = "LifeSupportItem";
			break;
			
			case StoreClass.ShopType.ShipParts:
				this.tag = "ShipPartItem";
			break;
		}
	}
	
	public void doSomething() //test function
	{
		Debug.Log("Do Something Has been called");
	}	
	
	void OnMouseOver()
	{
		
	}
	
	void OnGUI()
	{
		
	}
}

