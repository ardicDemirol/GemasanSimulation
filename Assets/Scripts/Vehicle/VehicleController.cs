using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private float maxVelocityX = 0.75f;
    [SerializeField] private float maxVelocityY = 1f;
    [SerializeField] private float maxVelocityZ = 2f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float stopAcceleration = 0.1f;

    private Rigidbody _rb;
    private bool _canRotate = true;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }


    private void FixedUpdate()
    {
        MovementController();
        HeightController();
        MouseController();
        ClampVelocity();
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }


    private void SubscribeEvents()
    {
        CameraSignals.Instance.OnPressRightClick += PressRightClick;
    }

    private void UnSubscribeEvents()
    {
        CameraSignals.Instance.OnPressRightClick -= PressRightClick;
    }

    private void PressRightClick(bool arg0)
    {
        _canRotate = !arg0;
    }

    private void MovementController()
    {
        RBMovement();
    }

    private void HeightController()
    {
        if (Inputs.Instance.Height != Vector2.zero)
        {
            _rb.AddRelativeForce(Inputs.Instance.Height.y * 75f * Time.fixedDeltaTime * Vector3.up, ForceMode.Acceleration);
        }
        else if (_rb.velocity.y != 0f)
        {
            float acceleration = (_rb.velocity.y > 0f) ? -stopAcceleration : stopAcceleration;
            _rb.velocity += new Vector3(0f, acceleration * Time.fixedDeltaTime, 0f);
        }
    }

    private void MouseController()
    {
        if (!_canRotate) return;
        transform.Rotate(Vector3.up, Inputs.Instance.Look.x * rotationSpeed * Time.fixedDeltaTime);
    }

    private void ClampVelocity()
    {
        Vector3 clampedVelocity = _rb.velocity;

        clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -maxVelocityX, maxVelocityX);
        clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, -maxVelocityY, maxVelocityY);
        clampedVelocity.z = Mathf.Clamp(clampedVelocity.z, -maxVelocityZ, maxVelocityZ);

        _rb.velocity = clampedVelocity;
    }



    void RBMovement()
    {
        //Vector3 moveDirection = new Vector3(Inputs.Instance.Move.x, 0f, Inputs.Instance.Move.y).normalized;

        //Debug.Log(Inputs.Instance.Move.x != 0);

        //if (moveDirection != Vector3.zero)
        //{
        //    Vector3 force = 100f * Time.fixedDeltaTime * moveDirection;
        //    _rb.AddRelativeForce(force, ForceMode.Force);
        //}
        //else
        //{
        //    Vector3 deceleration = -_rb.velocity.normalized * stopAcceleration * Time.fixedDeltaTime;
        //    _rb.velocity += deceleration;
        //}

    }



}

