using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapControls : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit, 100))
			{
				if(hit.collider.CompareTag("LifeSupportShop"))
				{
					print("LifeSupportShop");
				}
				else if(hit.collider.CompareTag("BarShop"))
				{
					print("BarShop");
				}
				else if(hit.collider.CompareTag("ShipPartShop"))
				{
					print("ShipPartShop");
				}
			}
		}
	}
}
