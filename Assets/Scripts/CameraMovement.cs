using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float sensitivity;
    [SerializeField] private float maxRotation;
    [SerializeField] private float tiltRotation;
    [SerializeField] private float tiltSpeed;

    [Header("References")] 
    private Transform player;
    private Transform mainCamera;
    private PauseMenu menu;

    // Rotation
    private float xRotation;
    private float yRotation;
    private float tilt;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        mainCamera = Camera.main.transform;
        menu = GameObject.Find("Canvas").GetComponent<PauseMenu>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (menu.Paused) { return; }
        MoveCamera();
    }

    private void MoveCamera()
    {
        xRotation += Input.GetAxis("Mouse X") * sensitivity;
        yRotation -= Input.GetAxis("Mouse Y") * sensitivity;

        yRotation = Mathf.Clamp(yRotation, -maxRotation, maxRotation);


        if (Input.GetKey(KeyCode.A))
            tilt = Mathf.Lerp(tilt, tiltRotation, tiltSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.D))
            tilt = Mathf.Lerp(tilt, -tiltRotation, tiltSpeed * Time.deltaTime);
        else
            tilt = Mathf.Lerp(tilt, 0, tiltSpeed * Time.deltaTime);


        player.rotation = Quaternion.Euler(player.rotation.x, xRotation, player.rotation.z);
        mainCamera.localRotation = Quaternion.Euler(yRotation, mainCamera.rotation.y, tilt);
    }
}
