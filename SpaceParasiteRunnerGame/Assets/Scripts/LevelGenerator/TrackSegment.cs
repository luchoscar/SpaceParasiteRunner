using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackSegment : MonoBehaviour {
	private static float levelCreationTimeStamp;			// time in which this level was created.  Once certain amount of time has passed
															// the space dock station will be generated
	private TrackSegment rearSegment;						// segment that created this segment, it is attached to the rear of this segment
	private TrackSegment [] forwardSegments;				// all segments that are attached to the front of this segment
	[SerializeField] public int idType {private set; get;}	// id use to know which group this segment belongs
	[SerializeField] public int [] possibleSegmentIds;		// array of possible segment ids this block can attach to at the end
	private Transform [] nextSegmentPositions;				// positions where the next segments will be places
	private bool isBeingDestroied = false;					// flag indicating this segment is being destoied in order to prevent more than
															// one object to attempt to destroy it
	public bool hasBeenVisitedByPlayer {private set; get;}	// flag indicating that the player has visited this segment in order to start 
															// discard sequence once the rear segment is being discard

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		// when ship enters trigger, redirect it and start discard coroutine for previous segment
		if (other.gameObject.CompareTag("Player")) {
			other.transform.forward = transform.forward;
			rearSegment.DiscardBlock();
		}
	}

	public void DiscardBlock() {
		if (isBeingDestroied) return;

		isBeingDestroied = true;

		StartCoroutine(DiscardThisBlock());
	}

	private IEnumerator DiscardThisBlock() {
		yield return new WaitForSeconds(5.0f);

		// if the player has not gone into a segment created by this, discard that segment
		foreach(TrackSegment trackSegment in forwardSegments) {
			if (!trackSegment.hasBeenVisitedByPlayer)
				trackSegment.DiscardBlock();
		}

		if (gameObject)	Destroy (gameObject);
	}

	private IEnumerator CreateNewSegment() {
		yield return new WaitForSeconds(2.0f);

		if (Time.time - levelCreationTimeStamp >= 30.0f) {
			// TODO: create dock station in order for ship to 
		}
	}
}
