using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameSettingsController : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Image[] checkmarksFPS;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private AudioMixer audioMixer;


    private byte _fps;

    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SFX_VOLUME = "SFXVolume";
    private const string FPS_INDEX = "FPSIndex";
    private const string MUSIC = "Music";
    private const string SFX = "SFX";

    private const byte FIVE = 5;
    private const byte TWENTY = 20;
    private const byte THIRTY = 30;
    private const byte FORTY_FIVE = 45;
    private const byte SIXTY = 60;
    private const byte NINETY = 90;
    private const byte ONE_HUNDRED_TWENTY = 120;

    private void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME, 1);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME, 1);
        MaxFPSCotrol((byte)PlayerPrefs.GetInt(FPS_INDEX,4));
    }

    public void MusicVolumeControl()
    {
        float volume = musicVolumeSlider.value;
        audioMixer.SetFloat(MUSIC, Mathf.Log10(volume) * TWENTY);
        PlayerPrefs.SetFloat(MUSIC_VOLUME, volume);
    }
    public void SFXVolumeControl()
    {
        float volume = sfxVolumeSlider.value;
        audioMixer.SetFloat(SFX, Mathf.Log10(volume) * TWENTY);
        PlayerPrefs.SetFloat(SFX_VOLUME, volume);
    }


    public void MaxFPSCotrol(int index)
    {
        for (byte i = 0; i < FIVE; i++) checkmarksFPS[i].enabled = (i == index);

        switch (index)
        {
            case 0: _fps = THIRTY; break;
            case 1: _fps = FORTY_FIVE; break;
            case 2: _fps = SIXTY; break;
            case 3: _fps = NINETY; break;
            case 4: _fps = ONE_HUNDRED_TWENTY; break;
            default: break;
        }

        Application.targetFrameRate = _fps;
        PlayerPrefs.SetInt(FPS_INDEX, index);

    }


}
