using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Builder : MonoBehaviour
{

    [SerializeField] private Map _map;
    [SerializeField] private BuildingPermit _buildingPermit;
    [SerializeField] private RegionsSystem _regionsSystem;

    //[HideInInspector] public UnityEvent<BuildingController, Vector3, Quaternion> OnBuildingPlaced;
    //[HideInInspector] public UnityEvent<BuildingController> OnBuildingCanceled;
    [HideInInspector] public UnityEvent<BuildingController> OnBuildingEdited;
    [HideInInspector] public UnityEvent<BuildingController> OnFlyingBuildingChanged;
    [HideInInspector] public UnityEvent<BuildingController> OnFlyingBuildingMoved;
    [HideInInspector] public UnityEvent<bool> OnModeChanged;

    public BuildingController FlyingBuilding { get; private set; }

    public bool IsModeActive { get; private set; } = false;

    private List<BuildWall> _hiddenWalls = new List<BuildWall>();

    private UnityAction _onPlace;
    private UnityAction _onCancel;

    private Vector3Int _lastCell;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (IsModeActive && FlyingBuilding != null)
            MoveFlyingObject();
    }

    public void SetMode(bool mode)
    {
        IsModeActive = mode;
        OnModeChanged.Invoke(mode);

        if (mode == false && FlyingBuilding != null)
        {
            CancelEditFlyingObject();
        }
    }

    public void SetBuilding(BuildingController building, UnityAction onPlace, UnityAction onCancel)
    {
        if (IsModeActive == false)
            return;

        if (FlyingBuilding != null)
        {
            CancelEditFlyingObject();
        }

        FlyingBuilding = building;
        FlyingBuilding.Init(PlaceFlyingObject, CancelEditFlyingObject);

        _onPlace = onPlace;
        _onCancel = onCancel;

        OnFlyingBuildingChanged.Invoke(building);

        CancelEventSystem.AddEvent(CancelEdit);
    }

    public void PlaceFlyingObject(BuildingController[] buildings)
    {
        if (FlyingBuilding == null)
            return;

        if (FlyingBuilding.OnlyWalls)
        {
            foreach (var item in _hiddenWalls)
                item.gameObject.SetActive(true);
            _hiddenWalls.Clear();

            bool canBuild = false;
            Vector3Int cell = MapGrid.current.grid.WorldToCell(FlyingBuilding.transform.position);
            BuildWall centerWall = _map.GetWall(cell);
            Vector3Int size = MapGrid.current.grid.WorldToCell(FlyingBuilding.GetSize());
            if (_map.GetWall(cell + Vector3Int.left * (size.x / 2)) != null &&
                _map.GetWall(cell + Vector3Int.right * (size.x / 2)) != null)
            {
                FlyingBuilding.transform.eulerAngles = new Vector3(0, 90, 0);
                for (int i = cell.x - size.x / 2; i <= cell.x + size.x / 2; i++)
                {
                    BuildWall wall = _map.GetWall(new Vector3Int(i, cell.y));
                    if (wall != null)
                    {
                        _map.DeleteWall(wall, _regionsSystem.CheckWall(wall) ? false : true);
                        wall.gameObject.SetActive(false);
                    }
                }
                canBuild = true;
            }
            else if (_map.GetWall(cell + Vector3Int.up * (size.x / 2)) != null &&
                _map.GetWall(cell + Vector3Int.down * (size.x / 2)) != null)
            {
                FlyingBuilding.transform.eulerAngles = new Vector3(0, 0, 0);
                for (int i = cell.y - size.y / 2; i <= cell.y + size.y / 2; i++)
                {
                    BuildWall wall = _map.GetWall(new Vector3Int(cell.x, i));
                    if (wall != null)
                    {
                        _map.DeleteWall(wall, _regionsSystem.CheckWall(wall) ? false : true);
                        wall.gameObject.SetActive(false);
                    }
                }
                canBuild = true;
            }
            if (canBuild == false)
                return;
        }
        else if (_buildingPermit.IsBuildingPlaceAvailable(FlyingBuilding) == false)
            return;

        FlyingBuilding = null;

        _onPlace();

        foreach (var item in buildings)
        {
            //OnBuildingPlaced.Invoke(item, item.transform.position, item.transform.rotation);
            OnBuildingEdited.Invoke(item);
        }

        CancelEventSystem.RemoveEvent(CancelEdit);
    }

    private void CancelEdit()
    {
        if (FlyingBuilding == null)
            return;

        FlyingBuilding.Cancel();
    }

    private void CancelEditFlyingObject()
    {
        //if (_map.BuildObjects.Contains(FlyingBuilding) == false)
        //{
        //    Destroy(FlyingBuilding.gameObject);
        //}
        CancelEventSystem.RemoveEvent(CancelEdit);

        _onCancel();
        //OnBuildingCanceled?.Invoke(FlyingBuilding);

        OnBuildingEdited.Invoke(FlyingBuilding);

        FlyingBuilding = null;
    }

    private void MoveFlyingObject()
    {
        if (FlyingBuilding == null)
            return;

        if (HotKey.GetKeyDown(HotKeyType.BuildObjectRotate))
        {
            FlyingBuilding.Rotate();
        }

        Vector3 cursorPosition = GetRayPosition();
        bool canBuild = false;

        if (FlyingBuilding.OnlyWalls)
        {
            Vector3Int cell = MapGrid.current.grid.WorldToCell(cursorPosition);
            BuildWall centerWall = _map.GetWall(cell);
            if (centerWall != null &&
                ((FlyingBuilding.OnlyRegionWalls && _regionsSystem.CheckWall(centerWall)) || 
                FlyingBuilding.OnlyRegionWalls == false))
            {
                foreach (var item in _hiddenWalls)
                    item.gameObject.SetActive(true);
                _hiddenWalls.Clear();

                cursorPosition = centerWall.transform.position;
                Vector3Int size = MapGrid.current.grid.WorldToCell(FlyingBuilding.GetSize());
                if (_map.GetWall(cell + Vector3Int.left * (size.x / 2)) != null &&
                    _map.GetWall(cell + Vector3Int.right * (size.x / 2)) != null)
                {
                    FlyingBuilding.transform.eulerAngles = new Vector3(0, 90, 0);
                    for (int i = cell.x - size.x / 2; i <= cell.x + size.x / 2; i++)
                    {
                        BuildWall wall = _map.GetWall(new Vector3Int(i, cell.y));
                        if (wall != null)
                        {
                            _hiddenWalls.Add(wall);
                            wall.gameObject.SetActive(false);
                        }
                    }
                    canBuild = true;
                }
                else if (_map.GetWall(cell + Vector3Int.up * (size.x / 2)) != null &&
                    _map.GetWall(cell + Vector3Int.down * (size.x / 2)) != null)
                {
                    FlyingBuilding.transform.eulerAngles = new Vector3(0, 0, 0);
                    for (int i = cell.y - size.y / 2; i <= cell.y + size.y / 2; i++)
                    {
                        BuildWall wall = _map.GetWall(new Vector3Int(cell.x, i));
                        if (wall != null)
                        {
                            _hiddenWalls.Add(wall);
                            wall.gameObject.SetActive(false);
                        }
                    }
                    canBuild = true;
                }
            }
            else if (_hiddenWalls.Count > 0)
            {
                foreach (var item in _hiddenWalls)
                    item.gameObject.SetActive(true);
                _hiddenWalls.Clear();
            }
        }

        FlyingBuilding.MouseMove(cursorPosition);

        if (FlyingBuilding.OnlyWalls)
        {
            if (canBuild)
                FlyingBuilding.transparentBuilding.SetNormal();
            else
                FlyingBuilding.transparentBuilding.SetTransparent(Color.red, 0.5f);
        }

        Vector3Int[] area = FlyingBuilding.GetCurrentAreaCells();
        Vector3Int checkCell = area[0] + area[area.Length - 1];

        if (checkCell != _lastCell)
        {
            OnFlyingBuildingMoved.Invoke(FlyingBuilding);

            _lastCell = checkCell;
        }

        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            FlyingBuilding.MouseDown(cursorPosition);
        }
    }

    private Vector3 GetRayPosition()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane hPlane = new Plane(Vector3.up, Vector3.zero);
        if (hPlane.Raycast(ray, out float distance))
        {
            Vector3 point = ray.GetPoint(distance);
            return new Vector3(point.x, point.y, point.z);
        }
        return Vector3.zero;
    }

}
