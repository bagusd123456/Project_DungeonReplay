using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KuroAOE : MonoBehaviour
{
    public float spawnInterval;
    float spawnTime;
    public float damageCountdown;

    public float targetDistance = 3;
    public int spawnCount;

    public PlayerHealth player;
    public GameObject projectile;

    public List<GameObject> targetList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (spawnTime > 0)
        //    spawnTime -= Time.deltaTime;
        //else
        //{
        //    spawnTime = spawnInterval;
        //    StartCoroutine(SpawnOnPlayer2());
        //}


        //for (int i = 0; i < targetList.Count; i++)
        //{
        //    if (targetList[i] != null)
        //    {
        //        if (targetList[i] == null)
        //        {
        //            //targetList[i].gameObject.SetActive(false);
        //            targetList.RemoveAt(i);
        //        }
        //    }
        //    else
        //    {
        //        i++;
        //    }
        //}
    }

    [ContextMenu("Spawn")]
    public void SpawnTarget()
    {
        int j = 0;
        for (int i = targetList.Count; i < spawnCount; i++)
        {
            var prj = Instantiate(projectile, transform);
            prj.SetActive(true);
            targetList.Add(prj);
        }
    }

    //Debug
    public void SpawnOnPlayer()
    {
        var prj = Instantiate(projectile, transform);

        targetList.Add(prj);
    }

    public IEnumerator SpawnOnPlayer2()
    {
        for (int i = targetList.Count; i < spawnCount; i++)
        {
            yield return new WaitForSeconds(0.3f);
            var prj = Instantiate(projectile, transform);
            prj.transform.position = player.transform.position;
            //prj.angle = player.currentAngle;
            //prj.damageCountdown = damageCountdown;

            targetList.Add(prj);
        }
        
    }
}
