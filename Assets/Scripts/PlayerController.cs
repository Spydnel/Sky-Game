using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IMovable
{
    Vector2 rotation;
    Vector3 inputVelocity;
    bool onGround;
    bool jumping;

    [SerializeField] Transform cameraTransform;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float movementSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float jumpStrength;

    PlayerInput playerInput;
    InputAction lookInput;
    InputAction moveInput;
    InputAction jumpInput;

    Rigidbody rigidBody;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerInput = GetComponent<PlayerInput>();
        lookInput = playerInput.actions["look"];
        moveInput = playerInput.actions["move"];
        jumpInput = playerInput.actions["jump"];
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdateRotation();
        UpdateMovement();
    }

    void OnCollisionEnter()
    {
        onGround = true;
    }

    void OnCollisionStay()
    {
        onGround = true;
    }

    private void FixedUpdate()
    {
        //Vector3 test()

        onGround = false;
    }

    void LateUpdate()
    {
        //transform.localRotation = Quaternion.Euler(0, rotation.x, 0);
        cameraTransform.localRotation = Quaternion.Euler(-rotation.y, rotation.x, 0);
    }

    void UpdateRotation()
    {
        var input = lookInput.ReadValue<Vector2>();
        rotation.x += input.x * mouseSensitivity;
        rotation.y += input.y * mouseSensitivity;

        rotation.y = math.clamp(rotation.y, -89f, 89f);
    }

    void UpdateMovement()
    {
        jumping = (jumpInput.ReadValue<float>() > 0);
        var input = moveInput.ReadValue<Vector2>();
        var inputVector = new Vector3();
        var facing = Quaternion.Euler(0, rotation.x, 0);

        inputVector += facing * Vector3.right * input.x;
        inputVector += facing * Vector3.forward * input.y;
        inputVector = Vector3.ClampMagnitude(inputVector, 1f);

        var inputDirection = Vector3.Normalize(inputVector);
        inputVelocity = inputVector * movementSpeed;
    }
}
