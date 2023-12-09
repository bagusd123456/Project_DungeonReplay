using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamage : MonoBehaviour
{
    public Transform rotateAround;
    public float targetDistance = 3;
    public float offsetPositionY = 1f;
    public float angle;
    public float radius = .5f;
    public int damageAmount;
    Vector3 offset;

    public float damageCountdown;
    public bool isDestroyed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (damageCountdown > 0)
            damageCountdown -= Time.deltaTime;
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            Invoke("OnTimeout", 0.005f);
        }
    }

    public void GiveDamage()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Character"));
        if (hit.Length > 0)
            hit[0].GetComponent<PlayerHealth>().TakeDamage(damageAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        //Damage every Character Collide
        if (other.GetComponent<PlayerCondition>() != null)
        {
            other.GetComponent<PlayerCondition>().TakeDamage(damageAmount);
        }*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void OnTimeout()
    {
        GiveDamage();
        isDestroyed = true;
    }

    public void SetPosition()
    {
        Vector3 targetPos = GetPosition(angle, targetDistance);
        transform.position = transform.parent.position + targetPos + offset;
    }

    //Get Next Position Based on Angle & Distance
    Vector3 GetPosition(float degrees, float dist)
    {
        float a = degrees * Mathf.PI / 180f;
        return new Vector3(Mathf.Sin(a) * dist, Mathf.Cos(a) * dist, 0);
    }
}
