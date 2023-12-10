using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [HideInInspector]
    public EnemyAttack enemyAttack;
    public int projectileDamage;

    private Rigidbody projectileRB;
    public float projectileSpeed = 15f;
    public void LaunchForward()
    {
        projectileRB.AddForce(transform.forward * projectileSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) return;

        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerHealth>();

            if (player != null)
            {
                if (enemyAttack != null)
                    player.TakeDamage(enemyAttack.attackDamage);
                else
                    player.TakeDamage(projectileDamage);
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
