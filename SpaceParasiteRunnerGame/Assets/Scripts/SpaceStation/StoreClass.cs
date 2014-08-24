using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoreClass : MonoBehaviour 
{
	//public Item itemScript = null;
	GameObject ItemSpawn;	
	Ship ShipClass; 
	
	public enum ShopType
	{
		None,
		Bar,
		LifeSupport,
		ShipParts
	}
	
	public ShopType shopType;
	public GameObject[] StoreInventory;
	public List<GameObject> StoreSlots = new List<GameObject>();
	//GameObject[] StoreSlots;

	void Start () 
	{
		string itemType; 
		if(this.CompareTag("BarShop"))
		{
			shopType = ShopType.Bar;
			itemType = "BarPrefab";
		}
		else if (this.CompareTag ("LifeSupportShop"))
		{
			shopType = ShopType.LifeSupport;
			itemType = "LifeSupportPrefab";
		}
		else if (this.CompareTag("ShipPartShop"))
		{
			shopType = ShopType.ShipParts;
			itemType = "ShipPartPrefab";
		}
		else 
		{
			shopType = ShopType.None;
			itemType = "ItemDummy";
		}
		
		if(this.shopType == ShopType.Bar)
		{
		}

		foreach(Transform child in transform)
		{
			StoreSlots.Add(child.gameObject);
		}
		//StoreSlots = GameObject.FindGameObjectsWithTag("ShopSpot");
		//StoreSlots = GetComponentsInChildren<GameObject>();
		//StoreInventory = new GameObject[StoreSlots.Length];
		StoreInventory = new GameObject[StoreSlots.Count];
		//for(int i = 0; i < StoreSlots.Length; i++)//swap 3 for variable of amount of slots, swap vector 3 for dummyposition array
		for(int i = 0; i < StoreSlots.Count; i++)//swap 3 for variable of amount of slots, swap vector 3 for dummyposition array
		{
			ItemSpawn = (GameObject)Instantiate(Resources.Load (itemType), /*new Vector3(0,0,0)*/StoreSlots[i].transform.position, Quaternion.identity);
			ItemSpawn.GetComponent<ItemClass>().Initiate(shopType);
			StoreInventory[i]=ItemSpawn;
		}
		//itemScript = new Item();
		//itemScript.doSomething();
		//CreateInventory();
	}

//	void Update () 
//	{
//		//needed?
//	}
	
	
	void CompareStoreAndShip()
	{
		GetShipInfo();
	}
	
	
	void GetShipInfo()
	{
	
	}
	
	
//	void CreateInventory()
//	{
//		
//	}
}
