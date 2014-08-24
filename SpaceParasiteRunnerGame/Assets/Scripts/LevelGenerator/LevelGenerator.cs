using UnityEngine;
using System.Collections;
<<<<<<< HEAD

public class LevelGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
=======
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {
	[SerializeField] private List<GameObject> AllPossibleSegments;
	GameObject firstSegment;

	// Use this for initialization
	void Awake () {
		TrackSegment.AllPossibleSegments = AllPossibleSegments;

		firstSegment = (GameObject) Instantiate(AllPossibleSegments[0], Vector3.zero, Quaternion.identity);

		StartCoroutine(KillTrack());
	}

	private IEnumerator KillTrack() {
		yield return new WaitForSeconds(5.0f);

		firstSegment.transform.GetComponent<TrackSegment>().DiscardBlock();

>>>>>>> origin/master
	}
}
