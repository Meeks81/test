using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class TaskSub
{

    public ClockWithDays time { get; private set; }
    public List<TaskData> tasks => new List<TaskData>(_tasks);

    public UnityEvent<List<TaskData>> OnChanged;
    public TaskSubType Type => _type;

    private List<TaskData> _tasks = new List<TaskData>();
    private TaskSubType _type = TaskSubType.Load;

    public TaskSub(ClockWithDays time, TaskSubType type)
    {
        this.time = time;
        _type = type;
    }

    public void AddTask(TaskData task)
    {
        if (_tasks.Contains(task))
            return;

        _tasks.Add(task);
        OnChanged?.Invoke(new List<TaskData>(_tasks));
    }

    public void CancelTask(TaskData task)
    {
        _tasks.Remove(task);
        OnChanged?.Invoke(new List<TaskData>(_tasks));
    }

    public void ClearTasks()
    {
        _tasks.Clear();
    }

}
