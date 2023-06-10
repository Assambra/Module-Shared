using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public bool IsWalking = false;

    [Header("Serialize fields")]
    [SerializeField] private CameraController cameraController = null;
    [SerializeField] private CharacterController characterController = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private MovementState movementState;
    [Header("Movement speed")]
    [SerializeField] private float movementSpeed = 2f;
    

    [Header("Rotation speed")]
    [SerializeField] private float rotationSpeed = 150f;

    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 2.0f;

    // Private variables
    private Vector3 movement = Vector3.zero;

    private bool isGrounded = false;
    private Vector3 playerVelocity;
    private float gravity = Physics.gravity.y;

    private void Awake()
    {
        cameraController = GameObject.FindObjectOfType<CameraController>();

        if (cameraController == null)
            Debug.LogError("No CameraController found!");

        movementState = MovementState.Stand;
    }

    void Update()
    {
        GetAxisInputNormalized();

        SetMovementState();
        animator.SetInteger("MovementState", (int)movementState);
        if (playerVelocity.y < 0f && isGrounded)
            playerVelocity.y = gravity * Time.deltaTime;
        else
            playerVelocity.y += gravity * Time.deltaTime;


        if (Input.GetButtonDown("Jump") && isGrounded)
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);

        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
        
        isGrounded = characterController.isGrounded;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            characterController.Move(transform.rotation * movement * Time.deltaTime * movementSpeed);
            
            if (movement.z > 0.01 || movement.z < -0.01)
                animator.SetFloat("Vertical", movement.z);
            else
                animator.SetFloat("Vertical", 0);
            
            if (movement.x > 0.01 || movement.x < -0.01)
                animator.SetFloat("Horizontal", movement.x);
            else
                animator.SetFloat("Horizontal", 0);

        }
        else
        {
            characterController.Move(transform.forward * movement.z * movementSpeed * Time.deltaTime);

            transform.Rotate(new Vector3(0, movement.x * rotationSpeed * Time.deltaTime, 0));

            if (movement.z > 0.01 || movement.z < -0.01)
                animator.SetFloat("Vertical", movement.z);
            else
                animator.SetFloat("Vertical", 0);

            if (movement.x > 0.01 || movement.x < -0.01)
                animator.SetFloat("Horizontal", movement.x);
            else
                animator.SetFloat("Horizontal", 0);
        }
    }

    private void GetAxisInputNormalized()
    {
        // Get Axis Input
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        // Normalize
        movement.Normalize();
    }

    private void SetMovementState()
    {
        if (movement.z != 0)
        {
            if(!IsWalking)
                movementState = MovementState.Run;
            else
                movementState = MovementState.Walk;
        }
        else
        {
            movementState = MovementState.Stand;
        }
        
        if (Input.GetButton("Sprint") && movementState != MovementState.Stand)
        {
            if(movementState == MovementState.Walk)
                IsWalking = false;

            movementState = MovementState.Sprint;
        }
        

        
    }
}
