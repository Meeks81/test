using HotKeys;
using System;
using System.Collections.Generic;

public class CancelEventSystem
{

    public HotKeyParams HotKeyParams { get; private set; }

    private Action _mainAction;
    private List<Action> _actions;

    internal static CancelEventSystem instance { get; private set; }

    public CancelEventSystem()
    {
        if (instance != null)
            return;

        instance = this;

        _actions = new List<Action>();
        HotKeyParams = new HotKeyParams();
    }

    public static void AddEvent(Action action, bool isMain = false)
    {
        if (isMain)
            instance._mainAction = action;
        else
            instance._actions.Add(action);
    }

    public static void RemoveEvent(Action action)
    {
        if (action == instance._mainAction)
            instance._mainAction = null;
        else
            instance._actions.Remove(action);
    }

    internal void Invoke()
    {
        if (HotKeyParams.isDown == false)
            return;

        if (_actions.Count == 0)
        {
            if (_mainAction != null)
                _mainAction();
        }
        else
        {
            _actions[_actions.Count - 1].Invoke();
        }
    }

}
