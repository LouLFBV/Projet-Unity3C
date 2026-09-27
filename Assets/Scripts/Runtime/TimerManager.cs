using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _timerText;


    private float _elapsedTime;
    private bool _isTimerRunning;

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
    }

    private void UpdateTimerUI()
    {
        if (_timerText != null)
        {
            _timerText.text = FormatTime(_elapsedTime);
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