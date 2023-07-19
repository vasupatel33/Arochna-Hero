using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    /*
        THIS IS A GRAPPLE ABILITY. IT USES UNITY'S BUILT IN PHYSICS ENGINE TO SIMULATE
        PHYSICS. 
    */
    
    [Header("Attributes")]
    [SerializeField] private Vector3 handPosition;
    [SerializeField] private float maxGrappleDist;
    [SerializeField] private float grappleStrength;
    [SerializeField] private float grappleShrink;
    [SerializeField] private float idealModifier;
    [SerializeField] private float energyUsed;
    [SerializeField] private LayerMask grappleMask;

    [Header("References")]
    [SerializeField] private ParticleSystem particles;
    public bool grappleStopped { get; private set; }
    private new Rigidbody rigidbody;
    private new Transform camera;
    private Energy energy;
    private LineRenderer line;

    // Variables
    private RaycastHit grappleHit;
    private Vector3 grapplePoint;
    private float idealLength;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        camera = Camera.main.transform;
        energy = GetComponent<Energy>();
        line = GetComponent<LineRenderer>();
        particles = GetComponent<ParticleSystem>();

        grapplePoint = Vector3.zero;
    }

    void Update()
    {
        StartStopGrapple();
        GrappleLine();
        // Use energy
        if(grapplePoint != Vector3.zero) {energy.UseEnergy(energyUsed * Time.deltaTime);}

        //for (int i = 0; i < line.positionCount; i++)
        //{
            //Vector3 pointPosition = line.GetPosition(i);
            //particles.transform.position = pointPosition;
            //particles.Emit(1);
        //}
    }

    void FixedUpdate()
    {
        Grapple();
    }

    private void StartStopGrapple()
    {
        grappleStopped = false;
        // Start
        if(Input.GetMouseButtonDown(1) && grapplePoint == Vector3.zero) {
            if(Physics.Raycast(camera.position, camera.forward, out grappleHit, maxGrappleDist, grappleMask, QueryTriggerInteraction.Ignore)) {
                if(grappleHit.transform.gameObject.GetComponent<Charger>() && grappleHit.transform.gameObject.GetComponent<Charger>().CanGrapple()) {
                    grapplePoint = grappleHit.point;
                    idealLength = grappleHit.distance*idealModifier;
                    line.positionCount = 2;
                } else {
                    Debug.Log("Unpowered target!");
                }
            } else {
                Debug.Log("Out of range!");
            }
        }

        // Stop
        if(grapplePoint != Vector3.zero && !(Input.GetMouseButton(1) && energy.HasEnoughEnergy(energyUsed))) {
            grappleStopped = true;
            grapplePoint = Vector3.zero;
            line.positionCount = 0;
        }
    }

    private void Grapple()
    {
        if(grapplePoint != Vector3.zero) {
            Vector3 toPoint = grapplePoint - transform.position;
            float currentLength = toPoint.magnitude;
            idealLength -= grappleShrink;
            Vector3 toIdealPoint = toPoint.normalized * (currentLength - idealLength);
            rigidbody.AddForce(toIdealPoint * grappleStrength);
            //if(currentLength < idealLength) {idealLength = currentLength;}
        }
    }

    private void GrappleLine()
    {
        if(grapplePoint != Vector3.zero) {
            line.SetPositions(new Vector3[] {transform.TransformPoint(handPosition), grapplePoint});
        }
    }

    public bool CanGrapple()
    {
        RaycastHit hit;
        if(Physics.Raycast(camera.position, camera.forward, out hit, maxGrappleDist, grappleMask, QueryTriggerInteraction.Ignore))
            if(hit.transform.gameObject.GetComponent<Charger>())
                return hit.transform.gameObject.GetComponent<Charger>().CanGrapple();
        
        return false;
    }

    public bool IsGrappling()
    {
        return grapplePoint != Vector3.zero;
    }
}
