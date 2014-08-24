using UnityEngine;
using System.Collections;

public class CameraTracking : MonoBehaviour
{
	public Transform t_CenterPoint;
	public Transform t_TrackingTarget;
	public float f_FocalPercentage;

	// Use this for initialization
	void Start()
	{
		if(f_FocalPercentage < 0)
		{
			f_FocalPercentage = 0;
		}
		else if(f_FocalPercentage > 1)
		{
			f_FocalPercentage = 1;
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		transform.LookAt(t_CenterPoint.position + ((t_TrackingTarget.position - t_CenterPoint.position) * f_FocalPercentage));
	}
}
