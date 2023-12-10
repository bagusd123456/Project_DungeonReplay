using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEntity : MonoBehaviour,ICollectible
{
    public LoadoutData currentData;
    public WeaponData weaponDataSO;

    public bool canCollect;
    private void Awake()
    {
        currentData.weaponData = weaponDataSO;
        currentData.InitData();
    }

    [ContextMenu("Collect")]
    public void Collect()
    {
        if (PlayerShooting.Instance.loadoutDataArray.Count <= 2)
        {
            var currentWeapon = PlayerShooting.Instance.loadoutDataArray[PlayerShooting.Instance.currentWeapon];
            currentWeapon = currentData;

            PlayerShooting.Instance.loadoutDataArray.Add(currentWeapon);
            PlayerShooting.Instance.WeaponSwitch(1);
            Destroy(gameObject);

        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canCollect = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canCollect = false;
        }
    }
}
