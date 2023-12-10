using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LaserDamage : MonoBehaviour
{
    public int damageAmount = 5;

    public float currentDamageTime = 0f;
    public float damageInterval = 1.5f;
    private void OnTriggerStay(Collider other)
    {
        DamageContinuous(other);
    }

    private void Update()
    {
        if (currentDamageTime > 0)
        {
            currentDamageTime -= Time.deltaTime;
        }

        else
        {
            currentDamageTime = 0;
        }
    }

    public void DamageContinuous(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() != null && currentDamageTime <= 0)
        {
            currentDamageTime = damageInterval;
            //Debug.Log("Player Take Damage");
            other.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
        }
    }

    public void DisableGO()
    {
        //transform.parent.GetComponent<KuroLaser>().scanTime = transform.parent.GetComponent<KuroLaser>().scanInterval;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
