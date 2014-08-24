using UnityEngine;
using System.Collections;

public class Tester : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    GameEvents.AsteroidHitPlayer += delegate(GameEvents.AsteroidHitArgs args) {
            Debug.Log(string.Format("Asteroid HIT! Taking {0} dmg!", args.Asteroid.damage));
       };

        GameEvents.AsteroidWasShot += delegate(GameEvents.AsteroidHitArgs args) {
            Debug.Log(string.Format("Asteroid was SHOT!!", args.Asteroid.health));
        };

        GameEvents.ItemPickedUp += delegate(PickupType pickup, float value)
        {
            if(pickup == PickupType.Health)
                Debug.Log(string.Format("Health pickup! Taking {0} hp!", value));
                
            if(pickup == PickupType.Fuel)
                Debug.Log(string.Format("Fuel pickup! Gaining {0} fuel!", value));

            if(pickup == PickupType.Minerals)
                Debug.Log(string.Format("Minerals pickup! Gaining {0} $$!", value));
        };
	}
}
