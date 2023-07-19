using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredDoor : MonoBehaviour
{
    [Header("References")]
    private Animator animator;
    protected Charger charger;
    private AudioSource sfx;

    // Variables
    private bool wasOn;
    private bool isOn;
    private bool hasPlayedSfx;

    void Awake()
    {
        animator = GetComponent<Animator>();
        charger = GetComponent<Charger>();
        sfx = GetComponent<AudioSource>();
        isOn = charger.powered;
    }

    void Start()
    {
        if(isOn) {
            animator.Play("Open");
        } else {
            animator.Play("Close");
        }
    }

    void Update()
    {
        wasOn = isOn;
        isOn = charger.powered;
        if(wasOn != isOn) {
            if(isOn) {
                
                animator.Play("Open");
            } else {
                animator.Play("Close");
            }
            sfx.Play();
        }
    }
}
