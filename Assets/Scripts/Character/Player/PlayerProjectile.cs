using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [HideInInspector]
    public int damageAmount;

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
        else
        {

        }

        Invoke(nameof(DisableGameObject), 3.5f);
    }

    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
