using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MoneyTextUI : MonoBehaviour
{

    [SerializeField] private Wallet _wallet;

    private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _wallet.OnChanged.AddListener(OnWalletChanged);
        OnWalletChanged(_wallet.money);
    }

    private void OnDisable()
    {
        _wallet.OnChanged.RemoveListener(OnWalletChanged);
    }

    private void OnWalletChanged(float money)
    {
        if (_text == null)
            _text = GetComponent<TextMeshProUGUI>();

        _text.text = _wallet.ToString();
    }

}
