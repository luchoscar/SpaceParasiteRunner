using UnityEngine;
using System.Collections;

public class HealthPickup : BaseObject {

    public float healthOnPickup = 5.0f;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameEvents.TriggerHealthPickup(healthOnPickup);
            SpawnEffects();
            Destroy (gameObject, 0.1f);
        }
    }
}
