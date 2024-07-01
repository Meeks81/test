using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingPermit : MonoBehaviour
{

    [SerializeField] private Tilemap _placedBuildingsTilemap;

    private List<Func<Vector3Int, bool>> _permissiveFunctions = new List<Func<Vector3Int, bool>>();

    public bool IsCellAvailble(Vector3Int cell)
    {
        foreach (var item in _permissiveFunctions)
        {
            if (item(cell) == false)
                return false;
        }

        return true;
    }

    public bool IsBuildingPlaceAvailable(BuildingController buildObject)
    {
        foreach (var item in buildObject.GetCurrentAreaCells())
        {
            if (IsCellAvailble(item) == false)
                return false;
        }

        return true;
    }
    
    public void AddPermissiveFunctions(Func<Vector3Int, bool> function)
    {
        _permissiveFunctions.Add(function);
    }

    //public bool IsCellAvailble(Vector3Int cell)
    //{
    //    Vector2Int area = new Vector2Int(Mathf.CeilToInt((float)cell.x / (float)_registrationSystem.areaSize.x), Mathf.CeilToInt((float)cell.y / (float)_registrationSystem.areaSize.y));
    //    if (_registrationSystem.GetCurrentAreas().Contains(area) == false)
    //        return false;

    //    if (cell.x < (area.x - 1) * _registrationSystem.areaSize.x ||
    //        cell.y < (area.y - 1) * _registrationSystem.areaSize.y || 
    //        cell.x > area.x * _registrationSystem.areaSize.x || 
    //        cell.y > area.y * _registrationSystem.areaSize.y)
    //        return false;

    //    return _placedBuildingsTilemap.GetTile(cell) == null;
    //}

    //public bool IsBuildingPlaceAvailable(BuildingController buildObject)
    //{
    //    foreach (var item in buildObject.GetCurrentAreaCells())
    //    {
    //        if (_placedBuildingsTilemap.GetTile(item) != null)
    //            return false;
    //        if (IsCellAvailble(item) == false)
    //            return false;
    //    }

    //    return true;
    //}

}
