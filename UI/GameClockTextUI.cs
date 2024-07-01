using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class GameClockTextUI : MonoBehaviour
{

    [SerializeField] private GameClock _gameClock;

    private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _gameClock.OnMinuteChanged.AddListener(OnMinutesChanged);
        OnMinutesChanged(_gameClock.Minutes);
    }

    private void OnDisable()
    {
        _gameClock.OnMinuteChanged.RemoveListener(OnMinutesChanged);
    }

    private void OnMinutesChanged(int minutes)
    {
        if (_text == null)
            _text = GetComponent<TextMeshProUGUI>();

        _text.text = _gameClock.ToString();
    }

}
