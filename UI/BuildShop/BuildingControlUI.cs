using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildingControlUI : MonoBehaviour
{

    [SerializeField] private Button _applyButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _deleteButton;

    public BuildingController CurrentBuilding { get; private set; }
    private Camera _mainCamera;

    private void Update()
    {
        if (CurrentBuilding == null)
            return;

        transform.position = _mainCamera.WorldToScreenPoint(CurrentBuilding.transform.position);
    }

    public void SetBuilding(BuildingController building, UnityAction onApply, UnityAction onCancel, UnityAction onDelete)
    {
        CurrentBuilding = building;

        InitializeButton(_applyButton, onApply);
        InitializeButton(_cancelButton, onCancel);
        InitializeButton(_deleteButton, onDelete);

        gameObject.SetActive(true);

        if (_mainCamera == null)
            _mainCamera = Camera.main;
    }

    private void InitializeButton(Button button, UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.gameObject.SetActive(action != null);
        if (action != null)
        {
            button.onClick.AddListener(() =>
            {
                action();
                gameObject.SetActive(false);
            });
        }
    }

}
