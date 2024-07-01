using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BuildingEdit : MonoBehaviour
{

    [SerializeField] private float _delayTime;
    [Space]
    [SerializeField] private Builder _builder;
    [SerializeField] private BuildingPackSystem _buildingPackSystem;
    [SerializeField] private BuildingControlUI _buildingControlUI;
    [SerializeField] private Map _map;

    public float DelayTime => _delayTime;
    public float Timer { get; private set; }
    public BuildingController ClickedBuilding { get; private set; }

    private Vector3 _mouseClickPos;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (_builder.IsModeActive == false)
            return;

        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Timer = 0;
            _mouseClickPos = Input.mousePosition;
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) &&
                hit.rigidbody != null &&
                hit.rigidbody.transform.TryGetComponent(out BuildingController building) &&
                _builder.FlyingBuilding != building &&
                building.IsEdit)
            {
                ClickedBuilding = building;
            }
        }
        if (Input.GetMouseButton(0) && ClickedBuilding != null)
        {
            Timer += Time.deltaTime;

            if (Timer >= _delayTime)
            {
                BuildingController building = ClickedBuilding;
                Vector3 position = building.transform.position;
                Quaternion rotation = building.transform.rotation;
                _builder.SetBuilding(building,
                () =>
                {
                    _buildingControlUI.SetBuilding(building,
                        () =>
                        {
                            _buildingPackSystem.MoveBuilding(building, position, rotation);
                        },
                        () =>
                        {
                            building.transform.position = position;
                            building.transform.rotation = rotation;
                        },
                        () =>
                        {
                            _map.DeleteBuilding(building);
                        });
                },
                    () =>
                    {
                        building.transform.position = position;
                        building.transform.rotation = rotation;
                    });
                ClickedBuilding = null;
            }

            if (Vector3.Distance(_mouseClickPos, Input.mousePosition) > 10f)
            {
                ClickedBuilding = null;
                Timer = 0;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Timer = 0;
            ClickedBuilding = null;
        }
    }

}
