using UnityEngine;
using System.Collections;

public class BaseObject : MonoBehaviour {

    public HitEffect hitEffectPrefab;

    public virtual void SpawnEffects()
    {
        if (hitEffectPrefab)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }
    }
}
