using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class BuildObject : BuildingController
{

    public override bool IsEdit => true;
    //public override Vector3 LastPosition => _lastPosition;
    //public override Quaternion LastRotation => _lastRotation;

    private UnityAction<BuildingController[]> _onPlace;
    private UnityAction _onCancel;

    private List<Vector3Int> _areaCells = new List<Vector3Int>();
    private Vector3Int _lastCell;
    private BoxCollider _boxCollider;

    //private Vector3 _lastPosition;
    //private Quaternion _lastRotation;

    public override void Init(UnityAction<BuildingController[]> onPlace, UnityAction onCancel)
    {
        if (_boxCollider == null)
            _boxCollider = GetComponent<BoxCollider>();

        _onPlace = onPlace;
        _onCancel = onCancel;
    }

    public override Vector3 GetSize() => _boxCollider.size;
    public override Vector3Int[] GetCurrentAreaCells()
    {
        return GetAreaCells(transform.position, transform.rotation).ToArray();
        //return _areaCells.ToArray();
    }
    public override List<Vector3Int> GetAreaCells(Vector3 position, Quaternion rotation)
    {
        Vector3Int startCell = MapGrid.current.grid.WorldToCell(GetStartPoint(position, rotation, GetSize()));
        Vector3Int endCell = MapGrid.current.grid.WorldToCell(GetEndPoint(position, rotation, GetSize()));

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

    public Vector3 GetOffset()
    {
        if (_boxCollider == null)
            _boxCollider = GetComponent<BoxCollider>();

        return _boxCollider.center - _boxCollider.size * 0.5f;
    }

    public override void Rotate()
    {
        transform.Rotate(new Vector3(0, 90f, 0));
        UpdateArea();
    }

    public override void Cancel()
    {
        //transform.position = _lastPosition;
        //transform.rotation = _lastRotation;
        //_areaCells = GetAreaCells(_lastPosition, _lastRotation);

        _onCancel();
    }

    //public override void Place()
    //{
    //    _lastPosition = transform.position;
    //    _lastRotation = transform.rotation;
    //    _areaCells = GetAreaCells(transform.position, transform.rotation);
    //}

    public override void MouseDown(Vector3 position)
    {
        _onPlace(new BuildingController[] { this });
    }

    public override void MouseUp(Vector3 position)
    {

    }

    public override void MouseDrag(Vector3 position)
    {

    }

    public override void MouseMove(Vector3 position)
    {
        transform.position = position;
        UpdateArea();
    }

    private Vector3 GetStartPoint() => transform.TransformPoint(GetOffset());
    private Vector3 GetEndPoint() => transform.TransformPoint(-GetOffset());

    private Vector3 GetStartPoint(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        return rotation * Vector3.Scale(Vector3.one * 0.5f, localScale) + position;
    }
    private Vector3 GetEndPoint(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        return rotation * Vector3.Scale(-Vector3.one * 0.5f, localScale) + position;
    }

    private void UpdateArea()
    {
        Vector3Int startCell = MapGrid.current.grid.WorldToCell(GetStartPoint());
        Vector3Int endCell = MapGrid.current.grid.WorldToCell(GetEndPoint());

        Vector3Int checkCell = startCell + endCell + new Vector3Int(0, (int)transform.eulerAngles.y, 0);
        if (checkCell != _lastCell)
        {
            _areaCells = GetAreaCells(transform.position, transform.rotation);

            _lastCell = checkCell;
        }
    }
}
