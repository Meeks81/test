using UnityEngine;

public class GraphicsQualitySettings : MonoBehaviour
{

    [SerializeField] private SelectorUI _qualitySelector;

    private void Start()
    {
        _qualitySelector.SetIndex(QualitySettings.GetQualityLevel());
        _qualitySelector.onChange.AddListener(ChangeQualityLevel);
    }

    private void ChangeQualityLevel(int level)
    {
        QualitySettings.SetQualityLevel(level);
    }

}
