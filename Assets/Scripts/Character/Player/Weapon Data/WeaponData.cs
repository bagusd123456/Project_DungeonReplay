using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Data")]
public class WeaponData : ScriptableObject
{
    public enum _weaponType { Pistol, AssaultRifle, Shotgun }

    [Header("Weapon Properties")]
    public string weaponName;
    public Sprite weaponImage;

    [Header("Weapon Parameter")]
    public _weaponType weaponType = _weaponType.Pistol;
    public float range;
    public int damage;
    public int ammoCapacity;
    public int magazineCapacity;
    public bool infMagazine;
    public float fireRate;
    public float reloadSpeed;

    [Header("Weapon Projectile")]
    public GameObject projectile;
    public float bulletSpeed = 70f;

    [Header("Weapon Camera Shake")]
    public bool shakeActive;


}
