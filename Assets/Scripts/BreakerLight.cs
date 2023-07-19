using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakerLight : PoweredLight
{
    [SerializeField] private Breaker breaker;

    protected override void ManageLight()
    {
        if(breaker.fuse.activeSelf) {
            GetComponent<Renderer>().material = onMaterial;

        } else {
            GetComponent<Renderer>().material = offMaterial;
        }
    }
}
