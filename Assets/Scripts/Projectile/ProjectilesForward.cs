using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesForward : MonoBehaviour
{
    PlayerHealth player;
    [HideInInspector]
    public Transform center;
    Vector3 offset;
    public float targetDistance = 3;
    [HideInInspector]
    public float angle;
    public float projectileSpeed = 1f;
    [Space]
    public int damageAmount;

    public bool canMove = false;

    public Transform caster;

    public Vector3 rotation;
    private void Awake()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        center = transform.parent;

        StartCoroutine(EnableCollider());
    }

    private void Start()
    {
        LookAtTarget();
    }

    // Update is called once per frame
    void Update()
    {
        LookAtTarget();
        if(canMove)
            transform.position += transform.forward * projectileSpeed * Time.deltaTime;

    }

    public void DisableGO()
    {
        gameObject.SetActive(false);
    }

    public void LookAtTarget()
    {
        Vector2 lookDir = center.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(rotation);
    }

    public void SetPosition()
    {
        offset = new Vector3(Mathf.Cos(angle) * targetDistance, .6f, Mathf.Sin(angle) * targetDistance) * targetDistance;
        transform.position = center.position + offset;
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.05f);
        //GetComponent<SphereCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform != center && other.GetComponent<ProjectilesForward>() == null)
        {
            //Damage every Character Collide
            if (other.GetComponent<PlayerHealth>() != null)
            {
                other.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
            }
            Destroy(this.gameObject);
        }
        
    }
}
