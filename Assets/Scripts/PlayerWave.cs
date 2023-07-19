using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWave : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float damage;
    [SerializeField] private float lifetime;

    void Awake()
    {
        StartCoroutine(Die());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);

            Destroy(gameObject);
        }
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(lifetime);

        Destroy(gameObject);
    }
}
