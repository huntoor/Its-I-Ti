using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plateform : MonoBehaviour
{

    private PlatformEffector2D platformEffector;
    private PlayerInput input;

    private void Awake()
    {
        platformEffector = GetComponent<PlatformEffector2D>();

        input = new PlayerInput();
        input.Enable();

        input.onFoot.Fall.performed += FallFromPlatform;
        input.onFoot.Jump.performed += JumpOnPlatform;
    }

    private void FallFromPlatform(InputAction.CallbackContext context)
    {
        platformEffector.rotationalOffset = 180f;
    }

    private void JumpOnPlatform(InputAction.CallbackContext context)
    {
        platformEffector.rotationalOffset = 0f;
    }
}
