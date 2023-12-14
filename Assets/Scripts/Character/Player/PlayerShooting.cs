﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.Serialization;

[System.Serializable]
public class LoadoutData
{
    [HideInInspector]
    public string weaponName;
    public WeaponData weaponData;
    public int magazine;
    public int ammo;

    public void InitData()
    {
        weaponName = weaponData.weaponName;
        magazine = weaponData.magazineCapacity;
        ammo = weaponData.ammoCapacity;
    }
}

public class PlayerShooting : MonoBehaviour
{
    [Header("Character Setup")]
    public Animator _animator;

    [Header("Weapon Setup")]
    public List<LoadoutData> loadoutDataArray;

    [Header("Projectile")]
    public List<GameObject> _projectileList;

    [Header("Defined Projectile")]
    public GameObject laserProjectile;

    [FormerlySerializedAs("PlayerLaserScript")] public PlayerLaserProjectile playerLaserScript;

    public int currentWeapon;
    public Transform firePointGO;
    [Space]

    private Vector2 mousePos;

    [Header("Shot Parameter")]
    public float range = 100f;

    public LayerMask shootableMask;

    private Ray shootRay = new Ray();

    public float Distance = 34f;

    public float shootTime;
    public float switchWeaponTime;

    public static PlayerShooting Instance { get; private set; }
    private void Awake()
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

        if (loadoutDataArray.Count > 0)
        {
            WeaponData weapon = loadoutDataArray[currentWeapon].weaponData;

            loadoutDataArray[currentWeapon].weaponName = weapon.name;
            loadoutDataArray[currentWeapon].ammo = weapon.ammoCapacity;
            loadoutDataArray[currentWeapon].magazine = weapon.magazineCapacity;

            WeaponSetup();
        }

        if(_animator == null)
            _animator = GetComponent<Animator>();
    }

    public void WeaponSetup()
    {
        //Get Weapon Magazine
        for (int i = 0; i < loadoutDataArray.Count; i++)
        {
            //_weaponMagazineList.Add(_weaponList[i].magazineCapacity);
            loadoutDataArray[i].magazine = loadoutDataArray[i].weaponData.magazineCapacity;
        }

        //Get Weapon Ammo
        for (int i = 0; i < loadoutDataArray.Count; i++)
        {
            //_weaponAmmoList.Add(_weaponList[i].ammoCapacity);
            loadoutDataArray[i].ammo = loadoutDataArray[i].weaponData.ammoCapacity;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //WeaponSwitch();

        ////On Hold Left Click
        //if (Mouse.current.leftButton.wasPressedThisFrame)
        //{
        //    //Shoot(_projectileList[currentWeapon]);
        //    if (loadoutDataArray.Count > 0)
        //        Shootv2(loadoutDataArray[currentWeapon].weaponData);
        //}

        ////On Press Right Click
        //if (Mouse.current.rightButton.isPressed)
        //{
        //    //Shoot(_projectileList[currentWeapon]);
        //    if (loadoutDataArray.Count > 0)
        //        Shootv2(loadoutDataArray[currentWeapon].weaponData);
        //}

        //if (Keyboard.current.rKey.wasPressedThisFrame)
        //{
        //    if (loadoutDataArray.Count > 0)
        //        WeaponReload(loadoutDataArray[currentWeapon].weaponData);
        //}

        //Calculate time Can Shoot
        /*if (shootTime >= 0f)
            shootTime -= Time.deltaTime;*/
    }

    public void TriggerShoot()
    {
        if(loadoutDataArray.Count > 0)
            Shootv2(loadoutDataArray[currentWeapon].weaponData);
    }

    void Shootv2(WeaponData weaponData)
    {
        if (loadoutDataArray[currentWeapon].ammo < 1)
        {
            WeaponReload(loadoutDataArray[currentWeapon].weaponData);
        }
        loadoutDataArray[currentWeapon].ammo--;
        shootTime = weaponData.fireRate;

        

        if (weaponData.weaponType == WeaponData._weaponType.Pistol ||
            weaponData.weaponType == WeaponData._weaponType.Shotgun)
        {
            var Bullet = Instantiate(weaponData.projectile, firePointGO.position, firePointGO.rotation);
            Rigidbody[] rb = Bullet.GetComponentsInChildren<Rigidbody>();
            foreach (var VARIABLE in rb)
            {
                VARIABLE.AddForce(firePointGO.forward * weaponData.bulletSpeed, ForceMode.Impulse);
                VARIABLE.GetComponent<PlayerProjectile>().damageAmount = weaponData.damage;
            }
        }

        else if (weaponData.weaponType == WeaponData._weaponType.AssaultRifle)
        {
            playerLaserScript.damageAmount = weaponData.damage;
            laserProjectile.gameObject.SetActive(true);
            StartCoroutine(StopLaser());
        }

            
        _animator.SetTrigger("Shoot"); //Play Animation
        
    }

    IEnumerator StopLaser()
    {
        yield return new WaitForSeconds(3.5f);

        laserProjectile.SetActive(false);
    }

    void ShootHandgun(WeaponData weaponData)
    {
        
    }

    void ShootShotgun(WeaponData weaponData)
    {
        
    }

    void ShootAR(WeaponData weaponData)
    {
        RaycastHit[] hit = Physics.RaycastAll(shootRay, range, shootableMask);

        foreach (var item in hit)
        {
            //Lakukan raycast hit component Enemyhealth
            EnemyHealth enemyHealth = item.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                //Lakukan Take Damage
                enemyHealth.TakeDamage(weaponData.damage);
                //enemyHealth.GetComponent<EnemyMovement>().anim.SetTrigger("damage");
                enemyHealth.GetComponent<EnemyMovement>().DisableNav();
            }
        }
    }

    void WeaponReload(WeaponData weaponData)
    {
        if (!weaponData.infMagazine)
        {
            if (loadoutDataArray[currentWeapon].magazine >= loadoutDataArray[currentWeapon].weaponData.ammoCapacity)
            {
                loadoutDataArray[currentWeapon].magazine -= loadoutDataArray[currentWeapon].weaponData.ammoCapacity - loadoutDataArray[currentWeapon].ammo;
                loadoutDataArray[currentWeapon].ammo = loadoutDataArray[currentWeapon].weaponData.ammoCapacity;
            }

            else
            {
                loadoutDataArray[currentWeapon].ammo += loadoutDataArray[currentWeapon].magazine;
                loadoutDataArray[currentWeapon].magazine = 0;
            }
        }

        else
        {
            if (loadoutDataArray[currentWeapon].magazine > 0)
            {
                loadoutDataArray[currentWeapon].ammo = loadoutDataArray[currentWeapon].weaponData.ammoCapacity;
            }
        }
    }

    public void WeaponSwitch(int _currentWeapon)
    {
        if (loadoutDataArray.Count > 0)
        {

            /*if (Keyboard.current.digit1Key.wasPressedThisFrame && loadoutDataArray.ElementAtOrDefault(0) != null)
            {
                currentWeapon = 0;
            }
            else if (Keyboard.current.digit2Key.wasPressedThisFrame && loadoutDataArray.ElementAtOrDefault(1) != null)
            {
                currentWeapon = 1;
            }
            else if (Keyboard.current.digit3Key.wasPressedThisFrame && loadoutDataArray.ElementAtOrDefault(2) != null)
            {
                currentWeapon = 2;
            }
            else if (Keyboard.current.digit4Key.wasPressedThisFrame && loadoutDataArray.ElementAtOrDefault(3) != null)
            {
                currentWeapon = 3;
            }*/

            currentWeapon = _currentWeapon;

            //InGameMenuController.Instance.UpdateAmmoInfo();
            WeaponUpdateData();
        }
    }

    public void WeaponUpdateData()
    {
        WeaponData weapon = loadoutDataArray[currentWeapon].weaponData;
        _animator.SetFloat("ShootSpeed", weapon.fireRate);


        //shootTime = _weaponList[currentWeapon].fireRate;
        UIManager.Instance.SwitchGunSprite(weapon.weaponImage);
        range = weapon.range;
    }

    public void OnDrawGizmos()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(ray.origin, ray.direction * Distance, Color.red);

        Debug.DrawRay(transform.position + Vector3.up * 1.3f, transform.forward * range, Color.blue);

        /*//Gizmos.DrawWireSphere(transform.position, range);
        if (loadoutDataArray.Count > 0)
        {
            if (loadoutDataArray[currentWeapon].weaponData == null) return;

            //if (loadoutDataArray[currentWeapon].weaponData.weaponType == WeaponData._weaponType.Shotgun)
            //    Gizmos.DrawWireCube(firePointGO.transform.position + (transform.forward * 2f), transform.localScale * range);
        }*/
            
    }
}