using UnityEngine;

public class BuildShopMenu : MonoBehaviour
{

    [SerializeField] private MenuUI _menu;
    [Space]
    [SerializeField] private BuildShop _shop;
    [SerializeField] private Builder _builder;
    [SerializeField] private BuildingPackSystem _buildingPackSystem;
    [SerializeField] private BuildingControlUI _buildingControlUI;

    public void OpenAllCategories()
    {
        _menu.ClearButtons();
        foreach (var item in _shop.categories)
        {
            BuildShopCategory category = item;
            _menu.AddButton(category.icon, () => OpenCategory(category));
        }
    }

    public void CloseMenu()
    {
        _menu.ClearButtons();
    }

    public void OpenCategory(BuildShopCategory category)
    {
        _menu.ClearButtons();
        foreach (var item in category.products)
        {
            BuildShopProduct product = item;
            _menu.AddButton(product.icon, () => BuyProduct(product));
        }
    }

    private void BuyProduct(BuildShopProduct product)
    {
        BuildingController building = Instantiate(product.prefab);
        _builder.SetBuilding(building, 
        () =>
        {
            _buildingControlUI.SetBuilding(building,
            () =>
            {
                _buildingPackSystem.PlaceBuilding(building);
            },
            () =>
            {
                Destroy(building.gameObject);
            },
            null);
        }, 
        () =>
        {
            Destroy(building.gameObject);
        });
    }

}
