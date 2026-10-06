using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private Slider _globalSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundEffectsSlider;

    private void Start()
    {
        //// 1. On initialise la valeur des sliders avec le volume actuel du Mixer (ou des PlayerPrefs)
        //InitSliderValue("Master", masterVolumeSlider);
        //InitSliderValue("MusicVolume", musicVolumeSlider);
        //InitSliderValue("SFXVolume", sfxVolumeSlider);
        //InitSliderValue("PasVolume", pasVolumeSlider);

        //// 2. On écoute les changements de valeur des sliders en temps réel
        //masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        //musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        //sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        //pasVolumeSlider.onValueChanged.AddListener(SetPasVolume);
    }

    public void SetMasterVolume(float value) => UpdateMixerVolume("Master", value);
    public void SetMusicVolume(float value) => UpdateMixerVolume("MusicVolume", value);
    public void SetSFXVolume(float value) => UpdateMixerVolume("SFXVolume", value);
    public void SetPasVolume(float value) => UpdateMixerVolume("PasVolume", value);

    private void UpdateMixerVolume(string parameterName, float sliderValue)
    {
        // Si le slider est à 0, on coupe complètement le son (-80 dB)
        // Sinon, on convertit la valeur 0-1 en échelle logarithmique de décibels
        float dB = sliderValue > 0 ? Mathf.Log10(sliderValue) * 20f : -80f;

        _audioMixer.SetFloat(parameterName, dB);

        // Optionnel : Sauvegarder le choix du joueur pour le prochain lancement du jeu
        PlayerPrefs.SetFloat(parameterName, sliderValue);
    }

    private void InitSliderValue(string parameterName, Slider slider)
    {
        // On récupère la valeur sauvegardée, ou 0.75f par défaut si c'est le premier lancement
        float savedValue = PlayerPrefs.GetFloat(parameterName, 1f);
        slider.value = savedValue;

        // On applique immédiatement la valeur au mixer au démarrage
        UpdateMixerVolume(parameterName, savedValue);
    }

    private void OnDestroy()
    {
        // Nettoyage des listeners quand on détruit le menu pour éviter les fuites de mémoire
        //masterVolumeSlider.onValueChanged.RemoveListener(SetMasterVolume);
        //musicVolumeSlider.onValueChanged.RemoveListener(SetMusicVolume);
        //sfxVolumeSlider.onValueChanged.RemoveListener(SetSFXVolume);
        //pasVolumeSlider.onValueChanged.RemoveListener(SetPasVolume);
    }
}
