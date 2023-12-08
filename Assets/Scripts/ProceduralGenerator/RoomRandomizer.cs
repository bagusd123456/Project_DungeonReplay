using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRandomizer : MonoBehaviour
{
    public GameObject tempRoom;
    public List<GameObject> roomPrefabList = new List<GameObject>();

    //Random Spawn One from the list
    [ContextMenu("Random Spawn")]
    public void RandomSpawn()
    {
        tempRoom.SetActive(false);
        int randomIndex = Random.Range(0, roomPrefabList.Count);
        var GO = Instantiate(roomPrefabList[randomIndex], transform);
        GO.transform.position = transform.localPosition;
        tempRoom = GO;
    }
}
