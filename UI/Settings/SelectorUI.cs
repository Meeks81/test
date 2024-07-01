using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectorUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Button _prevButton;
    [SerializeField] private Button _nextButton;
    [Space]
    [SerializeField] private int _currentIndex;
    [SerializeField] private List<string> _options;
    [Space]
    public UnityEvent<int> onChange;

    public List<string> options => new List<string>(_options);
    public int currentIndex => _currentIndex;

    public void SetIndex(int index)
    {
        if (index < 0 || index >= options.Count)
            return;
        if (options.Count == 0)
        {
            _currentIndex = -1;
            return;
        }

        _currentIndex = index;
        UpdateUI();
    }

    public void SetOptions(List<string> options)
    {
        if (_options == null)
            return;
        _options = options;
        UpdateUI();
    }

    public void Prev()
    {
        _currentIndex--;
        if (currentIndex < 0)
            _currentIndex = 0;

        UpdateUI();
        onChange.Invoke(currentIndex);
    }

    public void Next()
    {
        _currentIndex++;
        if (_currentIndex >= options.Count)
            _currentIndex = options.Count - 1;

        UpdateUI();
        onChange.Invoke(currentIndex);
    }

    private void UpdateUI()
    {
        if (_valueText != null && options.Count > 0 && options.Count >= _currentIndex && _currentIndex >= 0)
            _valueText.text = options[_currentIndex];

        if (_prevButton != null)
            _prevButton.interactable = _currentIndex > 0;
        if (_nextButton != null)
            _nextButton.interactable = _currentIndex < options.Count - 1;
    }

    private void OnValidate()
    {
        if (_currentIndex < -1)
            _currentIndex = -1;

        UpdateUI();
    }

}
