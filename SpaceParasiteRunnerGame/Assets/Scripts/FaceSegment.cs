using UnityEngine;
using System.Collections;

public class FaceSegment : MonoBehaviour {

    private Quaternion facingDirection = Quaternion.identity;
    
    void LateUpdate()
    {
        transform.rotation = facingDirection;
    }

    
    void OnTriggerStay(Collider collider)
    {
        var segment = collider.GetComponent<Segment>();
        if (segment)
        {
            facingDirection = segment.Direction.rotation;
        }
    }
}
