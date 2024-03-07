using UnityEngine;

public class ManuelController : MonoBehaviour
{
    [SerializeField] private float yLimit;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float boostSpeed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float accelSpeed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float yawSpeed;
    [SerializeField] private float yawAccelSpeed;
    [SerializeField] private float propellerRotateValue;

    [SerializeField] private ParticleSystem[] balloonEffects;
    [SerializeField] private GameObject[] propellers;
    //0 1
    //2 3
    //4 5

    
    private float _rotY;
    private float _y;

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

    private void Update()
    {
        Move();
        HandleBalloonEffectInputs();
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

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetAxis("Vertical") == 1)) moveSpeed = boostSpeed;
        else moveSpeed = normalSpeed;

        _y = 0f;
        _rotY = Input.GetAxis("Mouse X") * yawSpeed;

        if (Input.GetKey(KeyCode.Q) && yLimit > transform.position.y)
        {
            _y = -1f;
            //RotateMiddlePropellers();
            //balloonEffects[2].gameObject.transform.rotation = Quaternion.Euler(90, 45, 45);
            //balloonEffects[3].gameObject.transform.rotation = Quaternion.Euler(90, 45, 45);
            //balloonEffects[2].maxParticles = 200;
            //balloonEffects[3].maxParticles = 200;
        }
        else if (Input.GetKey(KeyCode.E) || yLimit < transform.position.y)
        {
            _y = 1f;
            //RotateMiddlePropellers();
            //balloonEffects[2].gameObject.transform.rotation = Quaternion.Euler(-90, 45, 45);
            //balloonEffects[3].gameObject.transform.rotation = Quaternion.Euler(-90, 45, 45);
            //balloonEffects[2].maxParticles = 200;
            //balloonEffects[3].maxParticles = 200;
        }
        else
        {
            //balloonEffects[2].maxParticles = 0;
            //balloonEffects[3].maxParticles = 0;
        }


        Vector3 rot = new(0f, transform.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rot), Time.deltaTime * rotSpeed);


        Vector3 vel = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal") + transform.up * _y).normalized * moveSpeed;
        _rb.velocity = Vector3.Lerp(_rb.velocity, vel, Time.deltaTime * accelSpeed);

        _rb.angularVelocity = Vector3.Lerp(_rb.angularVelocity, _canRotate ? new Vector3(0, _rotY, 0) : new Vector3(0, 0, 0), Time.deltaTime * yawAccelSpeed);

    }

    private void HandleBalloonEffectInputs()
    {
        if (Input.GetAxis("Vertical") == 1)
        {
            //balloonEffects[0].transform.rotation = Quaternion.Euler(35f, 225f, 135f);
            //balloonEffects[1].transform.rotation = Quaternion.Euler(35f, 135f, 135f);
            //balloonEffects[4].transform.rotation = Quaternion.Euler(35f, 135f, 135f);
            //balloonEffects[5].transform.rotation = Quaternion.Euler(35f, 225f, 135f);
            RotateFrontAndBackPropellers();
            ResumeEffects();
        }
        else if (Input.GetAxis("Vertical") == -1)
        {

            RotateFrontAndBackPropellers();
            ResumeEffects();
        }
        else
        {
            PauseEffects();
        }
    }

    private void RotatePropeller(GameObject propeller)
    {
        propeller.transform.Rotate(Vector3.up * propellerRotateValue * Time.deltaTime);
    }

    private void RotateMiddlePropellers()
    {
        //RotatePropeller(propellers[2]);
        //RotatePropeller(propellers[3]);
    }

    private void RotateFrontAndBackPropellers()
    {
        //RotatePropeller(propellers[0]);
        //RotatePropeller(propellers[1]);
        //RotatePropeller(propellers[4]);
        //RotatePropeller(propellers[5]);
    }

    private void ResumeEffects()
    {
        //balloonEffects[0].maxParticles = 400;
        //balloonEffects[1].maxParticles = 400;
        //balloonEffects[4].maxParticles = 400;
        //balloonEffects[5].maxParticles = 400;
    }

    private void PauseEffects()
    {
        //balloonEffects[0].maxParticles = 0;
        //balloonEffects[1].maxParticles = 0;
        //balloonEffects[4].maxParticles = 0;
        //balloonEffects[5].maxParticles = 0;
    }
}