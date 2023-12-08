using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [HideInInspector]
    public EnemyAttack enemyAttack;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.TakeDamage(enemyAttack.attackDamage);
                Destroy(GetComponentInChildren<Light>());
                Destroy(gameObject);
            }

        }
        else
        {
            
        }

        Destroy(gameObject, 3.5f);
    }
}
