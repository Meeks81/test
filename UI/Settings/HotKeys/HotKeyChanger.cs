using HotKeys;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HotKeyChanger : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _hotKeyText;
    [SerializeField] private Image _hotKeyBox;
    [Space]
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _unsuccessColor;

    public HotKey currentHotKey { get; private set; }

    private UnityAction<KeyCode> _onChange;

    public void SetHotKey(HotKey hotKey, UnityAction<KeyCode> onChange)
    {
        _onChange = onChange;
        currentHotKey = hotKey;

        _titleText.text = "Нажми любую кнопку...";
        _hotKeyBox.color = _defaultColor;
        _hotKeyText.text = currentHotKey.keyCode.ToString();
    }

    private void OnGUI()
    {
        if (currentHotKey == null)
            return;

        if (Event.current.isKey && Event.current.type == EventType.KeyUp)
        {
            if (Event.current.keyCode == KeyCode.None)
                return;

            KeyCode keyCode = Event.current.keyCode;

            if (currentHotKey.keyCode == keyCode)
                return;

            bool result = HotKeyManager.ChangeHotKeyCode(currentHotKey, keyCode);
            _hotKeyText.text = keyCode.ToString();

            if (result)
            {
                _titleText.text = "HotKey успешно установлен!";
                _hotKeyBox.color = _successColor;
                _onChange(keyCode);
            }
            else
            {
                _titleText.text = "Данная клавиша уже используется...";
                _hotKeyBox.color = _unsuccessColor;
            }
        }
    }

}
