using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // References
    private new Rigidbody rigidbody;
    private new Collider collider;
    private Transform mainCamera;
    private Transform groundCheck;
    private Grappling grapple;
    private AudioSource walkSfx;
    private AudioSource windSfx;
    private PauseMenu pause;

    // Attributes
    [Header("Moving")]
    [SerializeField] private List<Vector3> startPositions;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float hardMaxVelocity;
    [SerializeField] private float stillFriction;
    [SerializeField] private float movingFriction;
    [SerializeField] private float sfxEpsilon;
    [SerializeField] private float windEpsilon;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float midairControl;
    [SerializeField] private float checkDistance;
    [SerializeField] private LayerMask jumpMask;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        mainCamera = Camera.main.transform;
        groundCheck = transform.Find("Ground Check");
        grapple = GetComponent<Grappling>();
        walkSfx = GetComponent<AudioSource>();
        windSfx = groundCheck.GetComponent<AudioSource>();
        pause = GameObject.Find("Canvas").GetComponent<PauseMenu>();
    }

    void Update()
    {
        Jump();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 move = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal")).normalized;
        Vector3 currentVelocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
        Vector3 idealVelocity = (IsGrounded() ? move * maxSpeed : Vector3.Lerp(currentVelocity, move * maxSpeed, midairControl));
        Vector3 deltaVelocity = idealVelocity - currentVelocity;
        if(move.magnitude == 0 && !grapple.IsGrappling()) {
            collider.material.frictionCombine = PhysicMaterialCombine.Maximum;
            collider.material.dynamicFriction = stillFriction;
        } else {
            collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
            collider.material.dynamicFriction = movingFriction;
        }
        rigidbody.AddForce(deltaVelocity);
        if(rigidbody.velocity.magnitude > hardMaxVelocity) {
            rigidbody.AddForce(-1*Vector3.ClampMagnitude(rigidbody.velocity, hardMaxVelocity));
            //Debug.Log(-1*Vector3.ClampMagnitude(rigidbody.velocity, hardMaxVelocity));
            }
        
        if (IsGrounded())
        {
            windSfx.Stop();
            if (rigidbody.velocity.magnitude >= sfxEpsilon && !walkSfx.isPlaying)
                walkSfx.Play();
            else if (rigidbody.velocity.magnitude < sfxEpsilon || pause.Paused)
                walkSfx.Stop();
        }
        else
        {
            walkSfx.Stop();
            if (rigidbody.velocity.magnitude >= windEpsilon && !windSfx.isPlaying)
                windSfx.Play();
            else if (rigidbody.velocity.magnitude < windEpsilon || pause.Paused)
                windSfx.Stop();
        }

        //if (velNoY.magnitude >= sfxEpsilon && IsGrounded() && !walkSfx.isPlaying)
            //walkSfx.Play();
        //else if (velNoY.magnitude < sfxEpsilon || !IsGrounded() || pause.Paused)
            //walkSfx.Stop();
        
    }

    private void Jump()
    {

        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded()) 
        {
            rigidbody.AddForce(Vector3.up*jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        float scaleX = transform.localScale.x;
        float scaleZ = transform.localScale.z;

        Vector3 halfExtents = new Vector3(scaleX/2.0f, checkDistance, scaleZ/2.0f);
        return Physics.CheckBox(groundCheck.position, halfExtents, Quaternion.identity, jumpMask);
        //return Physics.CheckSphere(groundCheck.position, sphereRadius, jumpMask);
    }
}
