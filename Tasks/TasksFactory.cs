using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TasksFactory
{

    [SerializeField] private List<CargoBox> _boxesExpamples;

    public TaskData GetRandomTask(ClockWithDays currentTime)
    {
        CargoBox box = _boxesExpamples[Random.Range(0, _boxesExpamples.Count)];
        int count = Random.Range(1, 5);

        ClockWithDays avaiableUntil = currentTime;
        avaiableUntil.AddMinutes(Random.Range(60, 180));

        ClockWithDays unloadDate = avaiableUntil;
        unloadDate.AddHours(Random.Range(12, 48));
        unloadDate.AddMinutes(Random.Range(0, 60));

        float price = (float)System.Math.Round((count * 1.1f * ((100 - count) * 0.01f)) + (box.weight * 0.1f) + ((unloadDate - avaiableUntil).GetTotalMinutes() / 500f), 2);

        return new TaskData(count, box, avaiableUntil, unloadDate, price);
    }

}
