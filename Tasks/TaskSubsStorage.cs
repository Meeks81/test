using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class TaskSubsStorage
{

    private List<TaskSub> _taskSubs = new List<TaskSub>();

    public List<TaskSub> taskSubs => new List<TaskSub>(_taskSubs);

    public UnityEvent<List<TaskSub>> OnChanged = new UnityEvent<List<TaskSub>>();
    public UnityEvent<TaskSub> OnAdded = new UnityEvent<TaskSub>();
    public UnityEvent<TaskSub> OnRemoved = new UnityEvent<TaskSub>();

    public void AddTaskSub(TaskSub taskSub)
    {
        if (taskSub == null || _taskSubs.Contains(taskSub))
            return;

        _taskSubs.Add(taskSub);

        OnAdded?.Invoke(taskSub);
        OnChanged?.Invoke(_taskSubs);
    }

    public void RemoveTaskSub(TaskSub taskSub)
    {
        if (taskSub == null || _taskSubs.Contains(taskSub) == false)
            return;

        _taskSubs.Remove(taskSub);

        OnRemoved?.Invoke(taskSub);
        OnChanged?.Invoke(_taskSubs);
    }

}
