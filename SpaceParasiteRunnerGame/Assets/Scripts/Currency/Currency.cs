using UnityEngine;
using System.Collections;

public class Currency {
	private int totalMinerals;			// total currency collected
	private static Currency instance;	// singelton instance
	
	/// <summary>
	/// Instance constructtor
	/// </summary>
	private Currency() {
		totalMinerals = 0;
	}
	
	public static Currency Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new Currency();
			}
			return instance;
		}
	}
	
	/// <summary>
	/// Collects the minerals for currency.
	/// </summary>
	/// <param name="mineralValue">Mineral value.</param>
	public void CollectMineral(int mineralValue) {
		totalMinerals += mineralValue;
	}
	
	/// <summary>
	/// Check whether we have enough currency to buy an item
	/// </summary>
	/// <returns><c>true</c> if this instance has enough minerals to buyt the desired item; otherwise, <c>false</c>.</returns>
	/// <param name="itemValue">Value of item to buy.</param>
	private bool HasEnough(int itemValue) {
		return totalMinerals >= itemValue;
	}
	
	/// <summary>
	/// Spend minerals to buy item, if we do not have enough display respective message
	/// </summary>
	/// <param name="itemValue">Value of item to buy.</param>
	public void BuyItem(int itemValue) {
		
		if (HasEnough(itemValue)) {
			totalMinerals -= itemValue;
			// TODO: Message displaying succesfull transaction
		}
		else {
			// TODO: UI message indicating not enough money
			Debug.Log("Not enough money");
		}
	}
}
