using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RegionsGrid : MonoBehaviour
{

    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _tileBase;
    [Space]
    [SerializeField] private RegionsSystem _regionsSystem;
    [SerializeField] private RegionMode _regionMode;

    private void OnEnable()
    {
        _regionMode.OnChanged.AddListener(OnRegionModeChanged);
    }

    private void OnRegionModeChanged(bool isActive)
    {
        if (isActive)
        {
            UpdateRegionsTilemap();
        }
        else
        {
            _tilemap.ClearAllTiles();
        }
    }

    private void UpdateRegionsTilemap()
    {
        _tilemap.ClearAllTiles();

        foreach (var cell in _regionsSystem.GetMainRegionCells())
        {
            SetTile(cell, Color.white);
        }
        foreach (var region in _regionsSystem.regions)
        {
            foreach (var cell in region.cells)
            {
                SetTile(cell, region.color);
            }
        }
    }

    private void SetTile(Vector3Int cell, Color color)
    {
        _tilemap.SetTile(cell, _tileBase);
        _tilemap.SetColor(cell, color);
    }

}
