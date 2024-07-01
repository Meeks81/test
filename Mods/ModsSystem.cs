using System.Collections.Generic;
using UnityEngine;

public class ModsSystem : MonoBehaviour
{

    [SerializeField] private List<ModSwitcher> _modSwitchers;

    public void TurnOn(string modName)
    {
        ModSwitcher findedMod = _modSwitchers.Find(e => e.name == modName);
        if (findedMod == null)
            return;

        TurnOffAllMods();
        findedMod.TurnOn();
        CancelEventSystem.AddEvent(TurnOffAllMods);
    }

    public void TurnOff(string modName)
    {
        ModSwitcher findedMod = _modSwitchers.Find(e => e.name == modName);
        if (findedMod == null)
            return;

        findedMod.TurnOff();
    }

    public void TurnOffAllMods()
    {
        foreach (var item in _modSwitchers)
            item.TurnOff();

        CancelEventSystem.RemoveEvent(TurnOffAllMods);
    }

}
