using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public bool IsWalking = false;

    [Header("Serialize fields")]
    [SerializeField] private CameraController cameraController = null;
    [SerializeField] private CharacterController characterController = null;
    [SerializeField] private Animator animator = null;
    
    [SerializeField] private VerticalState verticalState;
    [SerializeField] private HorizontalState horizontalState;
    
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

        verticalState = VerticalState.Stand;
        horizontalState = HorizontalState.Rotate;
    }

    void Update()
    {
        GetAxisInput();
        SetMovementSpeedValue();
        SetVerticalState();

        animator.SetFloat("VerticalState", (int)verticalState);
        animator.SetFloat("HorizontalState", (int)horizontalState);

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
            horizontalState = HorizontalState.Move;

            //Only normalize if diagonal movement
            if (movement.z != 0 && movement.x != 0)
                movement.Normalize();

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
            horizontalState = HorizontalState.Rotate;

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

    private void GetAxisInput()
    {
        // Get Axis Input
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    private void SetVerticalState()
    {
        if (movement.z != 0 || (movement.x != 0 && Input.GetMouseButton(1)))
        {
            if(!IsWalking)
                verticalState = VerticalState.Run;
            else
                verticalState = VerticalState.Walk;
        }
        else
        {
            verticalState = VerticalState.Stand;
        }
        
        if (Input.GetButton("Sprint") && verticalState != VerticalState.Stand)
        {
            if(verticalState == VerticalState.Walk)
                IsWalking = false;

            verticalState = VerticalState.Sprint;
        }
    }
    private void SetMovementSpeedValue()
    {
        switch (verticalState)
        {
            case VerticalState.Stand:
                movementSpeed = 0;
                break;
            case VerticalState.Walk:
                // diagonal
                if(movement.z != 0 && movement.x != 0)
                {
                    // forward
                    if(movement.z > 0 )
                    {
                        movementSpeed = 1.45f;
                    }
                    // backward
                    if (movement.z < 0)
                        movementSpeed = 1.115f;
                }  
                else
                {
                    // forward
                    if (movement.z > 0)
                        movementSpeed = 1.45f;
                    // backward
                    else
                        movementSpeed = 1.115f;
                    // Left or right
                    if (movement.x != 0 && movement.z == 0)
                        movementSpeed = 1.45f;
                }
                
                break;
            case VerticalState.Run:
                // diagonal
                if (movement.z != 0 && movement.x != 0)
                {
                    // forward
                    if (movement.z > 0)
                    {
                        movementSpeed = 4.251f;
                    }
                    // backward
                    else
                        movementSpeed = 2.869f;
                }
                else
                {
                    //forward
                    if (movement.z > 0)
                        movementSpeed = 4.251f;
                    // Backward
                    else
                        movementSpeed = 2.869f;
                    // Left and right
                    if (movement.x != 0 && movement.z == 0)
                        movementSpeed = 4.178f;
                }    
                break;
            case VerticalState.Sprint:
                if (movement.z != 0 && movement.x != 0)
                {
                    // forward
                    if (movement.z > 0)
                        movementSpeed = 5.255f;
                    // backward
                    else
                        movementSpeed = 3.442f;
                }
                else
                {
                    //forward
                    if (movement.z > 0)
                        movementSpeed = 5.255f;
                    // backward
                    else
                        movementSpeed = 3.442f;
                    
                    // Left and right
                    if (movement.x != 0 && movement.z == 0)
                        movementSpeed = 5.007f;
                }
                break;
            default:
                break;
        }
    }
}
