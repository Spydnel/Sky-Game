using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 rotation;
    Vector3 inputVelocity;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float movementSpeed;
    [SerializeField] float acceleration;

    PlayerInput playerInput;
    InputAction lookInput;
    InputAction moveInput;

    Rigidbody rigidBody;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerInput = GetComponent<PlayerInput>();
        lookInput = playerInput.actions["look"];
        moveInput = playerInput.actions["move"];
        rigidBody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        UpdateRotation();
        UpdateMovement();
    }

    private void FixedUpdate()
    {
        var velocity = rigidBody.velocity;

        velocity = Vector3.MoveTowards(velocity, 
            new Vector3(inputVelocity.x, velocity.y, inputVelocity.z), acceleration);

        rigidBody.velocity = velocity;
    }

    void UpdateRotation()
    {
        var input = lookInput.ReadValue<Vector2>();
        rotation.x += input.x * mouseSensitivity;
        rotation.y += input.y * mouseSensitivity;

        rotation.y = math.clamp(rotation.y, -89f, 89f);

        transform.localRotation = Quaternion.Euler(0, rotation.x, 0);
        cameraTransform.localRotation = Quaternion.Euler(-rotation.y, 0, 0);
    }

    void UpdateMovement()
    {
        var input = moveInput.ReadValue<Vector2>();
        var inputVector = new Vector3();

        inputVector += transform.right * input.x;
        inputVector += transform.forward * input.y;
        inputVector = Vector3.ClampMagnitude(inputVector, 1f);

        var inputDirection = Vector3.Normalize(inputVector);
        inputVelocity = inputVector * movementSpeed;
    }
}
