using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    public int damageAmount = 5;
    private void OnTriggerStay(Collider other)
    {
        DamageContinuous(other);
    }

    public void DamageContinuous(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() != null)
        {
            //Debug.Log("Player Take Damage");
            other.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
        }
    }

    public void DisableGO()
    {
        //transform.parent.GetComponent<KuroLaser>().scanTime = transform.parent.GetComponent<KuroLaser>().scanInterval;
        gameObject.SetActive(false);
    }
}
