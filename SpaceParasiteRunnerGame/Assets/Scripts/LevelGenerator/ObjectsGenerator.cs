using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectsGenerator : MonoBehaviour {
	
	public ObjectSpawner spawner;
	public TrackSegment segment;

	void Awake()
	{
		if(!spawner)
			spawner = GameObject.FindWithTag("ObjSpawner").GetComponent<ObjectSpawner>();

		if(segment)
			segment = GetComponent<TrackSegment>();
	}

	// Use this for initialization
	void Start () {

		float maxRadius = GetMaxRadius ();
		Transform s = transform.FindChild ("StartNode");
		Transform e = transform.FindChild ("EndNode");

		if(!s || !e)
			return;

		
		SpawnType (ObjectSpawner.ObjType.Asteroid, maxRadius, s.position, e.position);

		SpawnType (ObjectSpawner.ObjType.Fuel, maxRadius, s.position, e.position);
		
		SpawnType (ObjectSpawner.ObjType.Minerals, maxRadius, s.position, e.position);
		
		SpawnType (ObjectSpawner.ObjType.HP, maxRadius, s.position, e.position);
	}

	private void SpawnType(ObjectSpawner.ObjType type, float radius, Vector3 start, Vector3 end)
	{
		int count = 0;
		var info = spawner.InfoForObj (type);
		count = Mathf.RoundToInt(Mathf.Lerp (info.minPerSegment, info.maxPerSegment, info.weight));

		for(int i = 0; i < count; i++)
		{
			var obj = spawner.SpawnObjAt(type, GetRandPositionInSegment(0.55f * radius, start, end));
			if(obj)
			{
				obj.transform.parent = transform;
				obj.transform.localScale = Vector3.one * Random.Range(info.minScale, info.maxScale);
			}
		}
	}

	private Vector3 GetRandPositionInSegment (float radius, Vector3 start, Vector3 end)
	{
		float rt = Random.Range (0.0f, 1.0f);
		Vector3 pos = Vector3.Lerp (start, end, rt);

		float rr = Random.Range (0, radius);
		var rrSphere = rr * Random.insideUnitSphere;
		return pos + rrSphere;
	}

	private float GetMaxRadius()
	{
		float maxRadius = 0;
		var colliders = GetComponentsInChildren<Collider> ();
		if(colliders != null)
		{
			foreach(var c in colliders)
			{
				if(c is SphereCollider)
				{
					var sc = c as SphereCollider;
					float wRadius = sc.transform.localToWorldMatrix.MultiplyPoint(sc.radius * Vector3.up).magnitude;
					if(maxRadius < wRadius)
						maxRadius = wRadius;
				}
				else if(c is CapsuleCollider)
				{
					var cc = c as CapsuleCollider;
					float wRadius = cc.transform.localToWorldMatrix.MultiplyPoint(cc.radius * Vector3.up).magnitude;
					if(maxRadius < wRadius)
						maxRadius = wRadius;
				}
			}
		}
		return maxRadius;
	}
}
