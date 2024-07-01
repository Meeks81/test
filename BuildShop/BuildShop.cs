using UnityEngine;

public class BuildShop : MonoBehaviour
{

    [SerializeField] private BuildShopCategory[] _categories;

    public BuildShopCategory[] categories => _categories;

}
