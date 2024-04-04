using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Image[] checkmarksFPS;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private AudioMixer audioMixer;
                     

    private int _fps;

    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SFX_VOLUME = "SFXVolume";
    private const string FPS_INDEX = "FPSIndex";
    private const string MUSIC = "Music";
    private const string SFX = "SFX";

    private const byte FIVE = 5;
    private const short TWENTY = 20;
    private const short THIRTY = 30;
    private const short FORTY_FIVE = 45;
    private const short SIXTY = 60;
    private const short NINETY = 90;
    private const short ONE_HUNDRED_TWENTY = 120;


    public void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME, 1);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME, 1);

        MaxFPSCotrol(PlayerPrefs.GetInt(FPS_INDEX, 2));
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

    public void PauseScreenControl()
    {
        settingsScreen.SetActive(!settingsScreen.activeInHierarchy);

        if (!settingsScreen.activeInHierarchy) return;
    }

    public void MaxFPSCotrol(int index)
    {
        for (int i = 0; i < FIVE; i++) checkmarksFPS[i].enabled = (i == index);

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
