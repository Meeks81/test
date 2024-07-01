using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{

    [SerializeField] private List<StorageArea> _storageAreas;

    public List<StorageArea> storageAreas => new List<StorageArea>(_storageAreas);

    public void AddBox(CargoBox box)
    {
        foreach (var item in storageAreas)
        {
            if (item.GetFreePlaceCount(box) > 0)
            {
                item.AddBox(box);
                break;
            }    
        }
    }

    public void RemoveBox(CargoBox box)
    {
        foreach (var item in storageAreas)
        {
            item.RemoveBox(box);
        }
    }

    public List<CargoBox> GetCargos()
    {
        List<CargoBox> boxes = new List<CargoBox>();

        foreach (var item in storageAreas)
            boxes.AddRange(item.boxes);

        return boxes;
    }

    public bool Contains(CargoBox box)
    {
        foreach (var item in storageAreas)
            if (item.boxes.Contains(box))
                return true;

        return false;
    }

    public int GetFreePlaceCount(CargoBox box)
    {
        int count = 0;

        foreach (var item in storageAreas)
            count += item.GetFreePlaceCount(box);

        return count;
    }

    public int GetPlaceCount(CargoBox box)
    {
        int count = 0;

        foreach (var item in storageAreas)
            count += item.GetPlaceCount(box);

        return count;
    }

    public void SetCargoBox(CargoBox cargoBox)
    {
        foreach (var item in storageAreas)
        {
            if (item.GetFreePlaceCount(cargoBox) > 0)
                item.AddBox(cargoBox);
        }
    }

}
