using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AI;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomRandomizer : MonoBehaviour
{
    public GameObject tempRoom;
    public List<RoomProbability> roomPrefabList = new List<RoomProbability>();

    public void Start()
    {
        //Delete Children on Start
        ClearSpawn();

        RandomSpawn();
    }

    [ContextMenu("Clear Spawn")]
    public void ClearSpawn()
    {
        int children = transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    //Random Spawn One from the list
    [ContextMenu("Random Spawn")]
    public void RandomSpawn()
    {
        //int randomIndex = Random.Range(0, roomPrefabList.Count);
        if (tempRoom != this.gameObject)
        {
            tempRoom.SetActive(false);
        }
        GameObject targetPrefab = ProbableElement.RandomWeightedElement(roomPrefabList).roomPrefab;

        var GO = Instantiate(targetPrefab, transform);
        GO.transform.position = transform.localPosition;
        tempRoom = GO;

        GenerateNavMesh();
    }

    public void GenerateNavMesh()
    {
        NavMeshBuilder.BuildNavMesh();
    }
}

[Serializable]
public class RoomProbability : ProbableElement
{
    public GameObject roomPrefab;
    public float probability;

    public float Probability
    {
        get => probability;
        set
        {

        }
    }
}