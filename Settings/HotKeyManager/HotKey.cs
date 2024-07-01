using HotKeys;
using UnityEngine;

[System.Serializable]
public class HotKey
{

    [SerializeField] private string _name;
    [SerializeField] private HotKeyType _type;
    [SerializeField] private KeyCode _keyCode;

    public string name { get => _name; internal set => _name = value; }
    public HotKeyType type { get => _type; internal set => _type = value; }
    public KeyCode keyCode { get => _keyCode; internal set => _keyCode = value; }

    #region Hot Keys By Name

    public static bool GetKeyDown(string name) => HotKeyManager.GetKeyDown(name);
    public static bool GetKey(string name) => HotKeyManager.GetKey(name);
    public static bool GetKeyUp(string name) => HotKeyManager.GetKeyUp(name);

    #endregion

    #region Hot Keys By Type

    public static bool GetKeyDown(HotKeyType type) => HotKeyManager.GetKeyDown(type);
    public static bool GetKey(HotKeyType type) => HotKeyManager.GetKey(type);
    public static bool GetKeyUp(HotKeyType type) => HotKeyManager.GetKeyUp(type);

    #endregion

}
