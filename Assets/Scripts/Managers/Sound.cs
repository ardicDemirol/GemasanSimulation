using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewSound", menuName = "ScriptableObject/SoundData", order = 1)]
public class Sound : ScriptableObject
{
    public string Name;
    public AudioClip Clip;

    [Range(0f, 1f)] public float Volume;
    [Range(0.1f, 3f)] public float Pitch;
    [Range(0f, 1f)] public float SpatialBlend;
    public bool Loop;

    [HideInInspector]
    public AudioSource Source;
}
