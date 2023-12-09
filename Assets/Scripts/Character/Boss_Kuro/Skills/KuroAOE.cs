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
    public AOEDamage projectile;

    public List<AOEDamage> targetList = new List<AOEDamage>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTime > 0)
            spawnTime -= Time.deltaTime;
        else
        {
            spawnTime = spawnInterval;
            StartCoroutine(SpawnOnPlayer2());
        }

        foreach (var item in targetList)
        {
            item.targetDistance = targetDistance;
            item.SetPosition();
            if (item.isDestroyed)
            {
                item.enabled = false;
            }
        }

        for (int i = 0; i < targetList.Count; i++)
        {
            if (targetList[i] != null)
            {
                if (targetList[i].isDestroyed)
                {
                    targetList[i].gameObject.SetActive(false);
                    targetList.RemoveAt(i);
                }
            }
            else
            {
                i++;
            }
        }
    }

    [ContextMenu("Spawn")]
    public void SpawnTarget()
    {
        int j = 0;
        for (int i = targetList.Count; i < spawnCount; i++)
        {
            var prj = Instantiate(projectile, transform);
            prj.rotateAround = transform;
            prj.angle = Random.Range(0f,7f) / spawnCount * j;
            prj.damageCountdown = damageCountdown;
            j++;

            targetList.Add(prj);
        }
    }

    //Debug
    public void SpawnOnPlayer()
    {
        var prj = Instantiate(projectile, transform);
        prj.rotateAround = transform;
        //prj.angle = player.transform.position;
        prj.damageCountdown = damageCountdown;

        targetList.Add(prj);
    }

    public IEnumerator SpawnOnPlayer2()
    {
        for (int i = targetList.Count; i < spawnCount; i++)
        {
            yield return new WaitForSeconds(0.3f);
            var prj = Instantiate(projectile, transform);
            prj.rotateAround = transform;
            //prj.angle = player.currentAngle;
            prj.damageCountdown = damageCountdown;

            targetList.Add(prj);
        }
        
    }
}
