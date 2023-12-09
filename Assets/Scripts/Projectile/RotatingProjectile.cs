using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingProjectile : MonoBehaviour
{
    public Transform rotateAround;
    public float projectileSpeed = 5f;
    public float targetDistance = 3;
    public float offsetPositionY = 1f;
    public float angle;
    Vector3 offset;

    void Update()
    {
        
    }

    public void SetPosition()
    {
        offset = new Vector3(Mathf.Sin(angle) * targetDistance, Mathf.Cos(angle) * targetDistance, 0) * targetDistance;
        transform.position = rotateAround.position + offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
