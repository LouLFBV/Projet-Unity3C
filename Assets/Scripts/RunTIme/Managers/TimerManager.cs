using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the level timer and keeps track of the best completion time
/// for each scene using <see cref="PlayerPrefs"/>.
/// </summary>
public class TimerManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the <see cref="TimerManager"/>.
    /// </summary>
    public static TimerManager Instance { get; private set; }

    #region --- UI ---

    [Header("UI")]
    /// <summary>
    /// UI text displaying the current elapsed time.
    /// </summary>
    [SerializeField] private TextMeshProUGUI _timerText;
    /// <summary>
    /// UI text displaying the best recorded time for the current scene.
    /// </summary>
    [SerializeField] private TextMeshProUGUI _recordText;


    #endregion

    #region --- TIMER DATA ---

    /// <summary>
    /// Current elapsed time in seconds.
    /// </summary>
    private float _elapsedTime;
    /// <summary>
    /// Indicates whether the timer is currently running.
    /// </summary>
    private bool _isTimerRunning;
    /// <summary>
    /// Name of the currently active scene used to identify its best record.
    /// </summary>
    private string _currentSceneName;

    #endregion

    /// <summary>
    /// Initializes the singleton instance and prevents duplicate instances.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Initializes the timer for the current scene, displays the best record,
    /// and starts the timer.
    /// </summary>
    private void Start()
    { 
        _currentSceneName = SceneManager.GetActiveScene().name;

        DisplayBestRecord();
        StartTimer();
    }
    /// <summary>
    /// Updates the elapsed time and refreshes the timer UI while the timer is running.
    /// </summary>
    private void Update()
    {
        if (!_isTimerRunning) return;

        _elapsedTime += Time.deltaTime;
        UpdateTimerUI();
    }
    /// <summary>
    /// Starts or restarts the timer from zero.
    /// </summary>  
    public void StartTimer()
    {
        _elapsedTime = 0f;
        _isTimerRunning = true;
    }
    /// <summary>
    /// Stops the timer, logs the final time, and checks whether a new record was achieved.
    /// </summary>
    public void StopTimer()
    {
        _isTimerRunning = false;
        Debug.Log($"Temps final : {FormatTime(_elapsedTime)}");

        CheckAndSaveRecord();
    }
    /// <summary>
    /// Updates the timer UI with the current elapsed time.
    /// </summary>
    private void UpdateTimerUI()
    {
        if (_timerText != null)
        {
            _timerText.text = FormatTime(_elapsedTime);
        }
    }
    /// <summary>
    /// Checks whether the current elapsed time is better than the saved record
    /// and saves it if a new record has been achieved.
    /// </summary>
    private void CheckAndSaveRecord()
    {
        string recordKey = $"BestTime_{_currentSceneName}";

        float previousBest = PlayerPrefs.GetFloat(recordKey, float.MaxValue);

        if (_elapsedTime < previousBest)
        {
            PlayerPrefs.SetFloat(recordKey, _elapsedTime);
            PlayerPrefs.Save(); 

            Debug.Log($"<color=green>Nouveau Record pour {_currentSceneName} : {FormatTime(_elapsedTime)} !</color>");
            DisplayBestRecord();
        }
    }
    /// <summary>
    /// Displays the best recorded time for the current scene,
    /// or a default placeholder if no record exists.
    /// </summary>
    private void DisplayBestRecord()
    {
        if (_recordText == null) return;

        string recordKey = $"BestTime_{_currentSceneName}";

        if (PlayerPrefs.HasKey(recordKey))
        {
            float bestTime = PlayerPrefs.GetFloat(recordKey);
            _recordText.text = $"Record :\n {FormatTime(bestTime)}";
        }
        else
        {
            _recordText.text = "Record :\n--:--.---";
        }
    }
    /// <summary>
    /// Formats a time value in seconds into a minutes, seconds,
    /// and milliseconds representation.
    /// </summary>
    /// <param name="timeInSeconds">Time value to format, expressed in seconds.</param>
    /// <returns>The formatted time in the <c>MM:SS.mmm</c> format.</returns>
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 1000f) % 1000f);

        // Format : 01:23.456
        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }
}