using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{

    private List<BuildingController> _buildObjects = new List<BuildingController>();
    private List<BuildWall> _wallObjects = new List<BuildWall>();
    private Dictionary<Vector3Int, BuildWall> _wallsPositions = new Dictionary<Vector3Int, BuildWall>();

    public List<BuildingController> BuildObjects => new List<BuildingController>(_buildObjects);
    public List<BuildWall> WallObjects => new List<BuildWall>(_wallObjects);

    public UnityEvent<BuildingController> OnAddBuilding;
    public UnityEvent<BuildingController> OnDeleteBuilding;

    public void AddBuilding(BuildingController building)
    {
        if (building.TryGetComponent(out BuildWall wall))
        {
            AddWall(wall);
        }
        else
        {
            if (_buildObjects.Contains(building))
                return;

            _buildObjects.Add(building);

            OnAddBuilding.Invoke(building);
        }
    }

    public void DeleteBuilding(BuildingController building, bool deleteObject = true)
    {
        if (building.TryGetComponent(out BuildWall wall))
        {
            DeleteWall(wall);
        }
        else
        {
            if (_buildObjects.Contains(building) == false)
                return;

            _buildObjects.Remove(building);

            if (deleteObject)
                Destroy(building.gameObject);

            OnDeleteBuilding.Invoke(building);
        }
    }

    public void AddWall(BuildWall wall)
    {
        if (_wallObjects.Contains(wall))
            return;

        _wallObjects.Add(wall);
        if (_wallsPositions.ContainsKey(wall.GetCellPosition()))
            _wallsPositions[wall.GetCellPosition()] = wall;
        else
            _wallsPositions.Add(wall.GetCellPosition(), wall);

        OnAddBuilding.Invoke(wall);
    }

    public void DeleteWall(BuildWall wall, bool deleteObject = true)
    {
        if (_wallObjects.Contains(wall) == false)
            return;

        _wallObjects.Remove(wall);
        _wallsPositions.Remove(wall.GetCellPosition());

        if (deleteObject)
            Destroy(wall.gameObject);

        OnDeleteBuilding.Invoke(wall);
    }

    public BuildWall GetWall(Vector3Int cell) => _wallsPositions.ContainsKey(cell) ? _wallsPositions[cell] : null;

}
