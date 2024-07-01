using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClicker : MonoBehaviour
{

    [SerializeField] private ScheduleUI _scheduleUI;
    [Space]
    [SerializeField] private Builder _builder;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false && _builder.IsModeActive == false)
        {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) && hit.rigidbody != null)
            {
                if (hit.rigidbody.TryGetComponent(out UnloadCargoZone cargoZone))
                {
                    _scheduleUI.SetCargoZone(cargoZone);
                    _scheduleUI.gameObject.SetActive(true);
                }
            }
        }
    }

}
