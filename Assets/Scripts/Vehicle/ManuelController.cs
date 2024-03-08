using UnityEngine;

public class ManuelController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float boostSpeed = 5;
    [SerializeField] private float normalSpeed = 3;
    [SerializeField] private float accelSpeed = 1;
    //[SerializeField] private float rotSpeed = 5;
    [SerializeField] private float yawSpeed = 0.1f;
    [SerializeField] private float yawAccelSpeed = 2;
    [SerializeField] private float propellerRotateValue;

    [SerializeField] private ParticleSystem[] balloonEffects;
    [SerializeField] private GameObject[] propellers;
    //0 1
    //2 3
    //4 5


    private Rigidbody _rb;

    private float _rotY;
    private bool _canRotate = true;



    private static readonly Vector3 _forwardRotation = new(0, 90, 270);
    private static readonly Vector3 _backwardRotation = new(0, 270, 270);
    private static readonly Vector3 _leftRotation = new(0, 0, 270);
    private static readonly Vector3 _rightRotation = new(0, 0, 90);
    private static readonly Vector3 _upRotation = new(0, 0, 180);
    private static readonly Vector3 _downRotation = Vector3.zero;

    private static readonly short _positive = 1;
    private static readonly short _negative = -1;



    #region Unity Callbacks
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() => SubscribeEvents();
    private void Update()
    {
        Move(GetBalloonEffects());

        HandleBalloonEffectInputs();
        RotateFrontAndBackPropellers();
    }

    private void OnDisable() => UnSubscribeEvents();
    #endregion

    #region Event Subscriptions
    private void SubscribeEvents()
    {
        CameraSignals.Instance.OnPressRightClick += PressRightClick;
    }
    private void UnSubscribeEvents()
    {
        CameraSignals.Instance.OnPressRightClick -= PressRightClick;
    }
    #endregion


    private void PressRightClick(bool arg0)
    {
        _canRotate = !arg0;
    }

    private ParticleSystem[] GetBalloonEffects()
    {
        return balloonEffects;
    }

    private void Move(ParticleSystem[] balloonEffects)
    {
        if (Input.GetKey(KeyCode.LeftShift) && (Inputs.Instance.Move.x == _positive)) moveSpeed = boostSpeed;
        else moveSpeed = normalSpeed;

        if (Inputs.Instance.Height.y == _positive)
        {
            balloonEffects[2].gameObject.transform.localEulerAngles = _upRotation;
            balloonEffects[3].gameObject.transform.localEulerAngles = _upRotation;
            balloonEffects[2].maxParticles = 200;
            balloonEffects[3].maxParticles = 200;
        }
        else if (Inputs.Instance.Height.y == _negative)
        {
            balloonEffects[2].gameObject.transform.localEulerAngles = _downRotation;
            balloonEffects[3].gameObject.transform.localEulerAngles = _downRotation;
            balloonEffects[2].maxParticles = 200;
            balloonEffects[3].maxParticles = 200;
        }
        else
        {
            balloonEffects[2].maxParticles = 0;
            balloonEffects[3].maxParticles = 0;
        }

        //Vector3 rot = new(0f, transform.eulerAngles.y, 0f);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rot), Time.deltaTime * rotSpeed);


        Vector3 vel = (transform.forward * Inputs.Instance.Move.y + transform.right * Inputs.Instance.Move.x + transform.up * Inputs.Instance.Height.y).normalized * moveSpeed;
        _rb.velocity = Vector3.Lerp(_rb.velocity, vel, Time.deltaTime * accelSpeed);

        _rotY = Inputs.Instance.Look.x * yawSpeed;
        _rb.angularVelocity = Vector3.Lerp(_rb.angularVelocity, _canRotate ? new Vector3(0, _rotY, 0) : _downRotation, Time.deltaTime * yawAccelSpeed);

    }

    private void HandleBalloonEffectInputs()
    {
        if (Inputs.Instance.Move.x == _positive)
        {
            SetVFXEulerAngles(_rightRotation);
        }
        else if (Inputs.Instance.Move.x == _negative)
        {
            SetVFXEulerAngles(_leftRotation);
        }

        if (Inputs.Instance.Move.y == _positive)
        {
            SetVFXEulerAngles(_forwardRotation);
        }
        else if (Inputs.Instance.Move.y == _negative)
        {
            SetVFXEulerAngles(_backwardRotation);
        }

        SetVFXCount(Inputs.Instance.Move.y == 0 && Inputs.Instance.Move.x == 0 ? 0 : 400);

    }


    #region Propeller Rotations
    private void RotatePropeller(GameObject propeller)
    {
        //propeller.transform.Rotate(propellerRotateValue * Time.deltaTime * Vector3.up);
        
        propeller.transform.Rotate(Vector3.up, propellerRotateValue * Time.deltaTime);

    }

    private void RotateMiddlePropellers()
    {
        RotatePropeller(propellers[2]);
        RotatePropeller(propellers[3]);
    }

    private void RotateFrontAndBackPropellers()
    {
        RotatePropeller(propellers[0]);
        RotatePropeller(propellers[1]);
        RotatePropeller(propellers[4]);
        RotatePropeller(propellers[5]);
    }



    #endregion

    #region Balloon Effects

    void SetVFXEulerAngles(Vector3 vector3)
    {
        balloonEffects[0].transform.localEulerAngles = vector3;
        balloonEffects[1].transform.localEulerAngles = vector3;
        balloonEffects[4].transform.localEulerAngles = vector3;
        balloonEffects[5].transform.localEulerAngles = vector3;
    }

    void SetVFXCount(int value)
    {
        balloonEffects[0].maxParticles = value;
        balloonEffects[1].maxParticles = value;
        balloonEffects[4].maxParticles = value;
        balloonEffects[5].maxParticles = value;
    }
    #endregion


}