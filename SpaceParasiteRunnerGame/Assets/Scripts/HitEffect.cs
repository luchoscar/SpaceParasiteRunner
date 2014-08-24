using UnityEngine;
using System.Collections;

public class HitEffect : MonoBehaviour {

    public float effectLifeTime = 5.0f;

    void Start()
    {
        Destroy(gameObject, effectLifeTime);
    }
}
