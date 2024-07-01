using UnityEngine;

public class HotKeysMenu : MonoBehaviour
{

    [SerializeField] private HotKeyChanger _hotKeyChanger;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _hotKeyChanger.gameObject.activeSelf)
        {
            CloseHotKeyChanger();
        }
    }

    public void ChangeHotKey(HotKeyField field)
    {
        _hotKeyChanger.gameObject.SetActive(true);
        _hotKeyChanger.SetHotKey(field.GetHotKey(), (keyCode) =>
        {
            field.UpdateHotKey();
        });
    }

    public void CloseHotKeyChanger()
    {
        _hotKeyChanger.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        CloseHotKeyChanger();
    }

    private void OnDisable()
    {
        CloseHotKeyChanger();
    }

}
