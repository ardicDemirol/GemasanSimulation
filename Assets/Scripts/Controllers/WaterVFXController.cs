using UnityEngine;

public class WaterVFXController : MonoBehaviour
{
    private const string DIRT_VFX_TAG = "Dirt";

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag(DIRT_VFX_TAG))
        {
            Debug.Log("Water VFX Collided with Dirt VFX");
            //other.GetComponent<ParticleSystem>().Stop();
        }
    }
}
