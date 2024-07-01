using System.Collections.Generic;
using UnityEngine;

public class ScreenResolutionSettings : MonoBehaviour
{

    [SerializeField] private SelectorUI _resolutionSelector;
    [SerializeField] private SelectorUI _fullScreenSelector;

    private List<Vector2Int> _availableResolutions;

    private void OnEnable()
    {
        InitializeScreenResolutionSelector();
        InitializeFullScreenSelector();
    }

    public void ApplySettings()
    {
        Vector2Int resolution = _availableResolutions[_resolutionSelector.currentIndex];
        Screen.SetResolution(resolution.x, resolution.y, (FullScreenMode)_fullScreenSelector.currentIndex);
    }

    private void InitializeScreenResolutionSelector()
    {
        Resolution[] resolutions = Screen.resolutions;
        _availableResolutions = new List<Vector2Int>();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (_availableResolutions.Contains(new Vector2Int(resolutions[i].width, resolutions[i].height)))
                continue;

            options.Add($"{resolutions[i].width}x{resolutions[i].height}");
            _availableResolutions.Add(new Vector2Int(resolutions[i].width, resolutions[i].height));

            if (Screen.width == resolutions[i].width &&
                Screen.height == resolutions[i].height)
            {
                currentResolutionIndex = options.Count - 1;
            }
        }
        _resolutionSelector.SetOptions(options);
        _resolutionSelector.SetIndex(currentResolutionIndex);
    }

    private void InitializeFullScreenSelector()
    {
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < System.Enum.GetNames(typeof(FullScreenMode)).Length; i++)
        {
            options.Add(GetScreenModeName((FullScreenMode)i));

            if (Screen.fullScreenMode == (FullScreenMode)i)
            {
                currentResolutionIndex = i;
            }
        }
        _fullScreenSelector.SetOptions(options);
        _fullScreenSelector.SetIndex(currentResolutionIndex);
    }

    private string GetScreenModeName(FullScreenMode screenMode)
    {
        switch (screenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                return "Полный экран";
            case FullScreenMode.FullScreenWindow:
                return "Полный экран";
            case FullScreenMode.MaximizedWindow:
                return "Окно на весь экран";
            case FullScreenMode.Windowed:
                return "Оконный";
            default:
                return screenMode.ToString();
        }
    }

}
