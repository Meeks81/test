using System.Collections.Generic;
using UnityEngine;

public class UnloadCargoZone : MonoBehaviour
{

    private List<TaskData> _tasks = new List<TaskData>();

    public List<TaskData> tasks => new List<TaskData>(_tasks);

    public void AddTask(TaskData task)
    {
        _tasks.Add(task);
    }

}
