using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private float maxVelocityX = 0.75f;
    [SerializeField] private float maxVelocityY = 1f;
    [SerializeField] private float maxVelocityZ = 2f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float stopAcceleration = 0.1f;

    private Rigidbody _rb;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
   
    private void FixedUpdate()
    {
        MovementController();
        HeightController();
        MouseController();
        ClampVelocity();

    }

    
    private void MovementController()
    {
        if (Inputs.Instance.Move.y != 0f && Mathf.Abs(_rb.velocity.z) <= Mathf.Abs(maxVelocityZ))
        {
            _rb.AddRelativeForce(100f * Time.fixedDeltaTime * Inputs.Instance.Move.y * Vector3.forward, ForceMode.Force);
        }
        if (Inputs.Instance.Move.x != 0f && Mathf.Abs(_rb.velocity.x) <= Mathf.Abs(maxVelocityX))
        {
            _rb.AddRelativeForce(Inputs.Instance.Move.x * 50f * Time.fixedDeltaTime * Vector3.right, ForceMode.Force);
        }
        if (_rb.velocity.z != 0f)
        {
            float acceleration = (_rb.velocity.z > 0f) ? -stopAcceleration : stopAcceleration;
            _rb.velocity += new Vector3(0f, 0f, acceleration * Time.fixedDeltaTime);
        }
        if (_rb.velocity.x != 0f)
        {
            float acceleration = (_rb.velocity.x > 0f) ? -stopAcceleration : stopAcceleration;
            _rb.velocity += new Vector3(acceleration * Time.fixedDeltaTime, 0f, 0f);
        }



    }
    private void HeightController()
    {
        if (Inputs.Instance.Height != Vector2.zero)
        {
            if (Mathf.Abs(_rb.velocity.y) <= Mathf.Abs(maxVelocityY))
            {
                _rb.AddForce(Inputs.Instance.Height.y * 75f * Time.fixedDeltaTime * Vector3.up, ForceMode.Acceleration);
            }

        }
        else if (_rb.velocity.y != 0f)
        {
            float acceleration = (_rb.velocity.y > 0f) ? -stopAcceleration : stopAcceleration;
            _rb.velocity += new Vector3(0f, acceleration * Time.fixedDeltaTime, 0f);
        }
    }

    private void MouseController()
    {
        transform.Rotate(Vector3.up, Inputs.Instance.Look.x * rotationSpeed * Time.fixedDeltaTime / 10);
    }

    private void ClampVelocity()
    {
        Vector3 clampedVelocity = _rb.velocity;

        clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -maxVelocityX, maxVelocityX);
        clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, -maxVelocityY, maxVelocityY);
        clampedVelocity.z = Mathf.Clamp(clampedVelocity.z, -maxVelocityZ, maxVelocityZ);

        _rb.velocity = clampedVelocity;
    }

}

