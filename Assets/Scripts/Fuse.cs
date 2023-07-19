using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuse : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float distance;

    [Header("States")]
    private float startY;
    private float currentTime;

    [Header("References")]
    private Energy energy;

    void Awake()
    {
        startY = transform.position.y;
        energy = GameObject.Find("Player").GetComponent<Energy>();
    }

    void Update()
    {
        float y = (0.5f * Mathf.Sin(speed * Mathf.PI * (currentTime - 0.5f)) + 0.5f) * distance;
        currentTime += Time.deltaTime;

        transform.position = new Vector3(transform.position.x, startY + y, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !energy.HasFuse)
        {
            energy.HasFuse = true;
            Destroy(this.gameObject);
        }
    }
}
