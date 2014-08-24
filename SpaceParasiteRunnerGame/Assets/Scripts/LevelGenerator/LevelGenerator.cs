using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {
	[SerializeField] private List<GameObject> AllPossibleSegments;
	GameObject firstSegment;
	public float levelTime = 10.0f;

	// Use this for initialization
	void Awake () {
		InitializeLevel();
	}

	private IEnumerator KillTrack() {
		yield return new WaitForSeconds(5.0f);

		firstSegment.transform.GetComponent<TrackSegment>().DiscardBlock(2.0f);
	}

	public void InitializeLevel() {
		TrackSegment.AllPossibleSegments = AllPossibleSegments;
		TrackSegment.TrackDuration = levelTime;
		TrackSegment.levelCreationTimeStamp = Time.time;
		firstSegment = (GameObject) Instantiate(AllPossibleSegments[0], Vector3.zero, Quaternion.identity);
		
		//StartCoroutine(KillTrack());
	}
}
