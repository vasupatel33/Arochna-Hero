using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private bool startWithFuse;
    private Animator animator;
    private Energy energy;
    private Charger charger;
    public GameObject fuse { get; private set; }
    //private GameObject red;
    //private GameObject green;

    void Awake()
    {
        animator = GetComponent<Animator>();
        energy = GameObject.Find("Player").GetComponent<Energy>();
        charger = GetComponent<Charger>();
        fuse = transform.Find("Fuse").gameObject;
        Interact.OnUse += UseBox;

        //red = GameObject.Find("RedLight");
        //green = GameObject.Find("GreenLight");
    }

    void OnDestroy()
    {
        Interact.OnUse -= UseBox;
    }

    void Start()
    {
        fuse.SetActive(startWithFuse);
        //green.SetActive(false);
    }

    void UseBox(GameObject obj)
    {
        if(obj == gameObject) {
            if(energy.HasFuse && !fuse.activeSelf) {
                fuse.SetActive(true);
                //red.SetActive(false);
                //green.SetActive(true);
                charger.hasPower = true;
                energy.HasFuse = false;
            } else if(!energy.HasFuse && fuse.activeSelf) {
                fuse.SetActive(false);
                //red.SetActive(true);
                //green.SetActive(false);
                charger.hasPower = false;
                energy.HasFuse = true;
            }
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.Play("Open");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.Play("Close");
        }
    }
}
