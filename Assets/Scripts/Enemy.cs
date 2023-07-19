using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    chasing,
    attacking
}

public class Enemy : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float health;
    [SerializeField] private float damageFlashTime;
    [SerializeField] private float attackTime;
    [SerializeField] private float chaseDistance;
    [SerializeField] private float memoryDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float FuseHeight;
    [SerializeField] private bool dropsFuse;
    [SerializeField] private LayerMask floorMask;

    [Header("States")]
    private EnemyState state;
    private float currentAttackTime;
    private bool canSee;

    [Header("References")]
    [SerializeField] private GameObject fuse;
    [SerializeField] private GameObject shockwave;
    private Material material;
    private new Rigidbody rigidbody;
    private Transform player;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").transform;

        material = GetComponent<MeshRenderer>().material;

        state = EnemyState.idle;
        
        currentAttackTime = attackTime;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        StartCoroutine(DamageIndicator());
        
        if (health <= 0)
            Die();
    }

    public void Die()
    {
        if (dropsFuse)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit, floorMask);
            Vector3 pos = hit.point;
            pos.y += FuseHeight;
            Instantiate(fuse, pos, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    void Update()
    {
        state = GetEnemyState();

        switch(state)
        {
            case (EnemyState.attacking):
            {
                if (currentAttackTime >= attackTime)
                {
                    currentAttackTime = 0;
                    Instantiate(shockwave, transform.position, Quaternion.identity);
                }
                break;
            }
            case (EnemyState.chasing):
            {
                transform.Translate((player.position - transform.position).normalized * speed * Time.deltaTime);
                break;
            }
        }

        currentAttackTime += Time.deltaTime;
    }
    
    private EnemyState GetEnemyState()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackDistance)
            return EnemyState.attacking;
        else if (dist <= chaseDistance || (canSee && dist <= memoryDistance))
        {
            canSee = true;
            return EnemyState.chasing;
        }
        
        return EnemyState.idle;
    }
    
    private void LookAt(Vector3 pos)
    {
        Vector3 newPos = new Vector3(pos.x, transform.position.y, pos.z);
        Vector3 dir = newPos - transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }

    private IEnumerator DamageIndicator()
    {
        Color highlight = material.GetColor("_Highlight");
        material.SetColor("_Highlight", Color.black);

        yield return new WaitForSeconds(damageFlashTime);

        material.SetColor("_Highlight", highlight);
    }
}
