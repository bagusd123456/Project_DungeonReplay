using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    
    public Image damageImage;
    public AudioClip deathClip;

    Animator anim;
    AudioSource playerAudio;
    //ShootController _shootController;
    bool isDead;
    public bool damaged;

    public ItemEntity detectedItem;
    public WeaponEntity detectedWeapon;

    public static PlayerHealth Instance { get; private set; }

    void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        //Mendapatkan reference komponen
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        //_shootController = GetComponentInChildren<ShootController>();
        currentHealth = startingHealth;
    }


    void Update()
    {
        
    }

    //fungsi untuk mendapatkan damage
    public void TakeDamage(int amount)
    {
        damaged = true;

        //mengurangi health
        currentHealth -= amount;

        //Memainkan suara ketika terkena damage
        playerAudio.Play();

        //Memanggil method Death() jika darahnya kurang dari sama dengan 10 dan belu mati
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }


    void Death()
    {
        isDead = true;

        //playerShooting.DisableEffects ();

        //mentrigger animasi Die
        anim.SetTrigger("Die");

        //Memainkan suara ketika mati
        playerAudio.clip = deathClip;
        playerAudio.Play();

        ////mematikan script player movement
        //_playerMovement.enabled = false;

        //_shootController.enabled = false;
    }

    public void CollectItem()
    {
        if (detectedItem != null)
        {
            detectedItem.Collect();
        }

        if (detectedWeapon != null)
        {
            detectedWeapon.Collect();
        }
    }

    public void RestartLevel()
    {
        //meload ulang scene dengan index 0 pada build setting
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Collectible"))
        {
            other.gameObject.TryGetComponent(out detectedItem);
            other.gameObject.TryGetComponent(out detectedWeapon);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        detectedItem = null;
        detectedWeapon = null;
        Debug.Log("iam not detecting items");
    }
}
