using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskData
{

    public int count { get; private set; }
    public CargoBox box { get; private set; }
    public float weight => box.weight * count;
    public ClockWithDays availableUntil { get; private set; }
    public ClockWithDays unloadTime { get; private set; }
    public float price { get; private set; }

    public TaskSubType taskType { get; private set; }
    public ClockWithDays taskTimeSince { get; private set; }
    public ClockWithDays taskTimeTo { get; private set; }

    public List<CargoBox> boxes { get; private set; }

    public TaskData(int count, CargoBox box, ClockWithDays availableUntil, ClockWithDays unloadTime, float price)
    {
        this.count = count;
        this.box = box;
        this.availableUntil = availableUntil;
        this.unloadTime = unloadTime;
        this.price = price;
    }

    public void SetAction(TaskSubType taskType, ClockWithDays taskTimeSince, ClockWithDays taskTimeTo)
    {
        if (taskTimeTo < taskTimeSince)
            return;

        this.taskTimeSince = taskTimeSince;
        this.taskTimeTo = taskTimeTo;
    }

    public void SpawnBoxes(StorageArea storageArea)
    {
        if (boxes != null)
            return;

        boxes = new List<CargoBox>();
        for (int i = 0; i < count; i++)
        {
            CargoBox newBox = Object.Instantiate(box);
            storageArea.AddBox(newBox);
            boxes.Add(newBox);
        }
    }

    public void DestroyBoxes()
    {
        foreach (var item in boxes)
            Object.Destroy(item.gameObject);

        boxes = null;
    }

}
