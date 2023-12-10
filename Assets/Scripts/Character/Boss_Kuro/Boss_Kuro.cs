using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Boss_Kuro : EnemyHealth
{
    public PlayerHealth player;

    public enum state {Idle, Chase, RangedAttack, AreaAttack, Laser}
    public state _state = state.Chase;

    public float currentActionTime;
    public float attackCooldown = 1.5f;

    public float minimumRadius;
    public float distance;

    public GameObject projectile;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 15f;
    public int projectileDamage = 15;
    [Space] 
    public GameObject projectileLaser;

    [Header("Attack Behavior")]
    public int activeSkillIndex;
    public List<MonoBehaviour> availableSkill = new List<MonoBehaviour>();

    [Header("Movement Behavior")]
    public NavMeshAgent agent;

    [Header("Skill Set")] 
    public KuroAOE areaSkill;
    public List<Vector3> areaPosition = new List<Vector3>();
    [Space] 
    public AOEDamage aoeDamage;

    public bool isPlaying = false;
    private void OnValidate()
    {
        
    }
    private void Awake()
    {
        base.Awake();
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCondition>();
        player = FindObjectOfType<PlayerHealth>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        base.Update();

        distance = Vector3.Distance(transform.position, PlayerHealth.Instance.transform.position);

        if (currentActionTime > 0)
            currentActionTime -= Time.deltaTime;
        else
        {
            if (distance > minimumRadius)
            {
                _state = state.Chase;
            }
            else
            {
                transform.DOLookAt(PlayerHealth.Instance.transform.position, 1f);
                agent.destination = Vector3.zero;
                agent.isStopped = true;

                int ratio = Random.Range(1, 4);
                Debug.Log($"ratio is: {ratio}");
                if (ratio == 1)
                    _state = state.RangedAttack;
                else if (ratio == 2)
                    _state = state.Laser;
                else if (ratio == 3)
                    _state = state.AreaAttack;
            }
            Movement();
            currentActionTime = attackCooldown;
        }

        ExecuteSkill();
    }

    public void ExecuteSkill()
    {
        for (int i = 0; i < availableSkill.Count; i++)
        {
            if (availableSkill[i] == availableSkill[activeSkillIndex])
                availableSkill[i].enabled = true;
            else
                availableSkill[i].enabled = false;
        }
    }

    public void Movement()
    {
        currentActionTime = attackCooldown;

        switch (_state)
        {
            case state.Idle:
                break;
            case state.Chase:
                MoveToPlayer();
                break;
            case state.RangedAttack:
                SpawnProjectile();
                break;
            case state.AreaAttack:
                if (!isPlaying)
                    StartCoroutine(nameof(StartAOE));
                break;
            case state.Laser:
                if(!isPlaying)
                    StartCoroutine(nameof(StartLaser));
                break;
        }
    }

    public void MoveTowards(Vector3 targetPos)
    {
        agent.SetDestination(targetPos);
    }

    public void MoveToPlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(PlayerHealth.Instance.transform.position);
    }

    public void BasicAttack()
    {
        if (currentActionTime <= 0)
        {
            SpawnProjectile();
        }
    }

    public void SpawnProjectile()
    {
        var GO = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.localRotation);
        EnemyProjectile[] projectileGOs = GO.GetComponentsInChildren<EnemyProjectile>();
        foreach (var VARIABLE in projectileGOs)
        {
            VARIABLE.gameObject.SetActive(true);
            VARIABLE.projectileDamage = projectileDamage;
            VARIABLE.GetComponent<Rigidbody>().AddForce(projectileSpawnPoint.forward * projectileSpeed, ForceMode.Impulse);
        }
    }

    //Start Laser Attack
    IEnumerator StartLaser()
    {
        isPlaying = true;
        yield return new WaitForSeconds(0.2f);
        projectileLaser.SetActive(true);
        //anim.SetTrigger("isAttacking");

        yield return new WaitForSeconds(3.5f);
        Debug.Log("Stop Laser");
        projectileLaser.SetActive(false);
        isPlaying = false;
    }

    IEnumerator StartAOE()
    {
        isPlaying = true;
        Vector3 targetPos = player.transform.position;
        targetPos.y = targetPos.y - player.transform.position.y / 2;
        yield return new WaitForSeconds(0.5f);
        Instantiate(aoeDamage, player.transform.position, aoeDamage.transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Instantiate(aoeDamage, player.transform.position, aoeDamage.transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Instantiate(aoeDamage, player.transform.position, aoeDamage.transform.rotation);

        yield return new WaitForSeconds(3f);
        isPlaying = false;
    }

    float CurrentAngle()
    {
        Vector3 dir = transform.parent.position - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 180f;
        return angle;
    }

    Vector3 GetPosition(float degrees, float dist)
    {
        float a = degrees * Mathf.PI / 180f;
        return new Vector3(Mathf.Sin(a) * dist, Mathf.Cos(a) * dist, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, minimumRadius);
    }
}
