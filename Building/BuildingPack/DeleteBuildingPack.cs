public class DeleteBuildingPack : BuildingPack
{

    private GhostBuilding _ghost;

    public void Pack(Map map, BuildingController building)
    {
        _map = map;
        Building = building;

        _ghost = SpawnGhostBuilding(building.transform.position, building.transform.rotation);
        transform.position = building.transform.position;
        transform.rotation = building.transform.rotation;

        _map.DeleteBuilding(Building, false);
        Building.transform.SetParent(_buildingParent);

        foreach (var item in _animators)
            item.SetTrigger("Pack");
    }

    public override void Apply()
    {
        _map.DeleteBuilding(_ghost);
    }

}
