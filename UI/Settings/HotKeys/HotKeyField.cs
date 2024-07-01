using HotKeys;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotKeyField : MonoBehaviour
{

    [SerializeField] private string _title;
    [SerializeField] private HotKeyType _hotKeyType;
    [Space]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private Button _inputButton;
    [SerializeField] private TextMeshProUGUI _hotKeyText;

    public string title
    {
        get => _title;
        set
        {
            _title = value;
            if (_titleText != null)
                _titleText.text = value;
        }
    }

    public HotKeyType hotKeyType
    {
        get => _hotKeyType;
        set
        {
            _hotKeyType = value;
            UpdateHotKey();
        }
    }

    public HotKey GetHotKey() => HotKeyManager.GetHotKey(_hotKeyType);

    public void UpdateHotKey()
    {
        if (_hotKeyText != null)
            _hotKeyText.text = GetHotKey().keyCode.ToString();
    }

    private void Start()
    {
        UpdateHotKey();
    }

    private void OnValidate()
    {
        if (_titleText != null)
            _titleText.text = _title;
    }

}
