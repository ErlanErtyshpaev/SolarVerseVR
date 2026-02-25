using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SolarSystemUI : MonoBehaviour
{
    public SolarSystemManager manager;

    [Header("UI")]
    public TMP_Text dateText;
    public TMP_Text speedText;
    public Slider speedSlider;
    public Button playPauseButton;
    public TMP_Text playPauseButtonText;
    public Button stepBackButton;
    public Button stepForwardButton;
    public Button resetButton;

    double startDays;
    float startSpeed;
    bool startRunning;

    void Start()
    {
        if (!manager)
        {
            Debug.LogError("SolarSystemUI: manager is not set!");
            return;
        }

        // запомним стартовые значения для Reset
        startDays = manager.currentDaysFromEpoch;
        startSpeed = manager.daysPerSecond;
        startRunning = manager.isRunning;

        // настроим слайдер
        speedSlider.minValue = 0;
        speedSlider.maxValue = 100;
        speedSlider.value = manager.daysPerSecond;

        // подписки на кнопки
        playPauseButton.onClick.AddListener(TogglePlay);
        stepBackButton.onClick.AddListener(() => StepDays(-1));
        stepForwardButton.onClick.AddListener(() => StepDays(+1));
        resetButton.onClick.AddListener(ResetAll);

        // изменение скорости слайдером
        speedSlider.onValueChanged.AddListener(OnSpeedChanged);

        RefreshUI();
    }

    void Update()
    {
        if (!manager) return;
        RefreshUI();
    }

    void OnSpeedChanged(float value)
    {
        manager.daysPerSecond = value;
    }

    void TogglePlay()
    {
        manager.isRunning = !manager.isRunning;
        RefreshUI();
    }

    void StepDays(int delta)
    {
        manager.currentDaysFromEpoch += delta;
        manager.isRunning = false;
        manager.UpdateAllPlanetsPublic(); // вызов приватного метода через SendMessage
        RefreshUI();
    }

    void ResetAll()
    {
        manager.currentDaysFromEpoch = startDays;
        manager.daysPerSecond = startSpeed;
        manager.isRunning = startRunning;
        manager.UpdateAllPlanetsPublic();
        speedSlider.value = manager.daysPerSecond;
        RefreshUI();
    }

    void RefreshUI()
    {
        if (!manager) return;

        // дата
        dateText.text = $"Date: {manager.GetDateStringPublic()}";
        // скорость
        speedText.text = $"Speed: {manager.daysPerSecond:0} days/sec";
        // кнопка play/pause
        playPauseButtonText.text = manager.isRunning ? "Pause" : "Play";
    }
}
