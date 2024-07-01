using UnityEngine;
using UnityEngine.Events;

public class RegionMode : MonoBehaviour
{

    [SerializeField] private ObjectPool<RegionAreaTrigger> _openRegionButtonsPool;
    [Space]
    [SerializeField] private RegionsSystem _regionsSystem;
    [SerializeField] private MoveCamera _moveCamera;

    public bool IsActive { get; private set; }

    [HideInInspector] public UnityEvent<bool> OnChanged;

    public void OpenMode()
    {
        UpdateRegionButtons();
        IsActive = true;

        OnChanged?.Invoke(IsActive);

        _moveCamera.SetCustomSettings(70f, 88f);
    }

    public void CloseMode()
    {
        _openRegionButtonsPool.HideEverything();
        IsActive = false;

        OnChanged?.Invoke(IsActive);

        _moveCamera.TurnCustomSettingsOff();
    }

    private void OpenArea(Vector2Int area)
    {
        _regionsSystem.OpenArea(area);
        UpdateRegionButtons();
    }

    private void UpdateRegionButtons()
    {
        _openRegionButtonsPool.HideEverything();

        foreach (var item in _regionsSystem.GetAvailableAreasForOpen())
        {
            SpawnRegionButton(item);
        }
    }

    private void SpawnRegionButton(Vector2Int area)
    {
        RegionAreaTrigger regionButton = _openRegionButtonsPool.Get();

        Vector2Int cell = area * _regionsSystem.areaSize;
        regionButton.area = area;
        regionButton.transform.position = MapGrid.current.grid.CellToWorld((Vector3Int)cell) +
            MapGrid.current.grid.CellToWorld((Vector3Int)_regionsSystem.areaSize) * 0.5f;

        regionButton.onClick.RemoveListener(OpenArea);
        regionButton.onClick.AddListener(OpenArea);

        regionButton.gameObject.SetActive(true);
    }

}
