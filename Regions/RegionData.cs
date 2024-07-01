using System.Collections.Generic;
using UnityEngine;

public class RegionData
{

    public Color color => _color;
    public RegionType type => _type;
    public List<Vector3Int> cells => new List<Vector3Int>(_cells);

    private Color _color;
    public RegionType _type;
    private List<Vector3Int> _cells;

    public RegionData(Color color, RegionType type, List<Vector3Int> cells)
    {
        _color = color;
        _type = type;
        _cells = cells;
    }

    public void AddArea(List<Vector3Int> cells)
    {
        foreach (var item in cells)
        {
            if (_cells.Contains(item))
                continue;

            _cells.Add(item);
        }
    }

    public void RemoveArea(List<Vector3Int> cells)
    {
        foreach (var item in cells)
        {
            _cells.Remove(item);
        }
    }

}
