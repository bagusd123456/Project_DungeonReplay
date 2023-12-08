using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomRandomizer : MonoBehaviour
{
    public GameObject tempRoom;
    public List<RoomProbability> roomPrefabList = new List<RoomProbability>();

    public void Start()
    {
        RandomSpawn();
    }

    //Random Spawn One from the list
    [ContextMenu("Random Spawn")]
    public void RandomSpawn()
    {
        tempRoom.SetActive(false);
        //int randomIndex = Random.Range(0, roomPrefabList.Count);

        GameObject targetPrefab = ProbableElement.RandomWeightedElement(roomPrefabList).roomPrefab;

        var GO = Instantiate(targetPrefab, transform);
        GO.transform.position = transform.localPosition;
        tempRoom = GO;
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