using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildWall : BuildingController
{

    public override bool IsEdit => false;
    //public override Vector3 LastPosition => transform.position;
    //public override Quaternion LastRotation => transform.rotation;

    private List<Vector3Int> _areaCells = new List<Vector3Int>();
    private Vector3Int _lastCell;

    private bool _isExpand = false;
    private bool _axisX = false;
    private Vector3Int _startExpandCell;
    private ObjectPool<BuildWall> _cloneWalls;

    private UnityAction<BuildingController[]> _onPlace;
    private UnityAction _onCancel;

    public Vector3Int GetCellPosition() => MapGrid.current.grid.WorldToCell(transform.position);

    public override Vector3 GetSize() => MapGrid.current.grid.cellSize;
    public override Vector3Int[] GetCurrentAreaCells() => _areaCells.ToArray();
    public override List<Vector3Int> GetAreaCells(Vector3 position, Quaternion rotation)
    {
        return new List<Vector3Int>() { MapGrid.current.grid.WorldToCell(position) };
    }

    public override void Init(UnityAction<BuildingController[]> onPlace, UnityAction onCancel)
    {
        _cloneWalls = new ObjectPool<BuildWall>(this, null, 0, true);

        _onPlace = onPlace;
        _onCancel = onCancel;
    }

    public override void MouseDown(Vector3 position)
    {
        if (_isExpand)
        {
            _isExpand = false;
            List<BuildWall> walls = new List<BuildWall>() { this };
            walls.AddRange(_cloneWalls.GetActiveObjects());
            _onPlace(walls.ToArray());
            if (_cloneWalls != null)
            {
                _cloneWalls.Dispose();
                _cloneWalls = null;
            }
        }
        else
        {
            _isExpand = true;
            _startExpandCell = MapGrid.current.grid.WorldToCell(position);
        }
    }

    public override void Cancel()
    {
        if (_isExpand)
        {
            _isExpand = false;
            _cloneWalls.HideEverything();
            _areaCells = new List<Vector3Int>() { _areaCells[0] };
        }
        else
        {
            _cloneWalls.HideEverything();
            _cloneWalls.Dispose();
            _onCancel();
        }
    }

    //public override void Place()
    //{
    //    _isExpand = false;
    //    _areaCells = new List<Vector3Int>() { _areaCells[0] };

    //    if (_cloneWalls != null)
    //    {
    //        _cloneWalls.Dispose();
    //        _cloneWalls = null;
    //    }
    //}

    public override void MouseDrag(Vector3 position)
    {

    }

    public override void MouseMove(Vector3 position)
    {
        Vector3Int cell = MapGrid.current.grid.WorldToCell(position);

        if (cell != _lastCell)
        {
            if (_isExpand)
            {
                transform.position = MapGrid.current.grid.GetCellCenterWorld(_startExpandCell);
                _areaCells = new List<Vector3Int>() { _startExpandCell };

                int startX = _startExpandCell.x;
                int startY = _startExpandCell.y;
                int endX = cell.x;
                int endY = cell.y;

                Vector2Int size = new Vector2Int(Mathf.Abs(endX - startX), Mathf.Abs(endY - startY));

                if (size.x < 5 && size.y < 5)
                    _axisX = size.x > size.y;

                Expand(startX, startY, endX, endY, _axisX);
            }
            else
            {
                transform.position = MapGrid.current.grid.GetCellCenterWorld(cell);
                _areaCells = new List<Vector3Int>() { cell };
            }

            _lastCell = cell;
        }
    }

    public override void MouseUp(Vector3 position)
    {

    }

    public override void Rotate()
    {

    }

    private void Expand(int startX, int startY, int endX, int endY, bool axisX)
    {
        int x = startX;
        int y = startY;

        _cloneWalls.HideEverything();
        while (x != endX || y != endY)
        {
            if (axisX)
            {
                if (x != endX)
                    x = (int)Mathf.MoveTowards(x, endX, 1);
                else
                    y = (int)Mathf.MoveTowards(y, endY, 1);
            }
            else
            {
                if (y != endY)
                    y = (int)Mathf.MoveTowards(y, endY, 1);
                else
                    x = (int)Mathf.MoveTowards(x, endX, 1);
            }

            CloneWall(x, y);
            _areaCells.Add(new Vector3Int(x, y));
        }
    }

    private void CloneWall(int x, int y) => CloneWall(new Vector3Int(x, y));
    private void CloneWall(Vector3Int cell)
    {
        BuildWall clone = _cloneWalls.Get();
        clone.transform.position = MapGrid.current.grid.GetCellCenterWorld(cell);
        clone._areaCells = new List<Vector3Int>() { cell };

        clone.gameObject.SetActive(true);
    }

}
