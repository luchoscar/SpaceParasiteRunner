using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour
{
	public Transform t_Root;
	public Transform t_RotationRoot;
	public Transform t_VerticalSlide;

	// Basic Control Variables
	Vector2 v2_DirectionInput;
	float f_RotationSpeedScale;
	float f_RotationFinalSpeed;
	//float f_VerticalSpeedScale;
	float f_VerticalSpeed;
	//float f_VerticalFinalSpeed;
	float f_Shaking;
	float f_MaxHeightOffset;
	float f_ForwardSpeed;
	float f_TriggerTimeout;

	// Baseline ship controls
	float f_Acceleration;
	float f_MaxSpeed;
	float f_Shakiness;

	// Inherited from Ship Parts
	float f_AccelerationShipMod;
	float f_MaxSpeedShipMod;
	float f_ShakinessShipMod;
	float f_WeaponDamageShipMod;

	// Inherited from Pilots
	float f_AccelerationPilotMod;
	float f_MaxSpeedPilotMod;
	float f_ShakinessPilotMod;
	float f_WeaponDamagePilotMod;

//	[HideInInspector]
//	public float f_Acceleration;
//	[HideInInspector]
//	public float f_MaxSpeed;
//	[HideInInspector]
//	public float f_Shakiness;

	// Ship Attributes
	float f_ShipHealth;
	float f_ShipControl;
	float f_ShipFuel;
	float f_ShipFuelConsumptionRate;
	//??? ?_CurrentPilot;

	// Weapon Attributes
	public List<Weapon> w_Weapons = new List<Weapon>();
//	public GameObject[] g_ShotPrefab;
//	public Transform[] t_Weapon;
//	public float[] f_RefireDelay;
//	float[] f_LastFired;
//	public float[] f_WeaponDamage;
//	public float[] f_ShotSpeed;
	float f_LastShotFired;
	//float f_WeaponFireRate;
	bool b_WeaponOverheated;
	float f_WeaponOverheatTotal;
	float f_WeaponOverheatMax;
	float f_WeaponOverheatDelay;
	float f_WeaponCooldownRate;

	// Use this for initialization
	void Start()
	{
		v2_DirectionInput = new Vector2(0.0f, 0.0f);
		f_Acceleration = 10;
		f_MaxSpeed = 3;
		f_Shakiness = 0;
		f_MaxHeightOffset = 2;
		//f_ForwardSpeed = 5;
		f_ForwardSpeed = 30;
		f_VerticalSpeed = 3;
		t_RotationRoot.rigidbody.maxAngularVelocity = f_MaxSpeed * 2;
		foreach(Transform child in transform)
		{
			if(child.CompareTag("Weapon"))
			{
				w_Weapons.Add(child.GetComponent<Weapon>());
			}
		}
		f_WeaponOverheatDelay = 0.75f;
		f_WeaponCooldownRate = 50;
		f_WeaponOverheatMax = 100;
		b_WeaponOverheated = false;
	}
	
	// Update is called once per frame
	void Update()
	{
		v2_DirectionInput = new Vector2(Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		t_RotationRoot.transform.position = t_Root.position;
		f_Shaking = f_Shakiness * Mathf.Sin(Time.time * 9.83f) * Mathf.Cos(Time.time * 16.322f);

		if(!b_WeaponOverheated && f_WeaponOverheatTotal > f_WeaponOverheatMax)
		{
			StartCoroutine(WeaponOverheat(f_WeaponOverheatDelay * 6));
		}
		if(!b_WeaponOverheated && f_WeaponOverheatDelay < Time.time - f_LastShotFired && f_WeaponOverheatTotal > 0)
		{
			f_WeaponOverheatTotal -= f_WeaponCooldownRate * Time.deltaTime;
		}
		else if(f_WeaponOverheatTotal < 0)
		{
			f_WeaponOverheatTotal = 0;
		}

		if(Input.GetKey("space") && !b_WeaponOverheated)
		{
			FireWeapons();
		}
	}

	void FixedUpdate()
	{
		Rotate();
		MoveVertical();
		MoveForward();
	}

	void Rotate()
	{
		f_RotationSpeedScale = Vector3.Dot(v2_DirectionInput.x * t_Root.forward, t_RotationRoot.rigidbody.angularVelocity);
		f_RotationFinalSpeed = Mathf.Lerp(0, f_Acceleration, (f_MaxSpeed - f_RotationSpeedScale) / f_MaxSpeed);
		f_RotationFinalSpeed = Mathf.Clamp(f_RotationFinalSpeed, 0, f_Acceleration);
		t_RotationRoot.rigidbody.AddTorque((v2_DirectionInput.x * t_Root.forward * f_RotationFinalSpeed) + (t_Root.forward * f_Shaking), ForceMode.Acceleration);
	}

	void MoveVertical()
	{
		//f_RotationSpeedScale = Vector3.Dot(v2_DirectionInput.y * t_RotationRoot.up, 
		//f_VerticalFinalSpeed = Mathf.Lerp(0, f_Acceleration, Mathf.Abs(f_MaxHeightOffset * t_VerticalSlide.localPosition.y) / f_MaxHeightOffset);
		//f_VerticalSpeedScale = Vector3.Dot(v2_DirectionInput.y * t_RotationRoot.up, t_RotationRoot.up);
		//f_VerticalSpeedScale = Vector3.Dot(v2_DirectionInput.y * t_RotationRoot.up, t_VerticalSlide.localPosition / f_MaxHeightOffset);
//		f_VerticalSpeedScale = Mathf.Lerp(0, 1, 1 - ((f_MaxHeightOffset - Mathf.Abs(t_VerticalSlide.localPosition.y)) / f_MaxHeightOffset));
//		f_VerticalFinalSpeed = Mathf.Lerp(-1, 1, (f_MaxSpeed - f_VerticalSpeedScale) / f_MaxSpeed);
//		f_VerticalSpeed += f_VerticalFinalSpeed * v2_DirectionInput.y * Time.fixedDeltaTime;
//		f_VerticalSpeed = Mathf.Lerp(f_VerticalSpeed, 0, 3.0f * Time.fixedDeltaTime);
		//t_VerticalSlide.localPosition = new Vector3(t_VerticalSlide.localPosition.x, t_VerticalSlide.localPosition.y + f_VerticalSpeed, t_VerticalSlide.localPosition.z);
		//t_VerticalSlide.localPosition = new Vector3(t_VerticalSlide.localPosition.x, t_VerticalSlide.localPosition.y + (f_Acceleration * v2_DirectionInput.y * Time.fixedDeltaTime), t_VerticalSlide.localPosition.z);
		t_VerticalSlide.localPosition = new Vector3(t_VerticalSlide.localPosition.x, t_VerticalSlide.localPosition.y + (f_VerticalSpeed * v2_DirectionInput.y * Time.fixedDeltaTime) + ((1 - v2_DirectionInput.y) * -t_VerticalSlide.localPosition.y * Time.fixedDeltaTime), t_VerticalSlide.localPosition.z);
		if(Mathf.Abs(t_VerticalSlide.localPosition.y) > f_MaxHeightOffset)
		{
			t_VerticalSlide.localPosition = new Vector3(t_VerticalSlide.localPosition.x, Mathf.Sign(t_VerticalSlide.localPosition.y) * f_MaxHeightOffset, t_VerticalSlide.localPosition.z);
		}
		//t_VerticalSlide.rigidbody.AddForce(t_VerticalSlide.up * f_Acceleration, ForceMode.Acceleration);
	}

	void MoveForward()
	{
		t_Root.position = t_Root.position + (t_Root.forward * f_ForwardSpeed * Time.fixedDeltaTime);
	}

	public void RepairShip(float amount)
	{
		f_ShipHealth += amount;
	}

	// Note: Verify positive/negative and adjust accordingly
	public void TakeHit(float damage)
	{
		f_ShipHealth -= damage;
	}

	public void ChangePilot(int pilot)
	{

	}

	public void SetWeapon(int index, Weapon weapon)
	{
		if(index > w_Weapons.Count)
		{
			w_Weapons.Add(weapon);
		}
		else
		{
			w_Weapons[index] = weapon;
		}
		//t_Weapon[index] = weapon;
	}

	public float GetWeaponDamage(int index)
	{
		return w_Weapons[index].f_WeaponDamage;
	}

	public void SetHumanModifiers(float accelerationMod, float maxSpeedMod, float controlMod)
	{
		f_AccelerationPilotMod = accelerationMod;
		f_MaxSpeedPilotMod = maxSpeedMod;
		f_ShakinessPilotMod = controlMod;

	}

	public void SetShipModifiers(float accelerationMod, float maxSpeedMod, float controlMod)
	{
		f_AccelerationShipMod = accelerationMod;
		f_MaxSpeedShipMod = maxSpeedMod;
		f_ShakinessShipMod = controlMod;
	}

	void FireWeapons()
	{
		//Instantiate shots from both guns
		for(int i = 0; i < w_Weapons.Count; i++)
		{
			if(w_Weapons[i].f_RefireDelay < Time.time - w_Weapons[i].f_LastFired)
			{
				GameObject shot = Instantiate(w_Weapons[i].g_ShotPrefab, w_Weapons[i].t_Weapon.position, Quaternion.identity) as GameObject;
				Physics.IgnoreCollision(shot.collider, collider);
				Shot shotScript = shot.GetComponent<Shot>();
				shotScript.SetDamage(w_Weapons[i].f_WeaponDamage);
				shot.rigidbody.AddForce(transform.forward * w_Weapons[i].f_ShotSpeed, ForceMode.VelocityChange);
				Destroy(shot, 7.0f);
				w_Weapons[i].f_LastFired = Time.time;
				f_LastShotFired = Time.time;
				f_WeaponOverheatTotal += w_Weapons[i].f_OverheatValue;
			}
			//GameObject rightShot = Instantiate(g_RightShotPrefab, t_RightWeapon.position, Quaternion.identity) as GameObject;
			//Destroy(rightShot, 7.0f);
		}
	}

	void ExitShip()
	{

	}

	void EnterShip()
	{

	}

	public void ChangeDirection(Quaternion newDirection)
	{
		//print (Time.time + " --- " + f_TriggerTimeout);
		if(f_TriggerTimeout < Time.time - 1.0f)
		{
			t_Root.rotation = newDirection;
			f_TriggerTimeout = Time.time;
		}
	}

	IEnumerator WeaponOverheat(float cooldownDelay)
	{
		b_WeaponOverheated = true;
		yield return new WaitForSeconds(cooldownDelay);
		f_WeaponOverheatTotal = 0;
		b_WeaponOverheated = false;
	}

//	void OnTriggerEnter(Collider other)
//	{
//		if(other.CompareTag("Segment"))
//		{
//			Ship player = other.GetComponent<Ship>();
//			//player.ChangeDirection(Direction.forward);
//			player.ChangeDirection(transform.position - other.transform.position);
//		}
//	}

	void OnGUI()
	{
		GUI.Label(new Rect(0, 0, 250, 25), f_WeaponOverheatTotal.ToString("F3"));
		if(b_WeaponOverheated)
		{
			GUI.Label(new Rect(0, 25, 250, 25), "OVERHEATING!");
		}
	}

	void OnCollisionEnter(Collision other)
	{
		//if(other.tag == "Mineral")
		//{
		//	Currency grab = other.gameObject.GetComponent<Currency>();
		//}
	}
}
