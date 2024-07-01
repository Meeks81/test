using UnityEngine;
using UnityEngine.UI;

public class BuilderUI : MonoBehaviour
{

    [SerializeField] private Image _selectorProgressImage;
    [Space]
    [SerializeField] private BuildShopMenu _buildShopMenu;
    [SerializeField] private ModsSystem _modsSystem;
    [SerializeField] private Builder _builder;
    [SerializeField] private BuildingEdit _buildingEdit;

    private void Update()
    {
        if (_buildingEdit.ClickedBuilding != null)
        {
            _selectorProgressImage.fillAmount = 1f / _buildingEdit.DelayTime * _buildingEdit.Timer;
            _selectorProgressImage.transform.position = Input.mousePosition;
            _selectorProgressImage.gameObject.SetActive(true);
        }
        else
            _selectorProgressImage.gameObject.SetActive(false);
    }

    public void SetModeOn()
    {
        if (_builder.IsModeActive == false)
            _modsSystem.TurnOn("Building");

        _buildShopMenu.OpenAllCategories();
    }

}
