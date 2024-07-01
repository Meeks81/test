using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GhostBuilding : BuildingController
{

    public override bool IsEdit => false;
    //public override Vector3 LastPosition => transform.position;
    //public override Quaternion LastRotation => transform.rotation;

    private List<Vector3Int> _areaCells;
    private BuildingController _building;

    public void Initilize(BuildingController building, Vector3 position, Quaternion rotation)
    {
        _areaCells = building.GetAreaCells(position, rotation);
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = new Vector3(building.GetSize().x, transform.localScale.y, building.GetSize().y);
    }

    public override void Cancel()
    {

    }

    public override List<Vector3Int> GetAreaCells(Vector3 position, Quaternion rotation) => _building == null ? new List<Vector3Int>() : _building.GetAreaCells(position, rotation);
    public override Vector3Int[] GetCurrentAreaCells() => _areaCells.ToArray();
    public override Vector3 GetSize() => transform.localScale;

    public override void Init(UnityAction<BuildingController[]> onPlace, UnityAction onCancel)
    {

    }

    public override void MouseDown(Vector3 position)
    {

    }

    public override void MouseDrag(Vector3 position)
    {

    }

    public override void MouseMove(Vector3 position)
    {

    }

    public override void MouseUp(Vector3 position)
    {

    }

    public override void Rotate()
    {

    }
}
