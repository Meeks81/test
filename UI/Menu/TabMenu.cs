using System.Collections.Generic;
using UnityEngine;

public class TabMenu : MonoBehaviour
{

    [SerializeField] private List<TabButton> _tabButtons;
    [SerializeField] private int _defaultTabIndex;

    public TabButton SelectedTab { get; private set; }

    private void OnEnable()
    {
        if (_defaultTabIndex >= 0 && _defaultTabIndex < _tabButtons.Count)
            OpenTab(_tabButtons[_defaultTabIndex]);
    }

    public void OpenTab(TabButton tabButton)
    {
        if (_tabButtons.Contains(tabButton) == false)
            throw new System.Exception("This panel is not in the list");

        CloseAllPanels();

        if (tabButton.panel != null)
            tabButton.panel.SetActive(true);

        SelectedTab = tabButton;
        tabButton.UpdateColor();
    }

    public void CloseAllPanels()
    {
        SelectedTab = null;
        foreach (var item in _tabButtons)
        {
            if (item.panel != null)
                item.panel.SetActive(false);
            item.UpdateColor();
        }
    }

}
