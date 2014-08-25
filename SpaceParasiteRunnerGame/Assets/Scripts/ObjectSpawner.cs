using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour {

	public enum ObjType
	{
		Asteroid,
		HP,
		Fuel,
		Minerals
	}

	[System.Serializable]
	public class SpawnInfo
	{
		public ObjType objType;
		public string name;
		public BaseObject prefab;
		public float weight = 1.0f;
		public int minPerSegment = 1;
		public int maxPerSegment = 10;
		public float minScale = 1.0f;
		public float maxScale = 1.0f;
	}

	[SerializeField] 
	public List<SpawnInfo> infoTable;

	public BaseObject SpawnObjAt (ObjType objType, Vector3 position)
	{
		var objs = infoTable.FindAll(s => s.objType == objType);
		if(objs == null || objs.Count == 0)
		{
			Debug.LogError("No objs of type" + objType);
			return null;
		}

		var obj = objs[Random.Range (0, objs.Count)];
		if(obj != null)
			return (BaseObject)Instantiate (obj.prefab, position, Quaternion.identity);
		else
			Debug.LogError("No such object found in table: " + objType);

		return null;
	}

	public SpawnInfo InfoForObj(ObjType objType)
	{
		var objs = infoTable.FindAll(s => s.objType == objType);
		if(objs == null || objs.Count == 0)
		{
			Debug.LogError("No objs of type" + objType);
			return null;
		}
		
		var obj = objs[Random.Range (0, objs.Count)];
		return obj;
	}

	public void SpawnObjAt(Vector3 position)
	{
		var obj = infoTable [Random.Range (0, infoTable.Count - 1)];
		Instantiate (obj.prefab, position, Quaternion.identity);
	}
	 
}
