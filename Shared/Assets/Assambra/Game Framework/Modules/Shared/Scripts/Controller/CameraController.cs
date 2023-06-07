using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Public")]
    public float cameraPan = 0f;

    public bool IsOverUIElement { set; get; }

    [Header("Serialize fields")]
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private GameObject player = null;

    [Header("Camera offset")]
    [SerializeField] private Vector3 CameraOffset = new Vector3(0f, 1.8f, 0f);
    
    [Header("Camera distance")]
    [SerializeField] private float cameraStartDistance = 5f;
    [SerializeField] float cameraMinDistance = 0f;
    [SerializeField] float cameraMaxDistance = 35f;
    [SerializeField] private float mouseWheelSensitivity = 10f;

    [Header("Camera pan and tilt")]
    [SerializeField] private float cameraPanSpeed = 9f;
    [SerializeField] private float cameraTiltSpeed = 9f;
    [SerializeField] private float cameraTiltMin = -80f;
    [SerializeField] private float cameraTiltMax = 35f;



    // Private variables
    private float mouseX = 0f;
    private float mouseY = 0f;
    private float cameraDistance = 0f;
    private float mouseWheel = 0f;

    private float cameraTilt = 0f;
    
    private float lastCameraPan = 0f;
    private float cameraPanDifference = 0f;

    private void Awake()
    {
        if (mainCamera == null)
        {
            if (GameObject.FindGameObjectWithTag("MainCamera"))
                mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            else
                Debug.LogError("No Camera with Tag MainCamera found");
        }

        mainCamera.transform.parent = gameObject.transform;
        mainCamera.transform.position = Vector3.zero;
        mainCamera.transform.rotation = Quaternion.identity;
        cameraDistance = cameraStartDistance;
    }

    void Start()
    {
        if (player == null)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
                player = GameObject.FindGameObjectWithTag("Player");
            else
                Debug.LogError("No Player with Tag Player found");
        }

        gameObject.transform.localRotation = player.transform.localRotation;
        cameraPan = player.transform.localEulerAngles.y;
    }


    void Update()
    {
        CameraPanCorrection();
        lastCameraPan = cameraPan;

        GetMouseInput();
        HandleCameraDistance();

        if (Input.GetMouseButton(0) && !IsOverUIElement)
        {
            CameraTiltAndPan();
        }

        if (Input.GetMouseButton(1) && !IsOverUIElement)
        {
            CameraTiltAndPan();
            cameraPanDifference = lastCameraPan - cameraPan;
            player.transform.localEulerAngles -= new Vector3(0, cameraPanDifference);
        }
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position + CameraOffset - transform.forward * cameraDistance;
    }

    private void GetMouseInput()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y") ;
        mouseWheel = Input.GetAxis("Mouse ScrollWheel");
    }

    private void HandleCameraDistance()
    {
        cameraDistance -= mouseWheel * mouseWheelSensitivity;
        cameraDistance = Mathf.Clamp(cameraDistance, cameraMinDistance, cameraMaxDistance);
    }

    private void CameraTiltAndPan()
    {
        cameraPan += mouseX * cameraPanSpeed;
        cameraTilt += mouseY * cameraTiltSpeed;

        transform.localEulerAngles = new Vector3(-ClampCameraTilt(cameraTilt), cameraPan, 0);
    }

    private float ClampCameraTilt(float tilt)
    {
        return cameraTilt = Mathf.Clamp(cameraTilt, cameraTiltMin, cameraTiltMax);
    }

    private void CameraPanCorrection()
    {
        if (cameraPan < -180)
            cameraPan = 180;
        if (cameraPan > 180)
            cameraPan = -180; 
    }
}
