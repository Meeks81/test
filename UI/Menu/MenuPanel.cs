using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        CancelEventSystem.AddEvent(Close);
    }

    private void OnDisable()
    {
        CancelEventSystem.RemoveEvent(Close);
    }

}
