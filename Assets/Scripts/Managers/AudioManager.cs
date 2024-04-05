using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoSingleton<AudioManager>
{
    public Sound[] Sounds;
    private ushort _currentSoundIndex;

    private void OnEnable()
    {
        //PlayOrStop("Sound1",true);
        PlayOrStop(_currentSoundIndex);
    }

    void Awake()
    {
        foreach (Sound s in Sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.Loop;
            s.Source.spatialBlend = s.SpatialBlend;
        }
    }

    public void PlayOrStop(string name, bool mustStarted)
    {
        Sound s = Array.Find(Sounds, sound => sound.Name == name);
        if (s == null) return;

        if (mustStarted) s.Source.Play();
        else s.Source.Stop();
    }

    public void PlayOrStop(ushort index)
    {
        Sounds[index].Source.Play();

        StartCoroutine(WaitAndPlayNextSound(Sounds[index].Clip.length));
    }

    public IEnumerator WaitAndPlayNextSound(float waitTime)
    {
        yield return Extensions.GetWaitForSeconds(waitTime);

        _currentSoundIndex = (ushort)((_currentSoundIndex + 1) % Sounds.Length);

        PlayOrStop(_currentSoundIndex);

    }
}
