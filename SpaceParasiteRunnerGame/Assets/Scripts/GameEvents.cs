using UnityEngine;
using System.Collections;

public class GameEvents : MonoBehaviour {

	public class AsteroidHitArgs
	{
		public Asteroid Asteroid { get; set;}
		public AsteroidHitArgs(Asteroid asteroid)
		{
            Asteroid = asteroid;
		}
	}

	public delegate void AsteroidEventHandler(AsteroidHitArgs args);
	public static event AsteroidEventHandler AsteroidHitPlayer;
    public static event AsteroidEventHandler AsteroidWasShot;
	public static void TriggerAsteroidHitPlayer(AsteroidHitArgs args)
	{
		if (AsteroidHitPlayer != null)
        {
            AsteroidHitPlayer(args);
        }
	}
    
    public static void TriggerAsteroidWasShot(AsteroidHitArgs args)
    {
        if (AsteroidWasShot != null)
        {
            AsteroidWasShot(args);
        }
    }


    public delegate void PickupEventHandler(PickupType pickupType, float value);
    public static event PickupEventHandler ItemPickedUp;
    public static void TriggerHealthPickup(float health)
    {
        if (ItemPickedUp != null)
        {
            ItemPickedUp(PickupType.Health, health);
        }
    }

    public static void TriggerFuelPickup(float fuel)
    {
        if (ItemPickedUp != null)
        {
            ItemPickedUp(PickupType.Fuel, fuel);
        }
    }

    public static void TriggerMineralPickup(float minerals)
    {
        if (ItemPickedUp != null)
        {
            ItemPickedUp(PickupType.Minerals, minerals);
        }
    }
}
