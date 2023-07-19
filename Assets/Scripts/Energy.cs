using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float maxEnergy;
    [SerializeField] public Vector3 respawnPosition;
    [SerializeField] public LayerMask damageSources;

    [Header("States")]
    [SerializeField]  float currentEnergy;
    [SerializeField]  bool hasFuse;

    [Header("References")]
    private new Transform transform;
    private GameObject fuseSprite;

    public float MaxEnergy { get {return maxEnergy; } }
    public float CurrentEnergy { get { return currentEnergy; } }
    public bool HasFuse { get { return hasFuse; } set { hasFuse = value; } }

    public delegate void EnergyAmountUpdate();
    public event EnergyAmountUpdate updateEnergy;

    void Awake()
    {
        currentEnergy = maxEnergy;
        fuseSprite = GameObject.Find("Canvas").transform.Find("Fuse").gameObject;
    }

    void Start()
    {
        transform = GetComponent<Transform>();
        respawnPosition = transform.position;
    }

    void Update()
    {
        if (hasFuse)
            fuseSprite.SetActive(true);
        else
            fuseSprite.SetActive(false);
        if(Damaged()) {
            UseEnergy(50*Time.deltaTime);
        }
        if(currentEnergy == 0) {
            transform.position = respawnPosition;
            Charge(maxEnergy);
        }
    }

    private bool Damaged()
    {
        return Physics.CheckCapsule(transform.position + Vector3.up*.5f, transform.position + Vector3.down*.6f, .6f, damageSources);
    }

    public void UseEnergy(float amount)
    {
        currentEnergy -= amount;

        if (currentEnergy < 0)
            currentEnergy = 0;

        updateEnergy?.Invoke();
    }

    public void Charge(float amount)
    {
        currentEnergy += amount;

        if (currentEnergy > maxEnergy)
            currentEnergy = maxEnergy;

        updateEnergy?.Invoke();
    }

    public bool HasEnoughEnergy(float amount)
    {
        return amount <= currentEnergy;
    }
}
