using DG.Tweening;
using UnityEngine;

public class WaterVFXController : MonoBehaviour
{
    [SerializeField] private float minCollisionTime = 3f;
    private float _currentCollisionTime;
    private const string DIRT_VFX_TAG = "Dirt";

    private GameObject _lastDirtObject;


    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag(DIRT_VFX_TAG))
        {
            if (_lastDirtObject != other)
            {
                _lastDirtObject = other;
                _currentCollisionTime = 0;
            }
            if (Timer())
            {
                if (other.TryGetComponent(out ParticleSystem particleSystem))
                {
                    particleSystem.Stop();
                }
                else if (other.TryGetComponent(out SpriteRenderer spriteRenderer))
                {
                    spriteRenderer.material.DOFade(0, 3f);
                }
                else if (other.TryGetComponent(out MeshRenderer meshRenderer))
                {
                    meshRenderer.material.DOFade(0, 3f);
                }

                UISignals.Instance.OnDirtClean?.Invoke();
                EventSignals.Instance.OnDirtClean?.Invoke(other);

            }
        }
    }

    private bool Timer()
    {
        if (_currentCollisionTime >= minCollisionTime)
        {
            _currentCollisionTime = 0;
            return true;
        }
        else
        {
            _currentCollisionTime += 1 * Time.maximumParticleDeltaTime;
            return false;
        }
    }


}
