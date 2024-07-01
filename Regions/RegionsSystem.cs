using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RegionsSystem : MonoBehaviour
{

    [SerializeField] private Vector2Int _areaSize;
    [SerializeField] private Vector2Int _startAreas;
    [SerializeField] private Vector2Int _maxAreas;
    [Space]
    [SerializeField] private RegionWall _regionWall;
    [SerializeField] private BuildingPermit _buildingPermit;

    public Vector2Int areaSize => _areaSize;
    public Vector2Int maxAreas => _maxAreas;
    public List<RegionData> regions => new List<RegionData>(_regions);

    private Dictionary<Vector2Int, Vector2Int> _currentAreas = new Dictionary<Vector2Int, Vector2Int>();
    private List<RegionData> _regions = new List<RegionData>();
    private RegionData _mainRegion;

    private void Start()
    {
        _mainRegion = new RegionData(Color.white, RegionType.Main, new List<Vector3Int>());
        SpawnStartAreas();
        _regionWall.SpawnWallsAroundRegion();

        _buildingPermit.AddPermissiveFunctions((cell) => _mainRegion.cells.Contains(cell));
    }

    public List<Vector2Int> GetCurrentAreas() => _currentAreas.Keys.ToList();

    public List<Vector2Int> GetAvailableAreasForOpen()
    {
        List<Vector2Int> availableAreas = new List<Vector2Int>();
        for (int x = 0; x < _maxAreas.x; x++)
        {
            for (int y = 0; y < _maxAreas.y; y++)
            {
                Vector2Int area = new Vector2Int(x, y);
                if (_currentAreas.ContainsKey(area))
                    continue;

                if (CheckSide(area, Vector2Int.up) ||
                   CheckSide(area, Vector2Int.down) ||
                   CheckSide(area, Vector2Int.left) ||
                   CheckSide(area, Vector2Int.right))
                {
                    availableAreas.Add(area);
                }
            }
        }

        return availableAreas;
    }

    public void OpenArea(Vector2Int area)
    {
        if (_currentAreas.ContainsKey(area) ||
            area.x >= _maxAreas.x ||
            area.y >= _maxAreas.y)
            return;

        _currentAreas.Add(area, area);
        _mainRegion.AddArea(GetAreaCells(area));
        _regionWall.SpawnWallsAroundRegion();
    }

    public bool CheckSide(Vector2Int area, Vector2Int offset) => _currentAreas.ContainsKey(area + offset);

    public List<Vector3Int> GetAreaCells(Vector2Int area)
    {
        List<Vector3Int> cells = new List<Vector3Int>();

        for (int x = area.x * areaSize.x; x < (area.x + 1) * areaSize.x; x++)
        {
            for (int y = area.y * areaSize.y; y < (area.y + 1) * areaSize.y; y++)
            {
                cells.Add(new Vector3Int(x, y));
            }
        }

        return cells;
    }

    public List<Vector3Int> GetMainRegionCells() => _mainRegion.cells;

    public void AddRegion(RegionData region)
    {
        if (_regions.Contains(region))
            return;

        _regions.Add(region);
    }

    public void RemoveRegion(RegionData region)
    {
        if (_regions.Contains(region) == false)
            return;

        _regions.Remove(region);
    }

    public bool CheckWall(BuildWall wall) => _regionWall.CheckWall(wall);

    private void SpawnStartAreas()
    {
        _currentAreas = new Dictionary<Vector2Int, Vector2Int>();
        for (int x = 0; x < _startAreas.x; x++)
        {
            for (int y = 0; y < _startAreas.y; y++)
            {
                Vector2Int area = new Vector2Int(x, y);
                _currentAreas.Add(area, area);

                _mainRegion.AddArea(GetAreaCells(area));
            }
        }
    }

}
