using System.Collections.Generic;
using UnityEngine;

namespace HotKeys
{
    public class HotKeyManager : MonoBehaviour
    {

        [SerializeField] private List<HotKey> _hotKeys;

        public static HotKeyManager instance { get; private set; }

        private Dictionary<HotKey, HotKeyParams> _hotKeysParams;

        private Dictionary<string, HotKey> _hotKeysByName;
        private Dictionary<HotKeyType, HotKey> _hotKeysByType;
        private Dictionary<KeyCode, HotKey> _hotKeysByKey;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }

            DontDestroyOnLoad(gameObject);

            Initialize();
        }

        public static HotKey GetHotKey(string name)
        {
            if (instance._hotKeysByName.ContainsKey(name) == false)
                return null;

            return instance._hotKeysByName[name];
        }
        public static HotKey GetHotKey(HotKeyType type)
        {
            if (instance._hotKeysByType.ContainsKey(type) == false)
                return null;

            return instance._hotKeysByType[type];
        }

        #region Change Hot Key

        public static bool ChangeHotKeyName(HotKey hotKey, string newName)
        {
            if (instance._hotKeysByName.ContainsKey(newName))
                return false;

            instance._hotKeysByName.Remove(hotKey.name);
            hotKey.name = newName;
            instance._hotKeysByName.Add(newName, hotKey);

            return true;
        }
        public static bool ChangeHotKeyType(HotKey hotKey, HotKeyType newType)
        {
            if (instance._hotKeysByType.ContainsKey(newType))
                return false;

            instance._hotKeysByType.Remove(hotKey.type);
            hotKey.type = newType;
            instance._hotKeysByType.Add(newType, hotKey);

            return true;
        }
        public static bool ChangeHotKeyCode(HotKey hotKey, KeyCode newKeyCode)
        {
            if (instance._hotKeysByKey.ContainsKey(newKeyCode))
                return false;

            instance._hotKeysByKey.Remove(hotKey.keyCode);
            hotKey.keyCode = newKeyCode;
            instance._hotKeysByKey.Add(newKeyCode, hotKey);

            return true;
        }

        #endregion

        #region Hot Keys By Name

        public static bool GetKeyDown(string name)
        {
            if (instance._hotKeysByName.ContainsKey(name) == false)
                return false;

            return GetParams(instance._hotKeysByName[name]).isDown;
        }
        public static bool GetKey(string name)
        {
            if (instance._hotKeysByName.ContainsKey(name) == false)
                return false;

            return GetParams(instance._hotKeysByName[name]).isPressed;
        }
        public static bool GetKeyUp(string name)
        {
            if (instance._hotKeysByName.ContainsKey(name) == false)
                return false;

            return GetParams(instance._hotKeysByName[name]).isUp;
        }

        #endregion
        #region Hot Keys By Type

        public static bool GetKeyDown(HotKeyType type)
        {
            if (instance._hotKeysByType.ContainsKey(type) == false)
                return false;

            return GetParams(instance._hotKeysByType[type]).isDown;
        }
        public static bool GetKey(HotKeyType type)
        {
            if (instance._hotKeysByType.ContainsKey(type) == false)
                return false;

            return GetParams(instance._hotKeysByType[type]).isPressed;
        }
        public static bool GetKeyUp(HotKeyType type)
        {
            if (instance._hotKeysByType.ContainsKey(type) == false)
                return false;

            return GetParams(instance._hotKeysByType[type]).isUp;
        }

        #endregion

        private static HotKeyParams GetParams(HotKey hotKey)
        {
            if (instance._hotKeysParams.ContainsKey(hotKey) == false)
                return new HotKeyParams();

            return instance._hotKeysParams[hotKey];
        }

        private void Initialize()
        {
            instance = this;
            new CancelEventSystem();

            _hotKeysParams = new Dictionary<HotKey, HotKeyParams>();
            _hotKeysByName = new Dictionary<string, HotKey>();
            _hotKeysByType = new Dictionary<HotKeyType, HotKey>();
            _hotKeysByKey = new Dictionary<KeyCode, HotKey>();

            foreach (var item in _hotKeys)
            {
                _hotKeysParams.Add(item, new HotKeyParams());
                _hotKeysByName.Add(item.name, item);
                _hotKeysByType.Add(item.type, item);
                _hotKeysByKey.Add(item.keyCode, item);
            }
        }

        private void OnGUI()
        {
            if (Event.current.isKey)
            {
                if (Event.current.keyCode == KeyCode.Escape)
                {
                    CancelEventSystem.instance.HotKeyParams.Set(Event.current.type);
                    CancelEventSystem.instance.Invoke();
                }

                if (_hotKeysByKey.ContainsKey(Event.current.keyCode) == false)
                    return;

                HotKey hotKey = _hotKeysByKey[Event.current.keyCode];
                HotKeyParams hotKeyParams = GetParams(hotKey);
                if (hotKey != null)
                    hotKeyParams.Set(Event.current.type);
            }
        }

    }
}