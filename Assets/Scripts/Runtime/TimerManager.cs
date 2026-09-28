using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _recordText;


    private float _elapsedTime;
    private bool _isTimerRunning;
    private string _currentSceneName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    { 
        _currentSceneName = SceneManager.GetActiveScene().name;

        DisplayBestRecord();
        StartTimer();
    }

    private void Update()
    {
        if (!_isTimerRunning) return;

        _elapsedTime += Time.deltaTime;
        UpdateTimerUI();
    }

    public void StartTimer()
    {
        _elapsedTime = 0f;
        _isTimerRunning = true;
    }

    public void StopTimer()
    {
        _isTimerRunning = false;
        Debug.Log($"Temps final : {FormatTime(_elapsedTime)}");

        CheckAndSaveRecord();
    }

    private void UpdateTimerUI()
    {
        if (_timerText != null)
        {
            _timerText.text = FormatTime(_elapsedTime);
        }
    }

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

    private void DisplayBestRecord()
    {
        if (_recordText == null) return;

        string recordKey = $"BestTime_{_currentSceneName}";

        if (PlayerPrefs.HasKey(recordKey))
        {
            float bestTime = PlayerPrefs.GetFloat(recordKey);
            _recordText.text = $"Record: {FormatTime(bestTime)}";
        }
        else
        {
            _recordText.text = "Record: --:--.---";
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 1000f) % 1000f);

        // Format : 01:23.456
        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }
}