using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;

    [Header("Enemy State")]
    public AudioClip deathClip;

    [HideInInspector]
    public Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    public bool isDead;
    public bool isReady = false;
    bool isSinking;

    [Header("On Enemy Die")] 
    public List<EnemyDrop> enemyDropList;

    public virtual void Awake()
    {
        //Mendapatkan reference komponen
        anim = GetComponentInChildren<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        //Set current health
        currentHealth = startingHealth;
    }


    public virtual void Update()
    {
        //Check jika sinking
        if (isSinking)
        {
            //memindahkan object kebawah
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage(int amount)
    {
        //Check jika dead
        if (isDead)
            return;

        //play audio
        //enemyAudio.Play();

        //kurangi health
        currentHealth -= amount;

        if(hitParticles != null)
        {
            //Ganti posisi particle
            hitParticles.transform.position = transform.position;

            //Play particle system
            hitParticles.Play();
        }
        

        //Dead jika health <= 0
        if (currentHealth <= 0)
        {
            Death();
        }
    }


    void Death()
    {
        //SetCapcollider ke trigger
        capsuleCollider.isTrigger = true;

        //set isdead
        isDead = true;

        //trigger play animation Dead
        //anim.SetTrigger("Dead");

        //Play Sound Dead
        //enemyAudio.clip = deathClip;
        //enemyAudio.Play();
        StartSinking();
    }

    public void StartSinking()
    {
        //disable Navmesh Component
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        //Set rigidbody ke kimematic
        //GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;
        //ScoreManager.score += scoreValue;
        Destroy(gameObject, 2f);
    }

    [ContextMenu("SpawnDropItem")]
    public void SpawnDropItem()
    {
        var itemDropScript = ProbableElement.RandomWeightedElement(enemyDropList);

        GameObject item = itemDropScript.item;
        int amount = ProbableElement.RandomWeightedElement(itemDropScript.dropAmountList).amount;

        for (int i = 0; i < amount; i++)
        {
            Instantiate(item, transform.position, Quaternion.identity);
        }
    }
}

[Serializable]
public class EnemyDrop : ProbableElement
{
    public GameObject item;
    public float probability;

    public float Probability
    {
        get => probability;
        set
        {

        }
    }

    public List<DropAmount> dropAmountList = new List<DropAmount>();
}

[Serializable]
public class DropAmount : ProbableElement
{
    public int amount;
    public float probability;

    public float Probability
    {
        get => probability;
        set
        {

        }
    }
}