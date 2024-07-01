using System.Collections;
using UnityEngine;

public class BuildingPackSystem : MonoBehaviour
{

    [SerializeField] private float _buildTime;
    [Space]
    [SerializeField] private PlaceBuildingPack _placeBuildingPack;
    [SerializeField] private MoveBuildingPack _moveBuildingPack;
    [SerializeField] private DeleteBuildingPack _deleteBuildingPack;
    [Space]
    [SerializeField] private Builder _builder;
    [SerializeField] private Map _map;

    //private void Start()
    //{
    //    _builder.OnBuildingPlaced.AddListener(OnBuildingPlaced);
    //}

    //private void OnBuildingPlaced(BuildingController building, Vector3 position, Quaternion rotation)
    //{
    //    //BuildingPack pack = Instantiate(building.PackPrefab == null ? _buildingPackPrefab : building.PackPrefab);
    //    //pack.SetBuilding(_map, building, position, rotation);

    //    //StartCoroutine(SetBuildTimer(pack));
    //    if (_map.BuildObjects.Contains(building))
    //        MoveBuilding(building);
    //    else
    //        PlaceBuilding(building);
    //}

    public void PlaceBuilding(BuildingController building)
    {
        PlaceBuildingPack pack = Instantiate(_placeBuildingPack);
        pack.Pack(_map, building);

        StartCoroutine(SetBuildTimer(pack));
    }

    public void MoveBuilding(BuildingController building, Vector3 lastPosition, Quaternion lastRotation)
    {
        MoveBuildingPack pack = Instantiate(_moveBuildingPack);
        pack.Pack(_map, building, lastPosition, lastRotation);

        StartCoroutine(SetBuildTimer(pack));
    }

    public void DeleteBuilding(BuildingController building)
    {
        DeleteBuildingPack pack = Instantiate(_deleteBuildingPack);
        pack.Pack(_map, building);

        StartCoroutine(SetBuildTimer(pack));
    }

    private IEnumerator SetBuildTimer(BuildingPack pack)
    {
        yield return new WaitForSeconds(_buildTime);
        pack.Apply();
    }

}
