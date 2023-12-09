using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Kuro : MonoBehaviour
{
    public PlayerHealth player;

    public enum state { IDLE, PATROL, HOSTILE, DASHING, ATTACKING}
    public state _state = state.IDLE;

    float attackTime;
    public float attackCooldown = 0.8f;

    [Header("Attack Behavior")]
    public int activeSkillIndex;
    public List<MonoBehaviour> availableSkill = new List<MonoBehaviour>();

    private void OnValidate()
    {
        
    }
    private void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCondition>();
        player = FindObjectOfType<PlayerHealth>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTime > 0)
            attackTime -= Time.deltaTime;

        ExecuteSkill();
    }

    public void ExecuteSkill()
    {
        for (int i = 0; i < availableSkill.Count; i++)
        {
            if (availableSkill[i] == availableSkill[activeSkillIndex])
                availableSkill[i].enabled = true;
            else
                availableSkill[i].enabled = false;
        }
    }

    public void Movement()
    {
        switch (_state)
        {
            case state.IDLE:
                
                break;

            case state.PATROL:
                
                break;

            case state.HOSTILE:
                
                break;

            case state.DASHING:
                
                break;

            case state.ATTACKING:
                
                BasicAttack();
                break;
        }
    }

    public void BasicAttack()
    {
        if (attackTime <= 0)
        {
            
        }
    }

    float CurrentAngle()
    {
        Vector3 dir = transform.parent.position - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 180f;
        return angle;
    }

    Vector3 GetPosition(float degrees, float dist)
    {
        float a = degrees * Mathf.PI / 180f;
        return new Vector3(Mathf.Sin(a) * dist, Mathf.Cos(a) * dist, 0);
    }
}
