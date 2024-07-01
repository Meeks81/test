using System.Collections.Generic;
using UnityEngine;

public class RegionsMenu : MonoBehaviour
{

    [SerializeField] private List<RegionIcon> _regionIcons;
    [Space]
    [SerializeField] private MenuUI _menuUI;

    public void OpenRegions()
    {
        _menuUI.ClearButtons();

        foreach (var item in _regionIcons)
        {
            _menuUI.AddButton(item.icon, () => { });
        }
    }

    public void CloseRegions()
    {
        _menuUI.ClearButtons();
    }

    [System.Serializable]
    private class RegionIcon
    {
        public RegionType type;
        public Sprite icon;
    }

}
