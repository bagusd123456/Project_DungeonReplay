using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    [HideInInspector] public float currentAngle;

    //[HideInInspector] public PlayerMovement player;
    [HideInInspector] public Transform center;
    [HideInInspector] public float targetDistance;

    [HideInInspector] public Vector3 offset;
    public float moveSpeed = 1f;
    [HideInInspector] public bool inverseRotation = false;
    [Space]
    public int damageAmount;
    [HideInInspector] public bool canMove;

    public GameObject projectileOnHit;
    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotationSet();
        LookAtTarget();
        Invoke("DestroyGO", 1f);
    }

    public void DestroyGO()
    {
        Destroy(gameObject);
    }

    //Get Next Position Based on Angle & Distance
    public static Vector3 GetPosition(float degrees, float dist)
    {
        float a = degrees * Mathf.PI / 180f;
        return new Vector3(Mathf.Sin(a) * dist, Mathf.Cos(a) * dist, 0);
    }

    public void LookAtTarget()
    {
        Vector2 lookDir = center.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, angle, 2f));
        
    }

    public void RotationSet()
    {
        //Rotate GameObject by Angle
        if (!inverseRotation)
            currentAngle += moveSpeed * Mathf.Rad2Deg * 2f * Time.deltaTime;
        else
            currentAngle += -moveSpeed * Mathf.Rad2Deg * 2f * Time.deltaTime;

        Vector3 targetPos = GetPosition(currentAngle, targetDistance);
        transform.position = center.position + targetPos + offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Damage every Character Collide

        //if (other.GetComponent<PlayerCondition>())
        //{
        //    other.GetComponent<PlayerCondition>().TakeDamage(damageAmount);
        //    if (projectileOnHit != null)
        //        Instantiate(projectileOnHit, transform.position, Quaternion.identity);

        //    Destroy(this.gameObject);
        //}
                
       

        //if(other.GetComponent<NPCCondition>())
        //{
        //    other.GetComponent<NPCCondition>().TakeDamage(damageAmount);
        //    if(projectileOnHit != null)
        //        Instantiate(projectileOnHit, transform.position, Quaternion.identity);

        //    Destroy(this.gameObject);
        //}

    }

}
