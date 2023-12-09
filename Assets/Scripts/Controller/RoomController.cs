using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<GameObject> gateList;
    public List<EnemyHealth> enemyHealthList;

    // Start is called before the first frame update
    void Start()
    {
        DetectEnemy();
    }

    [ContextMenu("DetectEnemy")]
    public void DetectEnemy()
    {
        enemyHealthList = GetComponentsInChildren<EnemyHealth>().ToList();
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    var Gate = transform.GetChild(i).CompareTag("WallGate");
        //    if (Gate)
        //    {
        //        gateList.Add(transform.GetChild(i).gameObject);
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemyHealthList.Count; i++)
        {
            if (enemyHealthList[i].isDead || enemyHealthList[i] == null)
                enemyHealthList.Remove(enemyHealthList[i]);
        }

        if (enemyHealthList.Count == 0)
        {
            UnlockAllGate();
        }
    }

    public void UnlockAllGate()
    {
        foreach (var gate in gateList)
        {
            gate.SetActive(false);
        }
    }

    public void LockAllGate()
    {
        foreach (var gate in gateList)
        {
            gate.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.CompareTag("Player"))
        //    LockAllGate();
    }
}
