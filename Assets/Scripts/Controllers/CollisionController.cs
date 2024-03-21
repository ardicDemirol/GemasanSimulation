using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private const string BOUNDARY_TAG = "Boundary";
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(BOUNDARY_TAG))
        {
            UISignals.Instance.OnBoundaryInfoPanelOpen?.Invoke(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(BOUNDARY_TAG))
        {
            UISignals.Instance.OnBoundaryInfoPanelOpen?.Invoke(false);
        }
    }
}
