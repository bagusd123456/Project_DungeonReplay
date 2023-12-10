using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DestroyVFX : MonoBehaviour
{
    public int expoDamage = 15;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(DestroyVFXObject), 5f);
    }

    void DestroyVFXObject()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(expoDamage);
        }
    }
}
