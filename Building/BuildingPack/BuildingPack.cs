using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingPack : MonoBehaviour
{

    [SerializeField] protected GhostBuilding _ghostBuildingPrefab;
    [SerializeField] protected Transform _buildingParent;
    [SerializeField] protected List<Animator> _animators;

    public BuildingController Building { get; protected set; }

    protected Map _map;

    public abstract void Apply();

    protected GhostBuilding SpawnGhostBuilding(Vector3 position, Quaternion rotation)
    {
        GhostBuilding ghost = Instantiate(_ghostBuildingPrefab);
        ghost.Initilize(Building, Building.transform.position, Building.transform.rotation);
        _map.AddBuilding(ghost);

        return ghost;
    }

}
