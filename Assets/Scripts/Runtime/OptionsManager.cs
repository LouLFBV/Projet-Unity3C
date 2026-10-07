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
        InitSliderValue("Master", _globalSlider);
        InitSliderValue("Music", _musicSlider);
        InitSliderValue("SFX", _soundEffectsSlider);

        _globalSlider.onValueChanged.AddListener(SetMasterVolume);
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _soundEffectsSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float value) => UpdateMixerVolume("Master", value);
    public void SetMusicVolume(float value) => UpdateMixerVolume("Music", value);
    public void SetSFXVolume(float value) => UpdateMixerVolume("SFX", value);

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
        _globalSlider.onValueChanged.RemoveListener(SetMasterVolume);
        _musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        _soundEffectsSlider.onValueChanged.RemoveListener(SetSFXVolume);
    }
}
