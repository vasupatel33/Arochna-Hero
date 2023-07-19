using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : MonoBehaviour
{
    [Header("Attributes")]
    private new Animation animation;
    public bool unGrappleable;
    [SerializeField] private float rechargeRate;
    [SerializeField] private float timeToOn;
    [SerializeField] private bool isCheckpoint;
    [SerializeField] private Vector3 respawnPosition;
    [SerializeField] private bool playSound;

    [Header("States")]
    public bool hasPower;
    public bool powered;
    private bool playerInRadius;
    private bool hasSparked;

    [Header("References")]
    [SerializeField] private List<GameObject> parentChargers;
    private ParticleSystem sparks;
    private Energy energy;
    private AudioSource lightOn;

    // Variables
    private float timer;
    private bool hasPlayedSound;

    public bool CanGrapple()
    {
        return powered && !unGrappleable;
    }

    void Awake()
    {
        energy = GameObject.Find("Player").GetComponent<Energy>();
        sparks = GetComponentInChildren<ParticleSystem>();
        if(GetComponent<Animation>()) {animation = GetComponent<Animation>();}
        lightOn = GetComponent<AudioSource>();
        timer = 0;
    }

    void Start()
    {
        if (powered)
            Debug.Log(powered);

        if (lightOn != null && (powered || (hasPower && parentChargers.Count < 1)))
            hasPlayedSound = true;

        timer = 0;
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRadius = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRadius = false;
        }
    }

    void Update()
    {
        if (playerInRadius && powered)
        {
            energy.Charge(rechargeRate * Time.deltaTime);
            if(isCheckpoint)
                energy.respawnPosition = respawnPosition;
        }

        if (lightOn != null) 
        {
            if (powered && !lightOn.isPlaying && !hasPlayedSound && playSound)
            {
                lightOn.Play();
                hasPlayedSound = true;
            }
            else if (!powered && !(hasPower && parentChargers.Count < 1) && playSound)
            {
                lightOn.Stop();
                hasPlayedSound = false;
            }
        }

        if(parentChargers.Count == 0) {
            bool wasPowered = powered;
            timer += (hasPower ? 1 : -1)*Time.deltaTime;
            timer = Mathf.Clamp(timer, 0, timeToOn);
            powered = hasPower && timer >= timeToOn;
            if(powered) {
                if(sparks != null && !hasSparked) {
                    hasSparked = true;
                    sparks.Play();
                }
            } else {
                if (sparks != null)
                    hasSparked = false;
            }
        } else if(parentChargers.Count == 1) {
            bool wasPowered = powered;
            bool ready = parentChargers[0].GetComponent<Charger>().powered && hasPower;
            timer += (ready ? 1 : -1)*Time.deltaTime;
            timer = Mathf.Clamp(timer, 0, timeToOn);
            powered = ready && timer >= timeToOn;
            if(ready && !powered) 
            {
                if (animation != null)
                    animation?.Play("Start");
            }
            if(powered) {
                if(animation != null) {
                    animation.Play("Active");
                }
                if (sparks != null && !hasSparked)
                {
                    hasSparked = true;
                    sparks.Play();
                }
            }
            if(wasPowered != powered) 
            {
                if (animation != null)
                {
                    animation.Stop(); 
                    animation.Play("Stop");
                }
                if (sparks != null)
                    hasSparked = false;
            }
        } else if(parentChargers.Count > 1) {
            bool wasPowered = powered;
            bool allPowered = true;
            foreach(GameObject parent in parentChargers) {
                if(!parent.GetComponent<Charger>().powered) {allPowered = false;}
            }
            bool ready = allPowered && hasPower;
            timer += (ready ? 1 : -1)*Time.deltaTime;
            timer = Mathf.Clamp(timer, 0, timeToOn);
            powered = ready && timer >= timeToOn;
            if(ready && !powered) 
            {
                if (animation != null)
                    animation.Play("Start");
            }
            if(powered) {
                if(animation != null)
                    animation.Play("Active");
                if (sparks != null && !hasSparked)
                {
                    sparks.Play();
                    hasSparked = true;
                }
            }
            if(wasPowered != powered) 
            {
                if (animation != null)
                {
                    animation.Stop(); 
                    animation.Play("Stop");
                }
                if (sparks != null)
                    hasSparked = false;
            }
        }
    }
}
