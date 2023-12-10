using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KuroRanged : MonoBehaviour
{
    public float spawnInterval;
    float spawnTime;

    public ProjectilesForward projectile;
    public List<ProjectilesForward> projectiles = new List<ProjectilesForward>();
    public float projectileSpeed = 1f;
    public int maxOrb = 1;
    public float targetDistance = 0.3f;

    public int spawnCount = 15;
    public float spawnRotation = 6.29f;

    public bool enableDebug = false;

    private void OnDisable()
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
            SpawnProjectile(spawnCount);
        }

        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles[i] != null)
            {
                projectiles[i].projectileSpeed = projectileSpeed;
                projectiles[i].targetDistance = targetDistance;
                if (!projectiles[i].GetComponent<ProjectilesForward>().canMove)
                    projectiles[i].SetPosition();
            }
            else
                projectiles.Remove(projectiles[i]);
            
        }
    }

    public void Launch()
    {
        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            ProjectilesForward prj = projectiles[i].GetComponent<ProjectilesForward>();
            prj.canMove = true;
            if (prj.canMove)
            {
                projectiles.RemoveAt(i);
            }
        }
    }

    // Spawn Projectiles Simultaneously
    public void SpawnProjectile(int spawnCount)
    {
        spawnRotation += 15f;
        for (int i = projectiles.Count; i < spawnCount; i++)
        {
            Vector3 targetPos = transform.position + Vector3.forward;
            var prj = Instantiate(projectile, targetPos, Quaternion.identity, transform.parent);
            prj.center = transform;
            projectiles.Add(prj);

            if (projectiles.Count > 0)
            {
                int j = 0;
                foreach (var item in projectiles)
                {

                    item.angle = 6.29f / projectiles.Count * j + spawnRotation;
                    j++;
                }
            }
        }

        Invoke("Launch",0.2f);
    }

    

    private void OnDrawGizmos()
    {
        if (enableDebug)
        {
            Debug.DrawRay(transform.position, transform.right * 2f);

            for (int i = 0; i < spawnCount; i++)
            {
                float angle = 6.29f / spawnCount * i + spawnRotation;
                Vector3 offset = new Vector3(Mathf.Cos(angle) * targetDistance, 0, Mathf.Sin(angle) * targetDistance) * targetDistance;
                Vector3 targetPos = transform.position + offset;

                Gizmos.DrawWireSphere(targetPos, 0.2f);
            }

            /* Recursive
            for (int i = 0; i < spawnCount; i++)
            {
                float angle = circleLength / spawnCount * i;
                Vector3 offset = new Vector3(Mathf.Sin(angle) * targetDistance, Mathf.Cos(angle) * targetDistance, 0) * targetDistance;
                Vector3 targetPos = transform.position + offset * i;

                Gizmos.DrawWireSphere(targetPos, 0.2f);
            }*/
        }
    }
}

#region Testing Code
/* Testing Code
  [ContextMenu("Launch")]
//Launch Projectiles Simultaneously

[ContextMenu("Launch2")]
public void DebugLaunch2()
{
    StartCoroutine(Launch2());
}

[ContextMenu("Spawn1")]
public void DebugSpawn1()
{
    SpawnProjectile(spawnCount);
}

[ContextMenu("Spawn2")]
public void DebugSpawn2()
{
    StartCoroutine(Spawn2());
}

//spawn Projectiles Recursively every 0.1f seconds
public IEnumerator Spawn2()
{
    int j = 0;
    for (int i = projectiles.Count; i < spawnCount; i++)
    {
        yield return new WaitForSeconds(0.1f);
        var prj = Instantiate(projectile, transform);
        prj.center = transform;
        prj.transform.position = transform.position + Vector3.forward;
        prj.angle = circleLength / spawnCount * j;
        j++;

        projectiles.Add(prj);
    }
    //Launch Projectiles
    StartCoroutine(Launch2());
}

[ContextMenu("Spawn3")]
public void Spawn3()
{
    int j = 0;
    for (int i = projectiles.Count; i < spawnCount; i++)
    {
        var prj = Instantiate(projectile, transform);
        prj.center = transform;
        prj.angle = circleLength / spawnCount * j;
        j++;

        projectiles.Add(prj);
    }
    StartCoroutine(Launch2());
}

public IEnumerator Launch2()
{
    for (int i = 0; i < projectiles.Count;)
    {
        var unit = projectiles[i].GetComponent<ProjectilesForward>();
        yield return new WaitForSeconds(0.1f);
        unit.canMove = true;

        if (unit.canMove)
        {
            projectiles.RemoveAt(i);
        }
        else
        {
            i++;
        }
    }
}

public void ListChecker()
    {
        if (projectiles.Count < maxOrb)
        {
            var prj = Instantiate(projectile,transform);
            prj.center = transform;
            prj.transform.position = transform.position + Vector3.forward;
            projectiles.Add(prj);

            if (projectiles.Count > 0)
            {
                int i = 0;
                foreach (var item in projectiles)
                {
                    
                    item.angle = 6.29f / projectiles.Count * i;
                    i++;
                }
            }
        }

        else if( projectiles.Count > maxOrb)
        {
            var prj = projectiles[projectiles.Count - 1];
            projectiles.Remove(prj);
            Destroy(prj.gameObject);

            if (projectiles.Count > 0)
            {
                int i = 0;
                foreach (var item in projectiles)
                {

                    item.angle = 6.29f / projectiles.Count * i;
                    i++;
                }
            }
        }
    }
*/
#endregion
