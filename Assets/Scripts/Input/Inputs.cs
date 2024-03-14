using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : MonoSingleton<Inputs>
{
    [Header("Character Input Values")]
    public Vector2 Move;
    public Vector2 Height;
    public Vector2 Look;
    public short MouseScroll;
    public bool LeftMouseButtonPressed;
    public bool MiddleMouseButtonPressed;
    public bool LeftShiftPressed;


    [Header("Mouse Cursor Settings")]
    [SerializeField] bool cursorLocked = true;
    [SerializeField] bool cursorInputForLook = true;

    private VehicleControls _vehicleControls;

    protected override void Awake()
    {
        base.Awake();
        _vehicleControls = new VehicleControls();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }


    private void SubscribeEvents()
    {
        _vehicleControls.Enable();
        _vehicleControls.Vehicle.Movement.performed += OnMovementPerformed;
        _vehicleControls.Vehicle.Movement.canceled += OnMovementCancelled;

        _vehicleControls.Vehicle.Height.performed += OnHeightPerformed;
        _vehicleControls.Vehicle.Height.canceled += OnHeightCancelled;

        _vehicleControls.Vehicle.Look.performed += OnLookPerformed;
        _vehicleControls.Vehicle.Look.canceled += OnLookCancelled;

        _vehicleControls.Vehicle.Camera.performed += OnCameraPerformed;

        _vehicleControls.Vehicle.MouseScroll.performed += OnMouseScrollPerformed;
        _vehicleControls.Vehicle.MouseScroll.canceled += OnMouseScrollCancelled;

        _vehicleControls.Vehicle.MouseButton.performed += OnMouseButtonPerformed;
        _vehicleControls.Vehicle.MouseButton.canceled += OnMouseButtonCancelled;

        _vehicleControls.Vehicle.SpeedUp.performed += ctx => LeftShiftPressed = true;
        _vehicleControls.Vehicle.SpeedUp.canceled += ctx => LeftShiftPressed = false;

    }



    private void UnSubscribeEvents()
    {
        _vehicleControls.Disable();
        _vehicleControls.Vehicle.Movement.performed -= OnMovementPerformed;
        _vehicleControls.Vehicle.Movement.canceled -= OnMovementCancelled;

        _vehicleControls.Vehicle.Height.performed -= OnHeightPerformed;
        _vehicleControls.Vehicle.Height.canceled -= OnHeightCancelled;

        _vehicleControls.Vehicle.Look.performed -= OnLookPerformed;
        _vehicleControls.Vehicle.Look.canceled -= OnLookCancelled;

        _vehicleControls.Vehicle.Camera.performed -= OnCameraPerformed;

        _vehicleControls.Vehicle.MouseScroll.performed -= OnMouseScrollPerformed;
        _vehicleControls.Vehicle.MouseScroll.canceled -= OnMouseScrollCancelled;

        _vehicleControls.Vehicle.MouseButton.performed -= OnMouseButtonPerformed;
        _vehicleControls.Vehicle.MouseButton.canceled -= OnMouseButtonCancelled;

        _vehicleControls.Vehicle.SpeedUp.performed -= ctx => LeftShiftPressed = true;
        _vehicleControls.Vehicle.SpeedUp.canceled -= ctx => LeftShiftPressed = false;
    }



    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        MoveInput(context.ReadValue<Vector2>());
    }

    private void OnMovementCancelled(InputAction.CallbackContext context)
    {
        MoveInput(Vector2.zero);
    }

    private void OnHeightPerformed(InputAction.CallbackContext context)
    {
        HeightInput(context.ReadValue<Vector2>());
    }

    private void OnHeightCancelled(InputAction.CallbackContext context)
    {
        HeightInput(Vector2.zero);
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        if (cursorInputForLook)
        {
            LookInput(context.ReadValue<Vector2>());
        }
    }

    private void OnLookCancelled(InputAction.CallbackContext context)
    {
        LookInput(Vector2.zero);
    }

    private void OnCameraPerformed(InputAction.CallbackContext context)
    {
        var keyName = context.control.name;
        CameraSignals.Instance.OnCameraChanged?.Invoke(StringToInt(keyName));
    }

    private void OnMouseScrollPerformed(InputAction.CallbackContext context)
    {
        MouseScrollInput(context.ReadValue<Vector2>());
    }
    private void OnMouseScrollCancelled(InputAction.CallbackContext context)
    {
        MouseScrollInput(Vector2.zero);
    }
    private void OnMouseButtonPerformed(InputAction.CallbackContext context)
    {
        MouseLeftButtonInput(Mouse.current.leftButton.isPressed);
        MouseMiddleButtonInput(Mouse.current.middleButton.isPressed);
    }

    private void OnMouseButtonCancelled(InputAction.CallbackContext context)
    {
        MouseLeftButtonInput(false);
    }


    private void MoveInput(Vector2 newMoveDirection)
    {
        Move = newMoveDirection;
    }

    private void HeightInput(Vector2 newHeightDirection)
    {
        Height = newHeightDirection;
    }

    private void LookInput(Vector2 newLookDirection)
    {
        Look = newLookDirection;
    }

    private void MouseScrollInput(Vector2 newScrollDirection)
    {
        MouseScroll = (short)newScrollDirection.y;
    }
    private void MouseLeftButtonInput(bool newMouseButton)
    {
        LeftMouseButtonPressed = newMouseButton;
    }

    private void MouseMiddleButtonInput(bool newMouseButton)
    {
        if (newMouseButton)
        {
            MiddleMouseButtonPressed = !MiddleMouseButtonPressed;
        }
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private int StringToInt(string input)
    {
        int result;
        if (int.TryParse(input, out result))
        {
            return result;
        }
        else
        {
            return int.MinValue;
        }
    }
}

