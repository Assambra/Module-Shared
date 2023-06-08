using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Serialize fields")]
    [SerializeField] private CameraController cameraController = null;
    [SerializeField] private CharacterController characterController = null;

    [Header("Movement speed")]
    [SerializeField] private float movementSpeed = 2f;
    

    [Header("Rotation speed")]
    [SerializeField] private float rotationSpeed = 150f;

    // Private variables
    private Vector3 movement = Vector3.zero;

    private void Awake()
    {
        cameraController = GameObject.FindObjectOfType<CameraController>();

        if (cameraController == null)
            Debug.LogError("No CameraController found!");
    }

    void Update()
    {
        GetAxisInput();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            characterController.Move(transform.rotation * movement * Time.deltaTime * movementSpeed);
        }
        else
        {
            characterController.Move(transform.forward * movement.z * movementSpeed * Time.deltaTime);

            transform.Rotate(new Vector3(0, movement.x * rotationSpeed * Time.deltaTime, 0));
        }
    }

    private void GetAxisInput()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }
}
