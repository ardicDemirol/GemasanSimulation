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


    private float _rotY;

    private Rigidbody _rb;
    private bool _canRotate = true;



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
        _canRotate = !arg0;
    }


    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift) && (Inputs.Instance.Move.x == 1)) moveSpeed = boostSpeed;
        else moveSpeed = normalSpeed;

        if (Inputs.Instance.Height.y == 1)
        {
            balloonEffects[2].gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);
            balloonEffects[3].gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);
            balloonEffects[2].maxParticles = 200;
            balloonEffects[3].maxParticles = 200;
        }
        else if (Inputs.Instance.Height.y == -1)
        {
            balloonEffects[2].gameObject.transform.localEulerAngles = Vector3.zero;
            balloonEffects[3].gameObject.transform.localEulerAngles = Vector3.zero;
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
        _rb.angularVelocity = Vector3.Lerp(_rb.angularVelocity, _canRotate ? new Vector3(0, _rotY, 0) : new Vector3(0, 0, 0), Time.deltaTime * yawAccelSpeed);

    }

    private void HandleBalloonEffectInputs()
    {
        if (Inputs.Instance.Move.x == 1)
        {
            balloonEffects[0].transform.localEulerAngles = new Vector3(0, 0, 90);
            balloonEffects[1].transform.localEulerAngles = new Vector3(0, 0, 90);
            balloonEffects[4].transform.localEulerAngles = new Vector3(0, 0, 90);
            balloonEffects[5].transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (Inputs.Instance.Move.x == -1)
        {
            balloonEffects[0].transform.localEulerAngles = new Vector3(0, 0, 270);
            balloonEffects[1].transform.localEulerAngles = new Vector3(0, 0, 270);
            balloonEffects[4].transform.localEulerAngles = new Vector3(0, 0, 270);
            balloonEffects[5].transform.localEulerAngles = new Vector3(0, 0, 270);
        }

        if (Inputs.Instance.Move.y == 1)
        {
            balloonEffects[0].transform.localEulerAngles = new Vector3(0, 90, 270);
            balloonEffects[1].transform.localEulerAngles = new Vector3(0, 90, 270);
            balloonEffects[4].transform.localEulerAngles = new Vector3(0, 90, 270);
            balloonEffects[5].transform.localEulerAngles = new Vector3(0, 90, 270);
        }
        else if (Inputs.Instance.Move.y == -1)
        {
            balloonEffects[0].transform.localEulerAngles = new Vector3(0, 270, 270);
            balloonEffects[1].transform.localEulerAngles = new Vector3(0, 270, 270);
            balloonEffects[4].transform.localEulerAngles = new Vector3(0, 270, 270);
            balloonEffects[5].transform.localEulerAngles = new Vector3(0, 270, 270);
        }

        if (Inputs.Instance.Move.y == 0 && Inputs.Instance.Move.x == 0)
        {
            PauseEffects();
        }
        else
        {
            ResumeEffects();
        }


        //SetVFXRotation( && Inputs.Instance.Move.x != 0 ? new Vector3(0, 0, 90) : new Vector3(0, 0, 270));
        //SetVFXRotation(Inputs.Instance.Move.y == 1 && Inputs.Instance.Move.y != 0 ? new Vector3(90, 0, 180) : new Vector3(270, 0, 180));
    }


    private bool InputController()
    {
        return Inputs.Instance.Move != Vector2.zero || Inputs.Instance.Height != Vector2.zero || Inputs.Instance.Look != Vector2.zero;
    }


    #region Propeller Rotations
    private void RotatePropeller(GameObject propeller)
    {
        propeller.transform.Rotate(propellerRotateValue * Time.deltaTime * Vector3.up);
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

    private void ResumeEffects()
    {
        balloonEffects[0].maxParticles = 400;
        balloonEffects[1].maxParticles = 400;
        balloonEffects[4].maxParticles = 400;
        balloonEffects[5].maxParticles = 400;
    }

    private void PauseEffects()
    {
        balloonEffects[0].maxParticles = 0;
        balloonEffects[1].maxParticles = 0;
        balloonEffects[4].maxParticles = 0;
        balloonEffects[5].maxParticles = 0;
    }

    #endregion



}