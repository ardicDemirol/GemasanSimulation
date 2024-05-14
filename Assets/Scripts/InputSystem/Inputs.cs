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
    public bool LeftShiftButtonPressed;
    public bool EscapeButtonPressed;

    [Header("Mouse Cursor Settings")]
    [SerializeField] bool cursorLocked = true;
    [SerializeField] bool cursorInputForLook = true;

    private VehicleController _vehicleControls;
    private PlayerInput _playerInput;
    private InputDevice _inputDevice;

    [SerializeField] private int _lookInputMultiple;
    [SerializeField] private int _mouseScrollMultiple;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _vehicleControls = new VehicleController();
        SetInputDevice();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }


    private void OnDisable()
    {
        UnsubscribeEvents();
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

        _vehicleControls.Vehicle.MouseLeftButton.performed += OnMouseLeftButtonPerformed;
        _vehicleControls.Vehicle.MouseLeftButton.canceled += OnMouseLeftButtonCancelled;

        _vehicleControls.Vehicle.MouseMiddleButton.performed += OnMouseMiddleButtonPerformed;
        _vehicleControls.Vehicle.MouseMiddleButton.canceled += OnMouseMiddleButtonCancelled;

        _vehicleControls.Vehicle.SpeedUp.performed += ctx => LeftShiftButtonPressed = true;
        _vehicleControls.Vehicle.SpeedUp.canceled += ctx => LeftShiftButtonPressed = false;

        _vehicleControls.Vehicle.Pause.performed += EscapeButtonInput;
    }



    private void UnsubscribeEvents()
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

        _vehicleControls.Vehicle.MouseLeftButton.performed -= OnMouseLeftButtonPerformed;
        _vehicleControls.Vehicle.MouseLeftButton.canceled -= OnMouseLeftButtonCancelled;


        _vehicleControls.Vehicle.MouseMiddleButton.performed -= OnMouseMiddleButtonPerformed;
        _vehicleControls.Vehicle.MouseMiddleButton.canceled -= OnMouseMiddleButtonCancelled;

        _vehicleControls.Vehicle.SpeedUp.performed -= ctx => LeftShiftButtonPressed = true;
        _vehicleControls.Vehicle.SpeedUp.canceled -= ctx => LeftShiftButtonPressed = false;


        _vehicleControls.Vehicle.Pause.performed -= EscapeButtonInput;
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
            LookInput(context.ReadValue<Vector2>() * _lookInputMultiple);
        }
    }

    private void OnLookCancelled(InputAction.CallbackContext context)
    {
        LookInput(Vector2.zero);
    }

    private void OnCameraPerformed(InputAction.CallbackContext context)
    {
        var keyName = context.control.name;
        if(keyName == "up") keyName = "1";
        else if(keyName == "right") keyName = "2";
        else if(keyName == "left") keyName = "3";
        else if(keyName == "down") keyName = "4";
        CameraSignals.Instance.OnCameraChanged?.Invoke(StringToInt(keyName));
    }

    private void OnMouseScrollPerformed(InputAction.CallbackContext context)
    {
        MouseScrollInput(context.ReadValue<Vector2>() * _mouseScrollMultiple);
    }
    private void OnMouseScrollCancelled(InputAction.CallbackContext context)
    {
        MouseScrollInput(Vector2.zero);
    }

    private void OnMouseLeftButtonPerformed(InputAction.CallbackContext context)
    {
        MouseLeftButtonInput(context.ReadValueAsButton());
    }

    private void OnMouseLeftButtonCancelled(InputAction.CallbackContext context)
    {
        MouseLeftButtonInput(false);
    }

    private void OnMouseMiddleButtonPerformed(InputAction.CallbackContext context)
    {
        MouseMiddleButtonInput(context.ReadValueAsButton());
    }

    private void OnMouseMiddleButtonCancelled(InputAction.CallbackContext context)
    {
        MouseLeftButtonInput(false);
    }
    private void EscapeButtonInput(InputAction.CallbackContext context)
    {
        EscapeButtonPressed = !EscapeButtonPressed;
        cursorLocked = !cursorLocked;
        SetCursorState(cursorLocked);
        UISignals.Instance.OnPausePanelToggle?.Invoke();
    }


    //////////////////////////////////////////////////////////////////////////
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
    private void MouseLeftButtonInput(bool newLeftMouseButton)
    {
        LeftMouseButtonPressed = newLeftMouseButton;
    }
    private void MouseMiddleButtonInput(bool newMiddleMouseButton)
    {
        if (newMiddleMouseButton)
        {
            MiddleMouseButtonPressed = !MiddleMouseButtonPressed;
            CameraSignals.Instance.OnPressMiddleClick?.Invoke(MiddleMouseButtonPressed);
        }
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private int StringToInt(string input)
    {
        if (int.TryParse(input, out int result)) return result;
        else return int.MinValue;
    }


    private void SetInputDevice()
    {
        _inputDevice = FindPreferredDevice();

        if (_inputDevice is Keyboard)
        {
            Debug.Log("Keyboard is being used.");
            _playerInput.SwitchCurrentControlScheme("Keyboard");
            _lookInputMultiple = 1;
            _mouseScrollMultiple = 1;
        }
        else if (_inputDevice is Gamepad)
        {
            Debug.Log("Gamepad is being used.");
            _playerInput.SwitchCurrentControlScheme("Gamepad");
            _lookInputMultiple = 20;
            _mouseScrollMultiple = 60;
        }
    }
    private InputDevice FindPreferredDevice()
    {
        InputDevice[] devices = InputSystem.devices.ToArray();
        foreach (InputDevice device in devices)
        {
            if (device is Gamepad)
            {
                return device;
            }
        }
        return InputSystem.devices[0];
    }

}

