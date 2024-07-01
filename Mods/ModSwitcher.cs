using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ModSwitcher
{

    public string name;

    [SerializeField] private UnityEvent _tunrOn;
    [SerializeField] private UnityEvent _tunrOff;

    public bool isActive { get; private set; }

    public void TurnOn()
    {
        if (isActive == true)
            return;

        isActive = true;
        _tunrOn.Invoke();
    }

    public void TurnOff()
    {
        if (isActive == false)
            return;

        isActive = false;
        _tunrOff.Invoke();
    }

}
