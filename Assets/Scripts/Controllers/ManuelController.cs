using DG.Tweening;
using UnityEngine;

public class ManuelController : MonoSingleton<ManuelController>
{
    [SerializeField] private float normalSpeed = 3;
    [SerializeField] private float boostSpeed = 5;
    [SerializeField] private float accelSpeed = 1;
    //[SerializeField] private float rotSpeed = 5;
    [SerializeField] private float yawSpeed = 0.1f;
    [SerializeField] private float yawAccelSpeed = 2;
    [SerializeField] private float idleDuration = 4f;
    [SerializeField] private float idleMoveDistance = 0.4f;
    [SerializeField] private float propellerRotateValue = 5000;

    [SerializeField] private ParticleSystem[] balloonEffects;
    [SerializeField] private GameObject[] propellers;
    //0 1
    //2 3
    //4 5


    private Rigidbody _rb;
    private Tween _tween;

    private float _moveSpeed;
    private float _rotY;
    private bool _canRotateVehicle = true;
    private bool _canStayIdleMode = true;



    private static readonly Vector3 _forwardRotation = new(0, 90, 270);
    private static readonly Vector3 _backwardRotation = new(0, 270, 270);
    private static readonly Vector3 _leftRotation = new(0, 0, 270);
    private static readonly Vector3 _rightRotation = new(0, 0, 90);
    private static readonly Vector3 _upRotation = new(0, 0, 180);
    private static readonly Vector3 _downRotation = Vector3.zero;

    private static readonly short _positive = 1;
    private static readonly short _negative = -1;
    private static readonly short _zero = 0;
    private static readonly float _errorRate = 0.1f;
    private static readonly short _maxParticle = 200;
    private static readonly short _midParticle = 100;


    #region Unity Callbacks

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() => SubscribeEvents();

    private void Update()
    {
        Move();
        HandleBalloonEffectInputs();
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
        _canRotateVehicle = !arg0;
    }

    private void Move()
    {
        VehicleIdleController();

        if (Inputs.Instance.LeftShiftButtonPressed && (Inputs.Instance.Move.y == _positive)) _moveSpeed = boostSpeed;
        else _moveSpeed = normalSpeed;

        Vector3 vel = (transform.forward * Inputs.Instance.Move.y + transform.right * Inputs.Instance.Move.x + transform.up * Inputs.Instance.Height.y).normalized * _moveSpeed;
        _rb.velocity = Vector3.Lerp(_rb.velocity, vel, Time.deltaTime * accelSpeed);

        _rotY = Inputs.Instance.Look.x * yawSpeed;
        _rb.angularVelocity = Vector3.Lerp(_rb.angularVelocity, _canRotateVehicle ? new Vector3(0, _rotY, 0) : _downRotation, Time.deltaTime * yawAccelSpeed);

    }

    private void HandleBalloonEffectInputs()
    {
        if (Inputs.Instance.Move.x == _positive)
        {
            RotateFrontAndBackPropellers();
            SetVFXEulerAngles(_rightRotation, RotatePropellerType.Move);
            SetVFXCount(_maxParticle, RotatePropellerType.Move);
        }
        else if (Inputs.Instance.Move.x == _negative)
        {
            RotateFrontAndBackPropellers();
            SetVFXEulerAngles(_leftRotation, RotatePropellerType.Move);
            SetVFXCount(_maxParticle, RotatePropellerType.Move);
        }

        if (Inputs.Instance.Move.y == _positive)
        {
            RotateFrontAndBackPropellers();
            SetVFXEulerAngles(_forwardRotation, RotatePropellerType.Move);
            SetVFXCount(_maxParticle, RotatePropellerType.Move);
        }
        else if (Inputs.Instance.Move.y == _negative)
        {
            RotateFrontAndBackPropellers();
            SetVFXEulerAngles(_backwardRotation, RotatePropellerType.Move);
            SetVFXCount(_maxParticle, RotatePropellerType.Move);
        }


        if (Inputs.Instance.Height.y == _positive)
        {
            RotateMiddlePropellers();
            SetVFXEulerAngles(_upRotation, RotatePropellerType.Height);
            SetVFXCount(_maxParticle, RotatePropellerType.Height);
        }
        else if (Inputs.Instance.Height.y == _negative)
        {
            RotateMiddlePropellers();
            SetVFXEulerAngles(_downRotation, RotatePropellerType.Height);
            SetVFXCount(_maxParticle, RotatePropellerType.Height);
        }

        if (Inputs.Instance.Look.x > _zero + _errorRate)
        {
            RotateFrontAndBackPropellers();
            if (Inputs.Instance.Move == Vector2.zero && _canRotateVehicle)
            {
                SetVFXEulerAngles(_rightRotation, RotatePropellerType.Move);
                SetVFXCount(_maxParticle, RotatePropellerType.Move);
            }
        }
        else if (Inputs.Instance.Look.x < _zero - _errorRate)
        {
            RotateFrontAndBackPropellers();
            if (Inputs.Instance.Move == Vector2.zero && _canRotateVehicle)
            {
                SetVFXEulerAngles(_leftRotation, RotatePropellerType.Move);
                SetVFXCount(_maxParticle, RotatePropellerType.Move);
            }
        }
       
        if (Inputs.Instance.Move == Vector2.zero && Inputs.Instance.Look.x == _zero)
        {
            SetVFXCount(_zero, RotatePropellerType.Move);
        }
        if (Inputs.Instance.Height == Vector2.zero && _canStayIdleMode)
        {
            SetVFXCount(_zero, RotatePropellerType.Height);
        }

    }

    void VehicleIdleController()
    {
        if (Inputs.Instance.Move == Vector2.zero && Inputs.Instance.Height == Vector2.zero && _rb.velocity.magnitude < 0.1f && _canStayIdleMode)
        {
            _tween = transform.DOMoveY(transform.position.y + idleMoveDistance, idleDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

            _canStayIdleMode = false;
        }
        else if ((Inputs.Instance.Move != Vector2.zero || Inputs.Instance.Height != Vector2.zero) && !_canStayIdleMode)
        {
            _tween?.Kill();
            _canStayIdleMode = true;
        }

        if (!_canStayIdleMode)
        {
            RotateMiddlePropellers();
            SetVFXCount(_midParticle, RotatePropellerType.Height);
            balloonEffects[2].gameObject.transform.localEulerAngles = _downRotation;
            balloonEffects[3].gameObject.transform.localEulerAngles = _upRotation;
        }
    }


    #region Propeller Rotations
    private void RotatePropeller(GameObject propeller)
    {
        if (_canRotateVehicle) propeller.transform.Rotate(Vector3.up, propellerRotateValue * Time.deltaTime);
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

    void SetVFXEulerAngles(Vector3 vector3, RotatePropellerType rotatePropellers)
    {
        if (rotatePropellers == RotatePropellerType.Move)
        {
            balloonEffects[0].transform.localEulerAngles = vector3;
            balloonEffects[1].transform.localEulerAngles = vector3;
            balloonEffects[4].transform.localEulerAngles = vector3;
            balloonEffects[5].transform.localEulerAngles = vector3;
        }
        else if (rotatePropellers == RotatePropellerType.Height)
        {
            balloonEffects[2].gameObject.transform.localEulerAngles = vector3;
            balloonEffects[3].gameObject.transform.localEulerAngles = vector3;
        }
    }

    void SetVFXCount(int value, RotatePropellerType rotatePropellers)
    {
        if (rotatePropellers == RotatePropellerType.Move)
        {
            Extensions.ChangeMaxParticle(balloonEffects[0], value);
            Extensions.ChangeMaxParticle(balloonEffects[1], value);
            Extensions.ChangeMaxParticle(balloonEffects[4], value);
            Extensions.ChangeMaxParticle(balloonEffects[5], value);
        }
        else if (rotatePropellers == RotatePropellerType.Height)
        {
            Extensions.ChangeMaxParticle(balloonEffects[2], value);
            Extensions.ChangeMaxParticle(balloonEffects[3], value);
        }
    }
    #endregion


}