using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private float maxVelocityY = 2f;
    [SerializeField] private float maxVelocityX = 1f;
    [SerializeField] private float stopAcceleration = 0.1f;

    private Inputs _input;
    private Rigidbody _rb;


    private void Awake()
    {
        _input = GetComponent<Inputs>();
        _rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {


    }

    private void FixedUpdate()
    {
        if (_input.Move != Vector2.zero)
        {
            if (Mathf.Abs(_rb.velocity.z) <= Mathf.Abs(maxVelocityY) /*|| Mathf.Abs(_rb.velocity.x) <= Mathf.Abs(maxVelocityX)*/)
            {
                _rb.AddForce(_input.Move.y * 100f * Time.fixedDeltaTime * Vector3.forward);
                //_rb.AddForce(_input.Move.x * 50f * Time.fixedDeltaTime * Vector3.left);
            }
        }
        else if (_rb.velocity.z != 0f)
        {
            float acceleration = (_rb.velocity.z > 0f) ? -stopAcceleration : stopAcceleration;
            _rb.velocity += new Vector3(_rb.velocity.x, _rb.velocity.y, acceleration * Time.fixedDeltaTime);
        }


    }

}
