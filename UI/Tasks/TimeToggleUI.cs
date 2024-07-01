using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeToggleUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Toggle _toggle;

    public bool isOn
    {
        get => _toggle.isOn;
        set => _toggle.isOn = value;
    }

    [HideInInspector] public UnityEvent<bool> OnValueCahnged;

    private void Start()
    {
        _toggle.onValueChanged.AddListener((value) => OnValueCahnged.Invoke(value));
    }

    public void Init(string text, ToggleGroup group = null)
    {
        _valueText.text = text;
        _toggle.group = group;
    }

}
