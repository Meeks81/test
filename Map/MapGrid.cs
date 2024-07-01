using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGrid : MonoBehaviour
{

    [SerializeField] private Grid _grid;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _tileBase;

    public Grid grid => _grid;

    public static MapGrid current { get;private set; }

    private void Awake()
    {
        current = this;
    }

    public static TileBase[] GetTilesBlock(BoundsInt area)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int iteration = 0;

        foreach (var item in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(item.x, item.y, 0);
            array[iteration] = current._tilemap.GetTile(pos);
            iteration++;
        }

        return array;
    }

    public static void FillArea(Vector3Int start, Vector3Int size, Tilemap tilemap)
    {
        tilemap.BoxFill(start, current._tileBase, start.x, start.y, start.x + size.x, start.y + size.y);
    }

}
