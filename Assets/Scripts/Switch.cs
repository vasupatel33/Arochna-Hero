using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [Header("References")]
    private Animator animator;
    private Charger charger;
    private new Transform camera;
    private AudioSource sfx;

    [Header("Properties")]
    [SerializeField] private float timeToFlip;
    public bool canUse { get; private set; }
    public bool active { get; private set; }

    // Variables
    private float timer;
    private bool hasPlayed;

    void Awake()
    {
        charger = GetComponent<Charger>();
        sfx = GetComponent<AudioSource>();
        camera = Camera.main.transform;
        Interact.OnUse += UseSwitch;
        charger.hasPower = false;
    }

    void OnDestroy()
    {
        Interact.OnUse -= UseSwitch;
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    void UseSwitch(GameObject obj)
    {
        if(obj == gameObject && timer > timeToFlip) {
            timer = 0;
            active = !active;
            if(active) {
                GetComponent<Animation>().Play("SwitchOn");
                sfx.Play();
            } else {
                GetComponent<Animation>().Play("SwitchOff");
                sfx.Play();

            }
            charger.hasPower = active;
        }
    }
}
