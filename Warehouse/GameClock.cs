using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameClock : MonoBehaviour
{

    [SerializeField] private ClockWithDays _clock;
    [SerializeField] private int _gameSecondPerSecond;

    public int Day => _clock.day;
    public int Hours => _clock.hours;
    public int Minutes => _clock.minutes;

    [HideInInspector] public UnityEvent<int> OnDayChanged;
    [HideInInspector] public UnityEvent<int> OnHourChanged;
    [HideInInspector] public UnityEvent<int> OnMinuteChanged;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Time.timeScale = 1f;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Time.timeScale = 2f;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Time.timeScale = 5f;
    }

    private void OnEnable()
    {
        StartCoroutine(Timer());
    }

    public override string ToString()
    {
        return _clock.ToString();
    }

    public ClockWithDays ToClockWithDays() => _clock;
    public Clock ToClock() => _clock.ToClock();

    private IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            int oldDay = _clock.day;
            int oldHours = _clock.hours;
            int oldMinutes = _clock.minutes;

            _clock.AddSeconds(_gameSecondPerSecond);

            if (_clock.day != oldDay)
                OnDayChanged?.Invoke(_clock.day);
            if (_clock.hours != oldHours)
                OnHourChanged?.Invoke(_clock.hours);
            if (_clock.minutes != oldMinutes)
                OnMinuteChanged?.Invoke(_clock.minutes);
        }
    }

}
