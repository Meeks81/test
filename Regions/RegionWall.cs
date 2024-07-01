using System.Collections.Generic;
using UnityEngine;

public class RegionWall : MonoBehaviour
{

    [SerializeField] private ObjectPool<BuildWall> _wallsPool;
    [Space]
    [SerializeField] private RegionsSystem _regionsSystem;
    [SerializeField] private BuilderGrid _buildGrid;
    [SerializeField] private Map _map;

    private List<Vector3Int> _busyCells;

    public bool CheckWall(BuildWall wall) => _wallsPool.GetObjects().Contains(wall);

    public void SpawnWallsAroundRegion()
    {
        _busyCells = _buildGrid.GetBusyCells();

        foreach (var item in _wallsPool.GetActiveObjects())
            _map.DeleteWall(item, false);

        _wallsPool.HideEverything();
        foreach (var item in _regionsSystem.GetCurrentAreas())
        {
            SpawnArea(item);
        }
    }

    public void SpawnArea(Vector2Int area)
    {
        if (_regionsSystem.CheckSide(area, Vector2Int.up) == false)
            SpawnWall(area.x * _regionsSystem.areaSize.x, (area.x + 1) * _regionsSystem.areaSize.x, (area.y + 1) * _regionsSystem.areaSize.y, true);
        if (_regionsSystem.CheckSide(area, Vector2Int.down) == false)
            SpawnWall(area.x * _regionsSystem.areaSize.x, (area.x + 1) * _regionsSystem.areaSize.x, area.y * _regionsSystem.areaSize.y - 1, true);

        if (_regionsSystem.CheckSide(area, Vector2Int.right) == false)
            SpawnWall(area.y * _regionsSystem.areaSize.y, (area.y + 1) * _regionsSystem.areaSize.y, (area.x + 1) * _regionsSystem.areaSize.x, false);
        if (_regionsSystem.CheckSide(area, Vector2Int.left) == false)
            SpawnWall(area.y * _regionsSystem.areaSize.y, (area.y + 1) * _regionsSystem.areaSize.y, area.x * _regionsSystem.areaSize.x - 1, false);
    }

    private void SpawnWall(int start, int end, int anotherAxis, bool axisX)
    {
        for (int i = start; i < end; i = (int)Mathf.MoveTowards(i, end, 1))
        {
            Vector3Int cell = new Vector3Int(axisX ? i : anotherAxis, axisX ? anotherAxis : i);

            BuildWall wall = _wallsPool.Get();
            wall.transform.position = MapGrid.current.grid.GetCellCenterWorld(cell);
            if (_busyCells.Contains(cell) == false)
                wall.gameObject.SetActive(true);
            _map.AddWall(wall);
        }
    }

}
