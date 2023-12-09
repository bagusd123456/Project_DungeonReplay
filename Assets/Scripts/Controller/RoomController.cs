using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<GameObject> gateList;
    public List<EnemyHealth> enemyHealthList;

    public float currentTime;

    public float waitTime = 3f;

    public int timesRevealed;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = waitTime;
        DetectEnemy();
        ShowEnemy(false);
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

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            currentTime = 0;
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

    public void ActivateEnemy(bool state = true)
    {
        for (int i = 0; i < enemyHealthList.Count; i++)
        {
            enemyHealthList[i].isReady = state;
        }
    }

    public void ShowEnemy(bool state = true)
    {
        for (int i = 0; i < enemyHealthList.Count; i++)
        {
            enemyHealthList[i].gameObject.SetActive(state);
        }
    }

    public void RevealEnemy()
    {
        timesRevealed++;
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < enemyHealthList.Count; i++)
        {
            //enemyHealthList[i].GetComponent<Rigidbody>().isKinematic = true;
            positionList.Add(enemyHealthList[i].transform.position);
            //var meshSize = enemyHealthList[i].gameObject.GetComponent<MeshRenderer>().bounds.size;
            enemyHealthList[i].gameObject.SetActive(false);
            enemyHealthList[i].transform.Translate(Vector3.down * 5);
            
        }

        for (int j = 0; j < enemyHealthList.Count; j++)
        {
            ShowEnemy();
            enemyHealthList[j].transform.DOMoveY(0, 3f).OnComplete(() =>
            {
                ActivateEnemy();
                //enemyHealthList[j].GetComponent<Rigidbody>().isKinematic = false;
            });
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentTime <= 0 && timesRevealed < 1)
            RevealEnemy();
    }
}
