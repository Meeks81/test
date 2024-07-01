using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TransparentBuilding))]
public abstract class BuildingController : MonoBehaviour
{

    [SerializeField] private BuildingPack _packPrefab;
    [SerializeField] private bool _onlyWalls;
    [SerializeField] private bool _onlyRegionWalls;

    public abstract bool IsEdit { get; }
    public bool OnlyWalls => _onlyWalls;
    public bool OnlyRegionWalls => _onlyRegionWalls;
    //public abstract Vector3 LastPosition { get; }
    //public abstract Quaternion LastRotation { get; }
    public BuildingPack PackPrefab => _packPrefab;

    private TransparentBuilding _transparentBuilding;
    public TransparentBuilding transparentBuilding => _transparentBuilding ??= GetComponent<TransparentBuilding>();

    public abstract void Init(UnityAction<BuildingController[]> onPlace, UnityAction onCancel);
    public abstract void MouseDown(Vector3 position);
    public abstract void MouseUp(Vector3 position);
    public abstract void MouseDrag(Vector3 position);
    public abstract void MouseMove(Vector3 position);

    public abstract void Rotate();

    public abstract void Cancel();
    //public abstract void Place();

    public abstract Vector3Int[] GetCurrentAreaCells();
    public abstract List<Vector3Int> GetAreaCells(Vector3 position, Quaternion rotation);
    public abstract Vector3 GetSize();

}
