using UnityEngine;

public class MoveBuildingPack : BuildingPack
{

    public Vector3 NewPosition { get; private set; }
    public Quaternion NewRotation { get; private set; }

    private GhostBuilding _currentPosGhost;
    private GhostBuilding _newPosGhost;

    public void Pack(Map map, BuildingController building, Vector3 lastPosition, Quaternion lastRotation)
    {
        _map = map;
        Building = building;
        NewPosition = building.transform.position;
        NewRotation = building.transform.rotation;

        _currentPosGhost = SpawnGhostBuilding(building.transform.position, building.transform.rotation);
        transform.position = lastPosition;
        transform.rotation = lastRotation;
        building.transform.position = lastPosition;
        building.transform.rotation = lastRotation;
        _newPosGhost = SpawnGhostBuilding(NewPosition, NewRotation);

        _map.DeleteBuilding(Building, false);
        Building.transform.SetParent(_buildingParent);

        foreach (var item in _animators)
            item.SetTrigger("Pack");
    }

    public override void Apply()
    {
        transform.position = NewPosition;
        transform.rotation = NewRotation;

        _map.AddBuilding(Building);

        _map.DeleteBuilding(_currentPosGhost);
        _map.DeleteBuilding(_newPosGhost);

        foreach (var item in _animators)
            item.SetTrigger("Place");
    }

    public void FinishBuildingAnimation()
    {
        if (Building == null)
            return;

        Building.transform.SetParent(null);
        Building.transform.localScale = Vector3.one;

        Destroy(gameObject);
        Building = null;
    }

}
