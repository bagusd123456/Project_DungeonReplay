using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserWallController : MonoBehaviour
{
    public List<Collider> gateList;

    public float colliderSize;
    // Start is called before the first frame update
    void Start()
    {
        GetEntranceList();
    }

    private void OnDisable()
    {
        foreach (var gate in gateList)
        {
            gate.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (var gate in gateList)
        //{
        //    gate.gameObject.SetActive(gameObject.activeSelf);
        //}
    }

    [ContextMenu("Get Entrance List")]
    public void GetEntranceList()
    {
        List<Collider> tempGateList = Physics.OverlapBox(transform.position, Vector3.one * colliderSize, Quaternion.identity).ToList();
        for (int i = 0; i < tempGateList.Count; i++)
        {
            if (tempGateList[i].CompareTag("WallGate"))
                gateList.Add(tempGateList[i]);
        }
    }

    private void OnDrawGizmos()
    {
        if (gateList.Count > 0)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }

        Gizmos.DrawWireCube(transform.position, Vector3.one * colliderSize);
    }
}
