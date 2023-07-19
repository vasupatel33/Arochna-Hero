using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredBlock : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 initialRotation;
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private float time;

    [Header("References")]
    private new Transform transform;
    private Charger charger;

    // Variables
    private float timer;
    private bool goingUp;

    void Awake()
    {
        transform = GetComponent<Transform>();
        charger = GetComponent<Charger>();
    }

    void Start()
    {
        timer = 0;
    }

    void Update()
    {
        timer += ((charger.powered ? 1 : -1)*Time.deltaTime)/time;
        timer = Mathf.Clamp01(timer);
        transform.position = Vector3.Lerp(initialPosition, targetPosition, timer);
    }
}
