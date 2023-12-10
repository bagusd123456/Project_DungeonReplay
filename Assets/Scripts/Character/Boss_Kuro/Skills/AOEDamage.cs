using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamage : MonoBehaviour
{
    public float radius = .5f;
    public int damageAmount = 5;

    public float currentDamageTime = 0f;
    public float damageInterval = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GiveDamage()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Character"));
        if (hit.Length > 0)
            hit[0].GetComponent<PlayerHealth>().TakeDamage(damageAmount);
    }

    private void OnTriggerStay(Collider other)
    {
        DamageContinuous(other);
    }

    private void Update()
    {
        //if (damageCountdown > 0)
        //    damageCountdown -= Time.deltaTime;
        //else
        //{
        //    Invoke("OnTimeout", 0.005f);
        //}

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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void OnTimeout()
    {
        GiveDamage();
        Destroy(gameObject);
    }
}
