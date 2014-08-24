using UnityEngine;
using System.Collections;

public class MineralPickup : BaseObject {

    public float minerals;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameEvents.TriggerMineralPickup(minerals);
            SpawnEffects();
            Destroy (gameObject, 0.1f);
        }
    }
}
