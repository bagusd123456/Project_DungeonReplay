using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public enum attackType { MELEE, SPITTER, BOMBER}
    public attackType _attackType = attackType.MELEE;

    public GameObject projectile;
    public Transform firePoint;

    public float attackRadius = 1.3f;
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;
    public LayerMask layer;

    public float distance;

    Animator anim;
    GameObject player;
    [HideInInspector]
    public PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    public bool playerInRange;
    public float timer;
    void Awake()
    {
        //Mencari game object dengan tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");

        //mendapatkan komponen player health
        playerHealth = player.GetComponent<PlayerHealth>();

        //mendapatkan komponen Animator
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        //playerInRange = Physics.CheckSphere(transform.position + Vector3.up * 1.02f, attackRadius, layer);

        distance = Vector3.Distance(transform.position, player.transform.position);
        if(distance <= attackRadius)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (timer >= timeBetweenAttacks && playerInRange /*&& playerHealth.currentHealth > 0*/)
        {
            //if (_attackType == attackType.BOMBER)
            //    Blow();
            //else if (_attackType == attackType.MELEE)
            //    gameObject.GetComponent<EnemyMovement>().anim.SetTrigger("attack");
            //else if (_attackType == attackType.SPITTER)
            //    gameObject.GetComponent<EnemyMovement>().anim.SetTrigger("attack");

            if (_attackType == attackType.BOMBER)
                Blow();
            else if (_attackType == attackType.MELEE)
                Attack();
            else if (_attackType == attackType.SPITTER)
                Attack();
        }
    }

    //Trigger Attack from Animation
    void Attack()
    {
        switch (_attackType)
        {
            case attackType.MELEE:
                //Reset timer
                timer = 0f;

                //Taking Damage
                if (playerHealth.currentHealth > 0)
                {
                    if (playerInRange)
                    {
                        gameObject.GetComponent<Transform>().LookAt(player.transform);
                        playerHealth.TakeDamage(attackDamage);
                    }

                }
                break;

            case attackType.SPITTER:
                //Reset timer
                timer = 0f;

                //Taking Damage
                if (playerHealth.currentHealth > 0)
                {
                    if (playerInRange)
                    {
                        gameObject.GetComponent<Transform>().LookAt(player.transform);
                        gameObject.GetComponent<EnemyMovement>()._enemyState = EnemyMovement.EnemyState.STAY;
                        var projectileGO = Instantiate(projectile, firePoint.position, firePoint.rotation);

                        EnemyProjectile[] projectileGOs = projectileGO.GetComponentsInChildren<EnemyProjectile>();
                        foreach (var VARIABLE in projectileGOs)
                        {
                            VARIABLE.GetComponent<Rigidbody>().AddForce(firePoint.forward * 2, ForceMode.Impulse);
                        }

                        //projectileGO.GetComponent<Rigidbody>().AddForce(firePoint.forward * Mathf.Sqrt(distance) * 2 + firePoint.up * Mathf.Sqrt(distance) * 2, ForceMode.Impulse);
                        //playerHealth.TakeDamage(attackDamage);
                    }
                }
                break;
        }
        
    }

    public void BasicAttack()
    {
        //Reset timer
        timer = 0f;

        ////Taking Damage
        //if (playerHealth.currentHealth > 0)
        //{
        //    if (distance <= 1.5f)
        //    {
        //        gameObject.GetComponent<Transform>().LookAt(player.transform);
        //        playerHealth.TakeDamage(attackDamage);
        //    }

        //}
    }

    public void Blow()
    {
        //Reset timer
        timer = 0f;

        //Taking Damage
        if (distance < 3f)
        {
            //var blowGO = Instantiate(projectile, transform.position, transform.rotation);
            //playerHealth.TakeDamage(attackDamage);
            enemyHealth.currentHealth = 0;
            Destroy(gameObject);
        }
    }

    float CalculateProjectileMotion(float x, Transform start, Transform target)
    {
        float a = Physics.gravity.y;
        float velocityX = start.position.x - target.position.x;
        float velocityY = Mathf.Sqrt(2 * (a * (start.position.y - target.position.y)));

        float projectileVelocity = Mathf.Sqrt(velocityX * velocityX + velocityY * velocityY);
        float trajectoryAngle = Mathf.Atan(velocityY / velocityX);

        return x * Mathf.Tan(trajectoryAngle) - ((a * x * x) / (2 * (projectileVelocity * projectileVelocity) * (Mathf.Cos(trajectoryAngle) * Mathf.Cos(trajectoryAngle))));
    }

    private void OnDrawGizmos()
    {
        if (playerInRange)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * .5f, attackRadius);
        
    }
}
