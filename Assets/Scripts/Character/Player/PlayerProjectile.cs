using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int damageAmount;
    public GameObject impactParticle;

    private void OnEnable()
    {
        Invoke(nameof(DisableGameObject), 3.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemyHealth = other.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
                
            }

        }
        Instantiate(impactParticle, transform.position, Quaternion.identity);
        Destroy(GetComponentInChildren<Light>());

        gameObject.SetActive(false);

        Invoke(nameof(DisableGameObject), 3.5f);
    }

    public void DisableGameObject()
    {

        gameObject.SetActive(false);
    }
}
