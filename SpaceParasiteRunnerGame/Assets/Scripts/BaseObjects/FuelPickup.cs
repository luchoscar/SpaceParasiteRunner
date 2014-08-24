using UnityEngine;
using System.Collections;

public class FuelPickup : BaseObject {

    public float fuelOnPickup = 5.0f;
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameEvents.TriggerFuelPickup(fuelOnPickup);
            SpawnEffects();
            Destroy (gameObject, 0.1f);
        }
    }
}
