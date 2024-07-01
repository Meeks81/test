using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WallsHiddenButton : Selectable, IPointerClickHandler
{

    [SerializeField] private Image _iconImage;
    [Space]
    [SerializeField] private List<Icon> _icons;

    private WallsHiddenMode _currentMode = WallsHiddenMode.Visible;

    private Camera _mainCamera;

    protected override void Start()
    {
        _mainCamera = Camera.main;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int nextModeIndex = (int)_currentMode + 1;
        if (nextModeIndex >= System.Enum.GetNames(typeof(WallsHiddenMode)).Length)
            nextModeIndex = 0;
        SetWallsMode((WallsHiddenMode)nextModeIndex);
    }

    private void SetWallsMode(WallsHiddenMode mode)
    {
        switch (mode)
        {
            case WallsHiddenMode.Visible:
                ShowMask("Wall");
                break;
            case WallsHiddenMode.Hidden:
                HideMask("Wall");
                break;
            default:
                break;
        }
        _currentMode = mode;

        Icon findedIcon = _icons.Find(e => e.mode == mode);
        if (findedIcon != null)
            _iconImage.sprite = findedIcon.sprite;
    }

    private void ShowMask(string maskName)
    {
        _mainCamera.cullingMask |= 1 << LayerMask.NameToLayer(maskName);
    }

    private void HideMask(string maskName)
    {
        _mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(maskName));
    }

    [System.Serializable]
    private class Icon
    {
        public WallsHiddenMode mode;
        public Sprite sprite;
    }

}
