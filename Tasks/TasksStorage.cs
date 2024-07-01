using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class TasksStorage
{

    private List<TaskData> _taskSubs = new List<TaskData>();

    public List<TaskData> tasks => new List<TaskData>(_taskSubs);

    public UnityEvent<List<TaskData>> OnChanged = new UnityEvent<List<TaskData>>();
    public UnityEvent<TaskData> OnAdded = new UnityEvent<TaskData>();
    public UnityEvent<TaskData> OnRemoved = new UnityEvent<TaskData>();

    public void AddTask(TaskData task)
    {
        if (task == null || _taskSubs.Contains(task))
            return;

        _taskSubs.Add(task);

        OnAdded?.Invoke(task);
        OnChanged?.Invoke(_taskSubs);
    }

    public void RemoveTask(TaskData task)
    {
        if (task == null || _taskSubs.Contains(task) == false)
            return;

        _taskSubs.Remove(task);

        OnRemoved?.Invoke(task);
        OnChanged?.Invoke(_taskSubs);
    }

}
