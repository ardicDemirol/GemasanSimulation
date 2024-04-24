using UnityEngine;

public class ArmController : MonoBehaviour
{
    [Header("Arm Controller Settings")]
    [SerializeField] private GameObject armParent;
    [SerializeField] private byte minArmParentAngle = 15;
    [SerializeField] private byte maxArmParentAngle = 105;

    [Header("Head Controller Settings")]
    [SerializeField] private GameObject headParent;
    [SerializeField] private byte minHeadParentAngle = 15;
    [SerializeField] private byte maxHeadParentAngle = 105;

    [Header("VFX Settings")]
    [SerializeField] private ParticleSystem waterParticle;

    private GameObject _rotateParent;

    private float _scrollAmount;
    private float _newZRotation;
    private float _currentZRotation;
    private float _clampedZRotation;
    private short _minParentAngle;
    private short _maxParentAngle;

    private static readonly byte _mouseScrollDivisior = 36;
    private static readonly byte _maxParticleAmount = 75;
    private static readonly byte _minParticleAmount = 0;

    void Update()
    {
        RotateController();
        ControlVFXAmount();
    }

    void ControlVFXAmount()
    {
        if (Inputs.Instance.LeftMouseButtonPressed)
        {
            if(!waterParticle.isPlaying) waterParticle.Play();
            Extensions.ChangeMaxParticle(waterParticle, _maxParticleAmount);
        }
        else
        {
            Extensions.ChangeMaxParticle(waterParticle, _minParticleAmount);
        }
    }

    void RotateController()
    {
        if (Inputs.Instance.MouseScroll != 0)
        {
            _rotateParent = Inputs.Instance.MiddleMouseButtonPressed ? headParent : armParent;
            _minParentAngle = Inputs.Instance.MiddleMouseButtonPressed ? minHeadParentAngle : minArmParentAngle;
            _maxParentAngle = Inputs.Instance.MiddleMouseButtonPressed ? maxHeadParentAngle : maxArmParentAngle;

            _scrollAmount = Mathf.Abs(Inputs.Instance.MouseScroll / _mouseScrollDivisior);
            _newZRotation = Inputs.Instance.MouseScroll > 0 ? _scrollAmount : -_scrollAmount;

            _currentZRotation = _rotateParent.transform.localEulerAngles.z;
            _clampedZRotation = Mathf.Clamp(_currentZRotation + _newZRotation, _minParentAngle, _maxParentAngle);

            if (_clampedZRotation >= _minParentAngle)
            {
                _rotateParent.transform.Rotate(Inputs.Instance.MouseScroll > 0 ? Vector3.forward : Vector3.back, _scrollAmount);
                _rotateParent.transform.localEulerAngles = new Vector3(0, 0, _clampedZRotation);
            }
        }
    }

}
