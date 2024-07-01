using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class StorageArea : MonoBehaviour
{

    private const float VOXEL_SIZE = 0.1f;

    [SerializeField] private int _places;

    public List<CargoBox> boxes => new List<CargoBox>(_boxes);

    private List<CargoBox> _boxes = new List<CargoBox>();
    private BoxCollider _boxCollider;

    private int[,] _grid;
    private int _gridMaxHeight = 0;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnDrawGizmosSelected()
    {
        if (_grid == null)
            return;

        Vector3 startPosition = transform.position - _boxCollider.bounds.size / 2f + _boxCollider.center;
        startPosition += Vector3.one * VOXEL_SIZE / 2f;
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                for (int h = 0; h < _gridMaxHeight; h++)
                {
                    Gizmos.color = _grid[x, y] > h ? new Color(1, 0, 0, 0.5f) : new Color(1, 1, 1, 0);
                    Gizmos.DrawCube(startPosition + new Vector3(x, h, y) * VOXEL_SIZE, Vector3.one * VOXEL_SIZE);
                }
            }
        }
    }

    public void AddBox(CargoBox box)
    {
        if (_boxes.Contains(box))
            return;

        _boxes.Add(box);
        box.transform.position = transform.position;

        PlaceBoxes(_boxes);
    }

    public void RemoveBox(CargoBox box)
    {
        _boxes.Remove(box);
        PlaceBoxes(_boxes);
    }

    public int GetFreePlaceCount(CargoBox box)
    {
        return _places - _boxes.Count;
    }

    public int GetPlaceCount(CargoBox box)
    {
        return _places;
    }

    private Vector3Int WorldToVoxel(Vector3 size)
    {
        return new Vector3Int(Mathf.FloorToInt(size.x / VOXEL_SIZE),
                              Mathf.FloorToInt(size.y / VOXEL_SIZE),
                              Mathf.FloorToInt(size.z / VOXEL_SIZE));
    }

    private void PlaceBoxes(List<CargoBox> boxes)
    {
        Vector3Int gridSize = WorldToVoxel(_boxCollider.bounds.size);
        _gridMaxHeight = gridSize.y;
        _grid = new int[gridSize.x, gridSize.z];

        foreach (var item in boxes)
        {
            List<Vector3Int> result = GetStartFreeVoxelForBox(item);
            Vector3Int startVoxel = result[0];
            Vector3Int boxSize = result[1];
            if (startVoxel == -Vector3Int.one)
            {
                item.transform.position = transform.position;
            }
            else
            {
                Vector3 position = transform.position + new Vector3(startVoxel.x * VOXEL_SIZE, startVoxel.y * VOXEL_SIZE, startVoxel.z * VOXEL_SIZE);
                position -= _boxCollider.bounds.size / 2f - _boxCollider.center;
                position += (Vector3)boxSize * VOXEL_SIZE / 2f + item.GetCollider().center;
                item.transform.position = position;
                for (int x = 0; x < boxSize.x; x++)
                {
                    for (int y = 0; y < boxSize.z; y++)
                    {
                        Vector2Int pos = new Vector2Int(x + startVoxel.x, y + startVoxel.z);
                        if (pos.x < _grid.GetLength(0) && pos.y < _grid.GetLength(0))
                            _grid[pos.x, pos.y] += boxSize.y;
                    }
                }
            }
        }
    }

    private List<Vector3Int> GetStartFreeVoxelForBox(CargoBox box)
    {
        Vector3Int startVoxel = -Vector3Int.one;
        Vector3Int boxSize = WorldToVoxel(box.GetCollider().bounds.size);
        List<Vector3Int> boxSizes = new List<Vector3Int>()
        {
            boxSize,
            new Vector3Int(boxSize.z, boxSize.y, boxSize.x),
            new Vector3Int(boxSize.x, boxSize.z, boxSize.y),
            new Vector3Int(boxSize.y, boxSize.z, boxSize.x),
        };
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                if (_gridMaxHeight - _grid[x, y] < boxSize.y)
                    continue;
                startVoxel = new Vector3Int(x, _grid[x, y], y);

                for (int i = 0; i < boxSizes.Count; i++)
                {
                    if (CheckBoxOnVoxelStart(boxSizes[i], startVoxel))
                    {
                        if (i == 1)
                            box.transform.Rotate(Vector3.up * 90);
                        else if (i == 2)
                            box.transform.Rotate(Vector3.right * 90);
                        else if (i == 3)
                        {
                            box.transform.Rotate(Vector3.right * 90);
                            box.transform.Rotate(Vector3.up * 90);
                        }
                        return new List<Vector3Int>() { startVoxel, boxSizes[i] };
                    }
                }
            }
        }

        return new List<Vector3Int>() { -Vector3Int.one, boxSize };
    }

    private bool CheckBoxOnVoxelStart(Vector3Int boxSize, Vector3Int voxelStart)
    {
        for (int x = 0; x < boxSize.x; x++)
        {
            for (int y = 0; y < boxSize.y; y++)
            {
                Vector2Int pos = new Vector2Int(x + voxelStart.x, y + voxelStart.z);
                if (pos.x >= _grid.GetLength(0) || pos.y >= _grid.GetLength(1))
                    return false;
                if (_gridMaxHeight - _grid[pos.x, pos.y] < boxSize.y || _grid[pos.x, pos.y] < voxelStart.y)
                    return false;
            }
        }

        return true;
    }

}
