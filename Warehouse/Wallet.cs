using UnityEngine;
using UnityEngine.Events;

public class Wallet : MonoBehaviour
{

    [SerializeField] private float _money;

    public float money => _money;

    [HideInInspector] public UnityEvent<float> OnChanged;

    public void Add(float value)
    {
        if (value <= 0)
            return;

        _money += value;
        OnChanged?.Invoke(_money);
    }

    public void Take(float value)
    {
        if (value <= 0)
            return;

        _money -= value;
        OnChanged?.Invoke(_money);
    }

    public override string ToString()
    {
        return $"{_money}$";
    }

}
