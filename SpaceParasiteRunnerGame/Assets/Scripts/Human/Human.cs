using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Human : MonoBehaviour {
	#region Variables
	// infected variables	
	[SerializeField] private float health = 1.0f;								// health of this human
	[SerializeField] private bool isInfected = false;							// whether this human is being controlled by the parasite
	[SerializeField] private static float infectionRate = 0.005f;				// infection rate of parasite
	[SerializeField] private float currentInfectionRate;						// this human current rate of infection when controlled by parasite
	[SerializeField] private float recoveryRate = 0.00125f;						// this human recovery rate of infection when not controlled by parasite
	[SerializeField] private float parasiteResistance = 0.5f;					// this human resistance to infection
	[SerializeField] public List<Transform> humanInfected {private set; get;}	// list of infested humans
	[SerializeField] public int maxHumanInfected {private set; get;}			// max number of humans to infect		
	
	// stats variables
	public List<float> shipStats {private set; get;}							// list of ship stats
	public List<float> lifeSupport {private set; get;}							// list of life support stats
	[SerializeField] public static List<float> humanStats {private set; get;}	// list of this human stats
	
	// movement variables
	[SerializeField] private float walkingSpeed;			// walking speed
	[SerializeField] private float turningSpeed;			// turning speed
	private List<Transform> path = new List<Transform>();	// list of nodes to transverse in order to get to destination
	private Transform destination;							// next node to move to
	private Transform leader = null;						// infested human to follow back to the ship
	
	Currency currency = Currency.Instance;
	#endregion

	#region UnityFunctions
	// Use this for initialization
	void Start () {
		currentInfectionRate = infectionRate;
		humanInfected = new List<Transform>();
		shipStats = new List<float>();
		lifeSupport = new List<float>();
		
		GameObject [] newPath = GameObject.FindGameObjectsWithTag("Path");
		int idx = 0;
		foreach(GameObject go in newPath) {
			go.name += ("_" + idx);
			idx++;
			path.Add(go.transform);
		}
		
		SetPath(path);
	}
	
	// Update is called once per frame
	void Update () {
		Walk ();
		UpdateHealth();
		
		if (Input.GetKeyUp(KeyCode.Q)) EnterEnvironment(10.0f);
		else if (Input.GetKeyUp(KeyCode.W)) GetInfected(10.0f);
		else if (Input.GetKeyUp(KeyCode.E)) currency.CollectMineral(5);
	}
	
	void OnGUI() {
	
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0, new Color(0.5f, 0.0f, 0.0f));
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(new Rect(0.0f, 0.0f, 500.0f, 25.0f), GUIContent.none);
		
		Texture2D texture2 = new Texture2D(1, 1);
		texture2.SetPixel(0,0, new Color(1.0f, 0.0f, 0.0f));
		texture2.Apply();
		GUI.skin.box.normal.background = texture2;
		GUI.Box(new Rect(0.0f, 0.0f, 500.0f * health, 25.0f), GUIContent.none);
	}
	#endregion
	
	#region WalkFunction
	/// <summary>
	/// Walk this human
	/// </summary>
	private void Walk() {
		if (ReachDestination()) return;
		
		// if controlled by parasite, this object decides where to move
		if (isInfected) {
			if (destination != null){
				Vector3 direction = destination.position - transform.position;
				Debug.DrawRay(transform.position, direction, Color.red);
				if(direction.magnitude <= 0.1f) {
					GetNextNode();
				}
				
				direction.Normalize();
				
				if (Vector3.Dot(transform.forward, direction) <= 0.01f) transform.forward = direction;
				transform.forward = Vector3.Slerp(transform.forward, direction, turningSpeed * Time.deltaTime);
				transform.position += transform.forward * walkingSpeed * Time.deltaTime;;
			}
			else 
				Debug.Log("Reached destination");
		}
	}
	
	/// <summary>
	/// Sets the path to follow
	/// </summary>
	/// <returns>The path.</returns>
	/// <param name="In_path">List of Vector3 position to transverse.</param>
	public void SetPath(List<Transform> In_path) {
		path = In_path;
		
		// set path on the same y-plane as this object
		for(int i = 0; i < path.Count; i++){
			Transform node = path[i];
			node.position = new Vector3(path[i].position.x, transform.position.y, path[i].position.z);
			path[i] = node;
		}
		
		GetNextNode();
	}
	
	/// <summary>
	/// Checks whether human has reached the destination.
	/// </summary>
	/// <returns><c>true</c>, if destination was reached, <c>false</c> otherwise.</returns>
	public bool ReachDestination() {
		return (destination == null);
	}
	
	/// <summary>
	/// Gets the next node to move to.
	/// </summary>
	/// <returns>The next node or null if we have reached the desitnation.</returns>
	private Transform GetNextNode() {
		if (path.Count == 0) {
			return null;
		}
		else {
			destination = path[0];
			path.RemoveAt(0);	
			return destination;
		}
	}
	#endregion
	
	#region InfectionFunctions
	/// <summary>
	/// Updates the health of human based on infected flag.
	/// </summary>
	private void UpdateHealth() {
		if (isInfected) {
			health -= infectionRate * Time.deltaTime;
			
			if (health <= 0.0f) {
				health = 0.0f;
				Debug.Log("You are dead");
			}
		}
		else if (health < 1.0f) {
			health += recoveryRate * Time.deltaTime;
		}
		else {
			health = 1.0f;
			Debug.Log("Human fully healed");
		}
		
	}
	
	/// <summary>
	/// Resets currentInfectionRate based on environment hazzard
	/// </summary>
	/// <param name="In_environmentHazzard">In_environment hazzard.</param>
	public void EnterEnvironment(float In_environmentHazzard = 1.0f) {
		currentInfectionRate *= In_environmentHazzard;
	}
	
	/// <summary>
	/// Get human target to infect
	/// </summary>
	/// <param name="target">Target.</param>
	private void InfectTarget(Transform target) {
		target.GetComponent<Human>().GetInfected();
	}
	
	/// <summary>
	/// Infect this current human
	/// </summary>
	/// <param name="In_environmentHazzard">In_environment hazzard.</param>
	public void GetInfected(float In_environmentHazzard = 1.0f) {
		currentInfectionRate = infectionRate * parasiteResistance * In_environmentHazzard;
		isInfected = true;
		humanInfected.Add(transform);
	}
	
	/// <summary>
	/// Changes the human body to control.
	/// </summary>
	/// <returns>The transform of the human body to control.</returns>
	public Transform ChangeBody(int index) {
		isInfected = false;
		
		return humanInfected[index];
	}
	#endregion
	
	#region StatsFunctions
	/// <summary>
	/// Sets the ship stats and life support stats.
	/// </summary>
	/// <param name="In_shipStats">In_ship stats.</param>
	/// <param name="In_lifeSupport">In_life support.</param>
	public void SetShipStats(List<float> In_shipStats, List<float> In_lifeSupport) {
		shipStats = In_shipStats;
		lifeSupport = In_lifeSupport;
	}
	#endregion
}
