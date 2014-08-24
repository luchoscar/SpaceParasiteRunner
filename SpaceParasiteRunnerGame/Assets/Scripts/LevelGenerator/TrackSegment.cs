using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackSegment : MonoBehaviour {
	/*
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
	*/
	public enum SegmentType {
		NONE = -1,
		STRAIGHT = 0,
		LEFT_TURN_START = 1,
		RIGHT_TURN_START = 2,
		LEFT_TURN = 3,
		RIGHT_TURN = 4,
		LEFT_TURN_END = 5,
		RIGHT_TURN_END = 6,
		LEFT_TO_RIGHT = 7,
		RIGHT_TO_LEFT = 8,
		SPLIT = 9,
		ANY = 10
	}
	public SegmentType segmentType = SegmentType.NONE;						// type of segment, this is use to decide which segments will be created at its ends
	private static float levelCreationTimeStamp;							// time in which this level was created.  Once certain amount of time has passed
																			// the space dock station will be generated
	public TrackSegment rearSegment;										// segment that created this segment, it is attached to the rear of this segment
	public List<TrackSegment> forwardSegments = new List<TrackSegment>();	// all segments that are attached to the front of this segment
	[SerializeField] public int idType {private set; get;}					// id use to know which group this segment belongs
	private List<int> possibleSegmentIds;									// array of possible segment ids this block can attach to at the end
	[SerializeField] private Transform [] nextSegmentPositions;				// positions where the next segments will be places
	private bool isBeingDestroied = false;									// flag indicating this segment is being destoied in order to prevent more than
																			// one object to attempt to destroy it
	public bool hasBeenVisitedByPlayer {private set; get;}					// flag indicating that the player has visited this segment in order to start 
																			// discard sequence once the rear segment is being discard

	public static List<GameObject> AllPossibleSegments;
	[SerializeField] public List<SegmentType> Connections = new List<SegmentType>();
	[SerializeField] private float splitPercentProbability = 0.2f;

	// Use this for initialization
	void Awake () {
		levelCreationTimeStamp = Time.time;

		possibleSegmentIds = new List<int>();
		switch(segmentType) {
		case SegmentType.NONE:
			break;
		case SegmentType.STRAIGHT:
		case SegmentType.SPLIT:
			possibleSegmentIds.Add((int)SegmentType.STRAIGHT);
			possibleSegmentIds.Add((int)SegmentType.RIGHT_TURN_START);
			possibleSegmentIds.Add((int)SegmentType.RIGHT_TURN_START);
			possibleSegmentIds.Add((int)SegmentType.SPLIT);
			break;
		case SegmentType.LEFT_TURN_START:
			possibleSegmentIds.Add((int)SegmentType.STRAIGHT);
			possibleSegmentIds.Add((int)SegmentType.LEFT_TURN);
			possibleSegmentIds.Add((int)SegmentType.LEFT_TO_RIGHT);
			possibleSegmentIds.Add((int)SegmentType.SPLIT);
			break;
		case SegmentType.RIGHT_TURN_START:
			possibleSegmentIds.Add((int)SegmentType.STRAIGHT);
			possibleSegmentIds.Add((int)SegmentType.RIGHT_TURN);
			possibleSegmentIds.Add((int)SegmentType.LEFT_TO_RIGHT);
			possibleSegmentIds.Add((int)SegmentType.SPLIT);
			break;
		case SegmentType.LEFT_TURN:
		case SegmentType.RIGHT_TO_LEFT:
			possibleSegmentIds.Add((int)SegmentType.STRAIGHT);
			possibleSegmentIds.Add((int)SegmentType.LEFT_TURN);
			possibleSegmentIds.Add((int)SegmentType.LEFT_TO_RIGHT);
			possibleSegmentIds.Add((int)SegmentType.LEFT_TURN_END);
			possibleSegmentIds.Add((int)SegmentType.SPLIT);
			break;
		case SegmentType.RIGHT_TURN:
		case SegmentType.LEFT_TO_RIGHT:
			possibleSegmentIds.Add((int)SegmentType.STRAIGHT);
			possibleSegmentIds.Add((int)SegmentType.RIGHT_TURN);
			possibleSegmentIds.Add((int)SegmentType.RIGHT_TO_LEFT);
			possibleSegmentIds.Add((int)SegmentType.RIGHT_TURN_END);
			possibleSegmentIds.Add((int)SegmentType.SPLIT);
			break;
		case SegmentType.LEFT_TURN_END:
		case SegmentType.RIGHT_TURN_END:
			possibleSegmentIds.Add((int)SegmentType.STRAIGHT);
			possibleSegmentIds.Add((int)SegmentType.SPLIT);
			break;
		case SegmentType.ANY:

			for(int i = 0; i < AllPossibleSegments.Count; i++)
				possibleSegmentIds.Add(i);
			break;
		}

		StartCoroutine(CreateNewSegment());
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

	public void DiscardBlock(float killTime = 1.0f) {
		Destroy (gameObject.collider);
		if (rearSegment)
			rearSegment.DiscardBlock(killTime);

		StartCoroutine(DiscardThisBlock(killTime));
	}

	private IEnumerator DiscardThisBlock(float killTime = 1.0f) {
		yield return new WaitForSeconds(killTime);

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
			Debug.Log("Time to create space dock station");
		}
		else {
			int newSegmentCount = possibleSegmentIds.Count;

			if (Random.Range(0.0f, 1.0f) <= splitPercentProbability)
				newSegmentCount--;

			foreach(Transform nextPos in nextSegmentPositions) {
				int nextIndex = possibleSegmentIds[Random.Range(0, possibleSegmentIds.Count)];
				Transform newSegment = ((GameObject)Instantiate(AllPossibleSegments[nextIndex], nextPos.position, nextPos.rotation)).transform;
				newSegment.GetComponent<TrackSegment>().rearSegment = transform.GetComponent<TrackSegment>();
				forwardSegments.Add(newSegment.GetComponent<TrackSegment>());
				Connections.Add((SegmentType)nextIndex);
            }
		}
	}
}
