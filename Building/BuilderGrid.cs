using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuilderGrid : MonoBehaviour
{

    [SerializeField] private Tilemap _placedBuildingsTilemap;
    [SerializeField] private Tilemap _flyingBuildingTilemap;
    [SerializeField] private TileBase _placedTile;
    [SerializeField] private TileBase _avaialbleTile;
    [SerializeField] private TileBase _unavailableTile;
    [Space]
    [SerializeField] private Builder _builder;
    [SerializeField] private Map _map;
    [SerializeField] private BuildingPermit _buildingPermit;

    private void Start()
    {
        _buildingPermit.AddPermissiveFunctions((cell) => _placedBuildingsTilemap.GetTile(cell) == null);

        _builder.OnFlyingBuildingChanged.AddListener(OnFlyingBuildingChanged);
        _builder.OnBuildingEdited.AddListener(OnBuildingEdited);
        _builder.OnFlyingBuildingMoved.AddListener(OnFlyingBuildingMoved);
        _builder.OnModeChanged.AddListener(OnModeChanged);
        _map.OnAddBuilding.AddListener((building) => UpdatePlacedTilemap());
        _map.OnDeleteBuilding.AddListener((building) => UpdatePlacedTilemap());
    }

    public List<Vector3Int> GetBusyCells()
    {
        List<Vector3Int> cells = new List<Vector3Int>();

        _placedBuildingsTilemap.ClearAllTiles();
        foreach (var building in _map.BuildObjects)
        {
            if (building == _builder.FlyingBuilding)
                continue;

            foreach (var item in building.GetCurrentAreaCells())
                cells.Add(item);
        }

        return cells;
    }

    private void OnFlyingBuildingChanged(BuildingController building)
    {
        UpdatePlacedTilemap();
    }

    private void OnBuildingEdited(BuildingController building)
    {
        _flyingBuildingTilemap.ClearAllTiles();
        UpdatePlacedTilemap();
            building.transparentBuilding.SetNormal();
    }

    private void OnFlyingBuildingMoved(BuildingController building)
    {
        Vector3Int[] area = building.GetCurrentAreaCells();
        _flyingBuildingTilemap.ClearAllTiles();
        FillArea(area, _flyingBuildingTilemap, (x, y) => _placedBuildingsTilemap.GetTile(new Vector3Int(x, y)) == null ? _avaialbleTile : _unavailableTile);

        if (_buildingPermit.IsBuildingPlaceAvailable(building))
            building.transparentBuilding.SetNormal();
        else
            building.transparentBuilding.SetTransparent(Color.red, 0.6f);
    }

    private void OnModeChanged(bool isMode)
    {
        if (isMode)
        {
            UpdatePlacedTilemap();
        }
        else
        {
            _placedBuildingsTilemap.ClearAllTiles();
        }
    }

    private void UpdatePlacedTilemap()
    {
        if (_builder.IsModeActive == false)
            return;

        _placedBuildingsTilemap.ClearAllTiles();
        foreach (var item in _map.BuildObjects)
        {
            if (item == _builder.FlyingBuilding)
                continue;

            FillArea(item.GetCurrentAreaCells(), _placedBuildingsTilemap, (x, y) => _placedTile);
        }
    }

    private void FillArea(Vector3Int[] area, Tilemap tilemap, Func<int, int, TileBase> tile)
    {
        if (_builder.IsModeActive == false)
            return;

        foreach (var item in area)
        {
            tilemap.SetTile(item, tile(item.x, item.y));
        }
    }

}
