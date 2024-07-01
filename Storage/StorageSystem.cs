using System.Collections.Generic;
using UnityEngine;

public class StorageSystem : MonoBehaviour
{

    [SerializeField] private Map _map;

    public List<Shelf> shelves => new List<Shelf>(_shelves);
    public List<Pallet> pallets => new List<Pallet>(_pallets);

    private List<Shelf> _shelves = new List<Shelf>();
    private List<Pallet> _pallets = new List<Pallet>();

    public int GetShelvesFreePlaceCount(CargoBox box) => GetFreePlaceCount(_shelves.ToArray(), box);
    public int GetPalletsFreePlaceCount(CargoBox box) => GetFreePlaceCount(_pallets.ToArray(), box);

    public int GetShelvesTotalPlaceCount(CargoBox box) => GetTotalPlaceCount(_shelves.ToArray(), box);
    public int GetPalletsTotalPlaceCount(CargoBox box) => GetTotalPlaceCount(_pallets.ToArray(), box);

    public int GetFreePlaceCount(Storage[] storages, CargoBox box)
    {
        int count = 0;

        foreach (var item in storages)
            count += item.GetFreePlaceCount(box);

        return count;
    }

    public int GetTotalPlaceCount(Storage[] storages, CargoBox box)
    {
        int count = 0;

        foreach (var item in storages)
            count += item.GetPlaceCount(box);

        return count;
    }

    public List<CargoBox> AddBoxes(List<CargoBox> boxes, Storage[] storages)
    {
        List<CargoBox> leftBoxes = new List<CargoBox>(boxes);

        foreach (var pallet in storages)
        {
            for (int i = 0; i < leftBoxes.Count; i++)
            {
                int freePlace = pallet.GetFreePlaceCount(leftBoxes[i]);
                if (freePlace > 0)
                {
                    pallet.AddBox(leftBoxes[0]);
                    leftBoxes.RemoveAt(0);
                    i--;
                }
            }
            if (leftBoxes.Count == 0)
                break;
        }

        return leftBoxes;
    }

    public void RemoveBoxes(List<CargoBox> boxes)
    {
        foreach (var pallet in pallets)
            foreach (var item in boxes)
                pallet.RemoveBox(item);

        foreach (var pallet in shelves)
            foreach (var item in boxes)
                pallet.RemoveBox(item);
    }

    private void OnEnable()
    {
        _map.OnAddBuilding.AddListener(OnBuildingAdded);
        _map.OnDeleteBuilding.AddListener(OnBuildingRemoved);
    }

    private void OnDisable()
    {
        _map.OnAddBuilding.RemoveListener(OnBuildingAdded);
        _map.OnDeleteBuilding.RemoveListener(OnBuildingRemoved);
    }

    private void OnBuildingAdded(BuildingController building)
    {
        if (building.TryGetComponent(out Shelf shelf))
            _shelves.Add(shelf);
        if (building.TryGetComponent(out Pallet pallet))
            _pallets.Add(pallet);
    }

    private void OnBuildingRemoved(BuildingController building)
    {
        if (building.TryGetComponent(out Shelf shelf))
            _shelves.Remove(shelf);
        if (building.TryGetComponent(out Pallet pallet))
            _pallets.Remove(pallet);
    }

}
