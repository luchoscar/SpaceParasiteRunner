using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackSegment : MonoBehaviour {
	#region Variables
	public static float TrackDuration;
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
	public static float levelCreationTimeStamp;								// time in which this level was created.  Once certain amount of time has passed
																			// the space dock station will be generated
	public TrackSegment rearSegment;										// segment that created this segment, it is attached to the rear of this segment
	public List<TrackSegment> forwardSegments = new List<TrackSegment>();	// all segments that are attached to the front of this segment
	[SerializeField] public int idType {private set; get;}					// id use to know which group this segment belongs
	public List<int> possibleSegmentIds;									// array of possible segment ids this block can attach to at the end
	[SerializeField] private Transform [] nextSegmentPositions;				// positions where the next segments will be places
	private bool isBeingDestroied = false;									// flag indicating this segment is being destoied in order to prevent more than
																			// one object to attempt to destroy it
	public bool hasBeenVisitedByPlayer {private set; get;}					// flag indicating that the player has visited this segment in order to start 
																			// discard sequence once the rear segment is being discard

	public static List<GameObject> AllPossibleSegments;
	[SerializeField] public List<SegmentType> Connections = new List<SegmentType>();
	[SerializeField] private float splitPercentProbability = 0.15f;
	private bool createSpaceDockStation = false;
	public bool alreadyCreatedTrack = false;
	#endregion

	[SerializeField] public List<Transform> path;
	Vector3 currentPoint;
	public static Transform shipObject = null;
	public GameObject spaceStation;
	//public ObjectsGenerator generator;

	#region UnityFunctions
	// Use this for initialization
	void Awake () {
		if (gameObject.GetComponent<ObjectsGenerator>() == null)
			gameObject.AddComponent<ObjectsGenerator>();

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
	}

	void Start() {
		if (alreadyCreatedTrack == false)
			StartCoroutine(CreateNewSegment());

		if (shipObject == null) 
			shipObject = GameObject.Find("ShipPrefab").transform;

		path = new List<Transform>();

		switch(segmentType) {
		case SegmentType.STRAIGHT:
			path.Add(transform.FindChild("EndNode").transform);
			break;
		case SegmentType.SPLIT:
			path.Add(transform.FindChild("InTrackRedirection").transform);
			path.Add(transform.FindChild("EndNode_0").transform);
			path.Add(transform.FindChild("EndNode_1").transform);
			break;
		default:
			path.Add(transform.FindChild("InTrackRedirection").transform);
			path.Add(transform.FindChild("EndNode").transform);
			break;
		}

		spaceStation = (GameObject)Resources.Load("SpaceDock");
	}

	void Update() {
		if (rearSegment == null && hasBeenVisitedByPlayer == false && alreadyCreatedTrack == false) 
			DiscardBlock();

		if (hasBeenVisitedByPlayer && rearSegment == null) {
			if (Vector3.Distance(shipObject.position, currentPoint) <= 1.05f) {
				shipObject.position = currentPoint;
				CalculateMoveDirection();
			}
			//else 
				//shipObject.forward = Vector3.Slerp(shipObject.forward, (currentPoint - shipObject.position).normalized, 5.0f * Time.deltaTime);
			//	shipObject.LookAt((currentPoint - shipObject.position).normalized);
		}

		if (path.Count > 0) {
			Debug.DrawRay(shipObject.position + Vector3.up * 0.25f, (currentPoint - shipObject.position), Color.blue);
			for(int i = 0; i < path.Count; i++) {
				Vector3 origingPoint = Vector3.zero;
				if (i == 0) {
					origingPoint = transform.position;
				}
				else if (path[i - 1] != null)
					origingPoint = path[i - 1].position;

				if (path.Count > 0 && path[i] != null) {
					Vector3 directionPoint = path[i].position - origingPoint;
					Debug.DrawRay(origingPoint, directionPoint, Color.red);
				}
			}
		}
	}

	private void CalculateMoveDirection() {
		if (path.Count == 0) {
			currentPoint = Vector3.zero;
			return;
		}

		int index = 0;
		bool clearPath = false;
		if (segmentType == SegmentType.SPLIT && path.Count == 2) {
			if (Random.Range(0.0f, 1.0f) > 0.5f) index = 1;
			clearPath = true;
		}

		currentPoint = path[index].position;
		path.RemoveAt(0);

		if (clearPath) path.Clear();

		Vector3 direction = currentPoint - shipObject.position;
		direction = new Vector3(direction.x, 0.0f, direction.z).normalized;
		shipObject.forward = direction;
	}

	void OnTriggerEnter(Collider other) {
		// when ship enters trigger, redirect it and start discard coroutine for previous segment
		if (other.gameObject.CompareTag("Player") && other.transform.parent == null) {
			hasBeenVisitedByPlayer = true;
			CalculateMoveDirection();

			if (rearSegment) Destroy(rearSegment.gameObject);
			/*
			Transform startNode = this.transform.FindChild("StartNode").transform;
			hasBeenVisitedByPlayer = true;
			other.transform.forward = startNode.forward;
			other.transform.position = startNode.position;
			if (rearSegment)
				rearSegment.DiscardBlock();
			*/
		}

		if (createSpaceDockStation && hasBeenVisitedByPlayer && forwardSegments.Count == 0) {
			Vector3 dockPos = transform.position + 500.0f * transform.forward;

			Instantiate(spaceStation, dockPos, Quaternion.identity);
			Debug.Log("Creating Space Dock");
		}
	}
	#endregion
	public void DiscardBlock(float killTime = 1.0f) {
		Destroy (gameObject.collider);
		if (rearSegment) {
			rearSegment.DiscardBlock(killTime);
		}
		else
			StartCoroutine(DiscardThisBlock(killTime));
	}

	private IEnumerator DiscardThisBlock(float killTime = 1.0f) {
		yield return new WaitForSeconds(killTime);

//		// if the player has not gone into a segment created by this, discard that segment
//		foreach(TrackSegment trackSegment in forwardSegments) {
//			if (trackSegment != null && !trackSegment.hasBeenVisitedByPlayer && rearSegment == null)
//				trackSegment.DiscardBlock(killTime);
//		}

		if (gameObject)	Destroy (gameObject);
	}

	private IEnumerator CreateNewSegment() {

		yield return new WaitForSeconds(5.0f);

		if (Time.time - levelCreationTimeStamp >= TrackDuration) {
			createSpaceDockStation = true;
			Debug.Log("Time is up");
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
