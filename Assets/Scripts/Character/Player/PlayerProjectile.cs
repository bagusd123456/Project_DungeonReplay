using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int damageAmount;

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
                Destroy(GetComponentInChildren<Light>());
                gameObject.SetActive(false);
            }

        }
    }

    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
