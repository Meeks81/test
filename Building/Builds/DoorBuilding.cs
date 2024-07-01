using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorBuilding : BuildingController
{

    public const int MIN_SIZE = 4;
    public const int MAX_SIZE = 8;

    [SerializeField] private Transform _columnStart;
    [SerializeField] private Transform _columnEnd;
    [SerializeField] private ObjectPool<Transform> _topColumnPool;

    public override bool IsEdit => false;
    //public override Vector3 LastPosition => transform.position;
    //public override Quaternion LastRotation => transform.rotation;

    private List<Vector3Int> _areaCells = new List<Vector3Int>();

    private bool _isExpand = false;
    private Vector3Int _startExpandCell;
    private Vector3Int _offset;

    private UnityAction<BuildingController[]> _onPlace;
    private UnityAction _onCancel;

    public override void Init(UnityAction<BuildingController[]> onPlace, UnityAction onCancel)
    {
        _onPlace = onPlace;
        _onCancel = onCancel;

        _columnStart.gameObject.SetActive(false);
        _columnEnd.gameObject.SetActive(false);
    }

    public override Vector3 GetSize() => new Vector3(MapGrid.current.grid.cellSize.x * (_offset.x == 0 ? 1 : _offset.x), MapGrid.current.grid.cellSize.y * (_offset.y == 0 ? 1 : _offset.y));
    public override Vector3Int[] GetCurrentAreaCells() => _areaCells.ToArray();
    public override List<Vector3Int> GetAreaCells(Vector3 position, Quaternion rotation)
    {
        Vector2 zonePlace = Helper.GetVectorByAngle(Helper.GetAngleByDirection(new Vector2(_offset.x, _offset.y)) + 90);
        Vector3Int centerCell = MapGrid.current.grid.WorldToCell(position);
        Vector3Int startCell = centerCell + new Vector3Int((int)zonePlace.x, (int)zonePlace.y) * 2;
        Vector3Int endCell = centerCell + _offset - new Vector3Int((int)zonePlace.x, (int)zonePlace.y) * 2;

        List<Vector3Int> areaCells = new List<Vector3Int>();

        int x = startCell.x;
        int y = startCell.y;
        for (x = startCell.x; true; x = (int)Mathf.MoveTowards(x, endCell.x, 1))
        {
            for (y = startCell.y; true; y = (int)Mathf.MoveTowards(y, endCell.y, 1))
            {
                areaCells.Add(new Vector3Int(x, y));

                if (y == endCell.y)
                    break;
            }
            if (x == endCell.x)
                break;
        }

        return areaCells;
    }

    public override void MouseDown(Vector3 position)
    {
        if (_isExpand)
        {
            if (_offset.magnitude >= MIN_SIZE)
            {
                _onPlace(new BuildingController[] { this });
                _isExpand = false;
            }
        }
        else
        {
            _isExpand = true;
            _startExpandCell = MapGrid.current.grid.WorldToCell(position);

            _columnStart.gameObject.SetActive(true);
            _columnEnd.gameObject.SetActive(true);
        }
    }

    public override void MouseDrag(Vector3 position)
    {

    }

    public override void MouseMove(Vector3 position)
    {
        Vector3Int cell = MapGrid.current.grid.WorldToCell(position);

        if (_isExpand)
        {
            transform.position = MapGrid.current.grid.GetCellCenterWorld(_startExpandCell);

            Vector3Int start = _startExpandCell;
            Vector3Int end = cell;

            Vector3Int size = new Vector3Int(Mathf.Abs(end.x - start.x), Mathf.Abs(end.y - start.y));

            if (size.x > size.y)
            {
                end.y = start.y;
                if (size.x > MAX_SIZE)
                    end.x = (int)Mathf.MoveTowards(start.x, end.x, MAX_SIZE);
            }
            else
            {
                end.x = start.x;
                if (size.y > MAX_SIZE)
                    end.y = (int)Mathf.MoveTowards(start.y, end.y, MAX_SIZE);
            }

            _offset = end - start;
            _columnEnd.position = MapGrid.current.grid.GetCellCenterWorld(end);
            SpawnTopColumns(start, end);
            _areaCells = GetAreaCells(transform.position, transform.rotation);
        }
        else
        {
            transform.position = MapGrid.current.grid.GetCellCenterWorld(cell);
            _areaCells = new List<Vector3Int>() { cell };
        }
    }

    public override void MouseUp(Vector3 position)
    {

    }

    public override void Rotate()
    {

    }

    public override void Cancel()
    {
        _onCancel();
    }

    //public override void Place()
    //{
    //    _isExpand = false;
    //}

    private void SpawnTopColumns(Vector3Int start, Vector3Int end) => SpawnTopColumns(start.x, start.y, end.x, end.y);
    private void SpawnTopColumns(int startX, int startY, int endX, int endY)
    {
        int x = startX;
        int y = startY;

        _topColumnPool.HideEverything();
        while (x != endX || y != endY)
        {
            if (x != endX)
                x = (int)Mathf.MoveTowards(x, endX, 1);
            else
                y = (int)Mathf.MoveTowards(y, endY, 1);

            Transform column = _topColumnPool.Get();
            column.position = MapGrid.current.grid.GetCellCenterWorld(new Vector3Int(x, y));

            column.gameObject.SetActive(true);
        }
    }

    //private void FillArea(Vector3Int startCell, Vector3Int endCell)
    //{
    //    int x = startCell.x;
    //    int y = startCell.y;
    //    for (x = startCell.x; true; x = (int)Mathf.MoveTowards(x, endCell.x, 1))
    //    {
    //        for (y = startCell.y; true; y = (int)Mathf.MoveTowards(y, endCell.y, 1))
    //        {
    //            _areaCells.Add(new Vector3Int(x, y));

    //            if (y == endCell.y)
    //                break;
    //        }
    //        if (x == endCell.x)
    //            break;
    //    }
    //}
}
