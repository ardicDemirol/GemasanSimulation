using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 Move;
    public Vector2 Height;
    public Vector2 Look;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    private VehicleControls _vehicleControls;

    private void Awake()
    {
        _vehicleControls = new VehicleControls();
    }

    private void OnEnable()
    {
        _vehicleControls.Enable();
        _vehicleControls.Vehicle.Movement.performed += OnMovementPerformed;
        _vehicleControls.Vehicle.Movement.canceled += OnMovementCancelled;
        
        _vehicleControls.Vehicle.Height.performed += OnHeightPerformed;
        _vehicleControls.Vehicle.Height.canceled += OnHeightCancelled;

        _vehicleControls.Vehicle.Look.performed += OnLookPerformed;
        _vehicleControls.Vehicle.Look.canceled += OnLookCancelled;
    }

   

    private void OnDisable()
    {
        _vehicleControls.Disable();
        _vehicleControls.Vehicle.Movement.performed -= OnMovementPerformed;
        _vehicleControls.Vehicle.Movement.canceled -= OnMovementCancelled;

        _vehicleControls.Vehicle.Height.performed -= OnHeightPerformed;
        _vehicleControls.Vehicle.Height.canceled -= OnHeightCancelled;

        _vehicleControls.Vehicle.Look.performed -= OnLookPerformed;
        _vehicleControls.Vehicle.Look.canceled -= OnLookCancelled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        MoveInput(context.ReadValue<Vector2>());
    }

    private void OnMovementCancelled(InputAction.CallbackContext context)
    {
        Move = Vector2.zero;
    }

    private void OnHeightPerformed(InputAction.CallbackContext context)
    {
        HeightInput(context.ReadValue<Vector2>());
    }

    private void OnHeightCancelled(InputAction.CallbackContext context)
    {
        Height = Vector2.zero;
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
        Look = Vector2.zero;
    }


   
    public void MoveInput(Vector2 newMoveDirection)
    {
        Move = newMoveDirection;
    }

    public void HeightInput(Vector2 newHeightDirection)
    {
        Height = newHeightDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        Look = newLookDirection;
    }

    



    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}

