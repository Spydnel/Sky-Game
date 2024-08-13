using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 lookDirection;
    Vector3 velocity;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float movementSpeed;
    [SerializeField] float acceleration;

    PlayerInput playerInput;
    InputAction lookInput;
    InputAction moveInput;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerInput = GetComponent<PlayerInput>();
        lookInput = playerInput.actions["look"];
        moveInput = playerInput.actions["move"];
    }
    void Update()
    {
        UpdateDirection();
        UpdateMovement();

    }

    void UpdateDirection()
    {
        var lookInputDirection = lookInput.ReadValue<Vector2>();
        lookDirection.x += lookInputDirection.x * mouseSensitivity;
        lookDirection.y += lookInputDirection.y * mouseSensitivity;

        lookDirection.y = math.clamp(lookDirection.y, -89f, 89f);

        transform.localRotation = Quaternion.Euler(0, lookDirection.x, 0);
        cameraTransform.localRotation = Quaternion.Euler(-lookDirection.y, 0, 0);
    }

    void UpdateMovement()
    {
        var moveInputDirection = moveInput.ReadValue<Vector2>();

        var moveDirection = new Vector3();
        moveDirection += transform.right * moveInputDirection.x;
        moveDirection += transform.forward * moveInputDirection.y;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);
        moveDirection *= movementSpeed;

        velocity.x = Mathf.Lerp(velocity.x, moveDirection.x, 1 - Mathf.Exp(-acceleration * Time.deltaTime));
        velocity.z = Mathf.Lerp(velocity.z, moveDirection.z, 1 - Mathf.Exp(-acceleration * Time.deltaTime));

        transform.Translate(velocity * Time.deltaTime, Space.World);
        
    }
}
