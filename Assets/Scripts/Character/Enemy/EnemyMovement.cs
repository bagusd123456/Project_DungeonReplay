using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public enum EnemyState { STAY, CHASE, BUSY, DIED}
    public EnemyState _enemyState;
    public bool alwaysChase = false;

    Transform player;
    Rigidbody rb;
    
    public Animator anim;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    EnemyAttack enemyAttack;

    [HideInInspector]
    public UnityEngine.AI.NavMeshAgent nav;

    [Header("Enemy Detection Parameter")]
    public float speed;
    public float sightDistance = 6f;

    public bool stop;
    public bool onSight;

    private void Awake ()
    {
        //Cari game
        player = GameObject.FindGameObjectWithTag("Player").transform;

        ////Mendapatkan Reference component
        //if(anim == null)
        //    anim = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyAttack = GetComponent<EnemyAttack>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        nav.enabled = false;
    }


    void Update ()
    {
        //speed = rb.velocity;
        //anim.SetFloat("speed", speed);

        CheckRadius();

        //Memindahkan posisi player
        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0 && _enemyState == EnemyState.CHASE)
        {
            Invoke("EnableNav", .1f);
            if (nav.enabled)
            {
                MoveBehavior();
            }
                
        }

        else //hentikan Moving
        {
            nav.enabled = false;
        }
    }

    void CheckRadius()
    {
        Ray raySight = new Ray(transform.position, transform.forward);

        Physics.Raycast(raySight.origin,raySight.direction, sightDistance, LayerMask.NameToLayer("Character"));

        if(gameObject.GetComponent<EnemyAttack>().distance <= sightDistance || alwaysChase)
        {
            _enemyState = EnemyState.CHASE;
        }

        else
        {
            _enemyState = EnemyState.STAY;
            //anim.SetBool("Stop", true);
        }
    }

    public void MoveBehavior()
    {
        if (enemyAttack._attackType == EnemyAttack.attackType.MELEE)
        {
            if (gameObject.GetComponent<EnemyAttack>().distance > .5f)
            {
                EnemyMove();
            }

            else
            {
                EnemyStop();
            }
        }

        if (enemyAttack._attackType == EnemyAttack.attackType.SPITTER)
        {
            if (gameObject.GetComponent<EnemyAttack>().distance > enemyAttack.attackRadius)
            {

                EnemyMove();
            }

            else
            {
                EnemyStop();
            }
        }

        if (enemyAttack._attackType == EnemyAttack.attackType.BOMBER)
        {
            if (gameObject.GetComponent<EnemyAttack>().distance > .5f)
            {

                EnemyMove();
            }

            else
            {
                EnemyStop();
            }
        }

        if (enemyAttack._attackType == EnemyAttack.attackType.LASER)
        {
            if (gameObject.GetComponent<EnemyAttack>().distance > enemyAttack.attackRadius && !enemyAttack.isAttacking)
            {
                EnemyMove();
            }

            else
            {
                EnemyStop();
            }
        }
    }

    private void EnemyMove()
    {
        //anim.SetBool("Stop", false);
        nav.SetDestination(player.position);
        nav.isStopped = false;
    }

    private void EnemyStop()
    {
        //anim.SetBool("Stop", true);
        nav.isStopped = true;
    }

    public void EnableNav()
    {
        nav.enabled = true;
    }

    public void DisableNav()
    {
        nav.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Ray raySight = new Ray(transform.position + (Vector3.up * 1.25f), transform.forward);
        Debug.DrawRay(raySight.origin, raySight.direction * sightDistance, Color.red);
    }
}
