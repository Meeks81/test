using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WallDeleter : MonoBehaviour
{

    [SerializeField] private BuildingControlUI _buildingControlUI;
    [SerializeField] private Map _map;

    private List<BuildingController> _selectBuildings = new List<BuildingController>();

    private bool _isActive = false;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            _isActive = true;
            foreach (var item in _selectBuildings)
                item.transparentBuilding.SetNormal();
            _selectBuildings.Clear();
            _buildingControlUI.gameObject.SetActive(false);
        }
        if (_isActive)
        {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) &&
                hit.rigidbody != null &&
                hit.rigidbody.transform.TryGetComponent(out BuildingController building) &&
                _selectBuildings.Contains(building) == false)
            {
                if (building.GetType() == typeof(BuildWall) || building.GetType() == typeof(DoorBuilding))
                {
                    _selectBuildings.Add(building);
                    building.transparentBuilding.SetTransparent(new Color(1f, 0.5f, 0f), 1f);
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            _isActive = false;

            if (_selectBuildings.Count > 0)
            {
                _buildingControlUI.SetBuilding(_selectBuildings[_selectBuildings.Count - 1],
                    null,
                    () =>
                    {
                        foreach (var item in _selectBuildings)
                            item.transparentBuilding.SetNormal();
                        _selectBuildings.Clear();
                        _buildingControlUI.gameObject.SetActive(false);
                    },
                    () =>
                    {
                        foreach (var item in _selectBuildings)
                            _map.DeleteBuilding(item);
                        _selectBuildings.Clear();
                        _buildingControlUI.gameObject.SetActive(false);
                    });
            }
        }
    }

}
