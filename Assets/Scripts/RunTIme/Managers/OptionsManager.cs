using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
/// <summary>
/// Manages the audio options and volume settings for the game.
/// Handles synchronization between UI sliders, the audio mixer,
/// and saved player preferences.
/// </summary>
public class OptionsManager : MonoBehaviour
{
    #region --- AUDIO REFERENCES ---

    /// <summary>
    /// Audio mixer used to control the different volume groups.
    /// </summary>
    [SerializeField] private AudioMixer _audioMixer;
    #endregion

    #region --- VOLUME SLIDERS ---

    /// <summary>
    /// Slider controlling the global master volume.
    /// </summary>
    [SerializeField] private Slider _globalSlider;
    /// <summary>
    /// Slider controlling the music volume.
    /// </summary>
    [SerializeField] private Slider _musicSlider;
    /// <summary>
    /// Slider controlling the sound effects volume.
    /// </summary>
    [SerializeField] private Slider _soundEffectsSlider;
    #endregion
    /// <summary>
    /// Initializes the volume sliders using the saved player preferences
    /// and registers their value change listeners.
    /// </summary>
    private void Start()
    {
        InitSliderValue("Master", _globalSlider);
        InitSliderValue("Music", _musicSlider);
        InitSliderValue("SFX", _soundEffectsSlider);

        _globalSlider.onValueChanged.AddListener(SetMasterVolume);
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _soundEffectsSlider.onValueChanged.AddListener(SetSFXVolume);
    }
    /// <summary>
    /// Updates the master volume.
    /// </summary>
    /// <param name="value">Normalized volume value between 0 and 1.</param>
    public void SetMasterVolume(float value) => UpdateMixerVolume("Master", value);
    /// <summary>
    /// Updates the music volume.
    /// </summary>
    /// <param name="value">Normalized volume value between 0 and 1.</param>
    public void SetMusicVolume(float value) => UpdateMixerVolume("Music", value);
    /// <summary>
    /// Updates the sound effects volume.
    /// </summary>
    /// <param name="value">Normalized volume value between 0 and 1.</param>
    public void SetSFXVolume(float value) => UpdateMixerVolume("SFX", value);
    /// <summary>
    /// Converts a normalized slider value to decibels and applies it
    /// to the corresponding audio mixer parameter.
    /// </summary>
    /// <param name="parameterName">Name of the audio mixer parameter to update.</param>
    /// <param name="sliderValue">Normalized slider value between 0 and 1.</param>

    private void UpdateMixerVolume(string parameterName, float sliderValue)
    {
        // If the slider is set to 0, completely mute the audio (-80 dB).
        // Otherwise, convert the 0-1 value to a logarithmic decibel scale.
        float dB = sliderValue > 0 ? Mathf.Log10(sliderValue) * 20f : -80f;

        _audioMixer.SetFloat(parameterName, dB);

        // Save the player's volume preference for the next game launch.
        PlayerPrefs.SetFloat(parameterName, sliderValue);
    }

    /// <summary>
    /// Initializes a volume slider using its saved value,
    /// or a default value if no preference has been saved yet.
    /// </summary>
    /// <param name="parameterName">Name of the audio mixer parameter and player preference key.</param>
    /// <param name="slider">Slider to initialize.</param>
    private void InitSliderValue(string parameterName, Slider slider)
    {
        // Retrieve the saved value, or use 1f by default on the first launch.
        float savedValue = PlayerPrefs.GetFloat(parameterName, 1f);
        slider.value = savedValue;

        // Immediately apply the saved value to the audio mixer.
        UpdateMixerVolume(parameterName, savedValue);
    }

    /// <summary>
    /// Removes the slider listeners when the options manager is destroyed
    /// to prevent unwanted callbacks.
    /// </summary>
    private void OnDestroy()
    {
        // Remove the listeners when the menu is destroyed.
        _globalSlider.onValueChanged.RemoveListener(SetMasterVolume);
        _musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        _soundEffectsSlider.onValueChanged.RemoveListener(SetSFXVolume);
    }
}
