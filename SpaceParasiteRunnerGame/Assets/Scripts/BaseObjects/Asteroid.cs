using UnityEngine;
using System.Collections;

public class Asteroid : BaseObject {

	public int damage = 1;
	public int health = 5;

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			GameEvents.TriggerAsteroidHitPlayer(new GameEvents.AsteroidHitArgs(this));
            SpawnEffects();
            Destroy (gameObject, 0.1f);
		}

        if(other.gameObject.CompareTag("Shot"))
        {
            GameEvents.TriggerAsteroidWasShot(new GameEvents.AsteroidHitArgs(this));
            SpawnEffects();
            Destroy (gameObject, 0.1f);
        }
	}
}
