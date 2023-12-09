using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DestroyVFX : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(DestroyVFXObject), 5f);
    }

    void DestroyVFXObject()
    {
        Destroy(this.gameObject);
    }
}
