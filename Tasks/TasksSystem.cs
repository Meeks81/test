using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksSystem : MonoBehaviour
{

    [SerializeField] private TasksFactory _tasksFactory;
    [SerializeField] private Transform _handFieldParent;
    [Space]
    [SerializeField] private GameClock _gameClock;
    [SerializeField] private StorageSystem _storageSystem;
    [SerializeField] private Wallet _wallet;

    public static Transform HandFieldParent => instance._handFieldParent;

    public TasksStorage activeTasks { get; private set; } = new TasksStorage();
    public TasksStorage loadTasks { get; private set; } = new TasksStorage();
    public TasksStorage unloadTasks { get; private set; } = new TasksStorage();
    public TasksStorage storageTasks { get; private set; } = new TasksStorage();
    //public TaskSubsStorage schedule { get; private set; } = new TaskSubsStorage();

    private static TasksSystem instance;

    private void Start()
    {
        _gameClock.OnMinuteChanged.AddListener(OnGameClockUpdate);
        StartCoroutine(Timer());

        instance = this;
    }

    //public bool AddLoadCargo(TaskData task, TaskSub taskSub)
    //{
    //    if (taskSub.Type != TaskSubType.Load ||
    //        activeTasks.tasks.Contains(task) == false || 
    //        schedule.taskSubs.Contains(taskSub) == false)
    //        return false;

    //    activeTasks.RemoveTask(task);
    //    taskSub.AddTask(task);

    //    return true;
    //}

    //public bool AddUnloadCargo(TaskData task, TaskSub taskSub)
    //{
    //    if (taskSub.Type != TaskSubType.Unload ||
    //        storageTasks.tasks.Contains(task) == false ||
    //        schedule.taskSubs.Contains(taskSub) == false)
    //        return false;

    //    storageTasks.RemoveTask(task);
    //    unloadTasks.AddTask(task);
    //    taskSub.AddTask(task);

    //    return true;
    //}

    //private void AddTaskSubToSchedule(TaskSub taskSub)
    //{
    //    int day = taskSub.time.ToClock() < _gameClock.ToClock() ? _gameClock.Day + 1 : _gameClock.Day;
    //    schedule.AddTaskSub(new TaskSub(new ClockWithDays(day, taskSub.time.ToClock()), taskSub.Type));
    //}

    private void OnGameClockUpdate(int value)
    {
        //UpdateScheduleTasks();
        UpdateActiveTasks();
    }

    private void UpdateActiveTasks()
    {
        List<TaskData> tasks = activeTasks.tasks;
        foreach (var item in tasks)
        {
            if (item.availableUntil < _gameClock.ToClockWithDays())
                activeTasks.RemoveTask(item);
        }
    }

    //private void UpdateScheduleTasks()
    //{
    //    foreach (var item in schedule.taskSubs)
    //    {
    //        if (item.time > _gameClock.ToClockWithDays())
    //            continue;

    //        schedule.RemoveTaskSub(item);

    //        if (item.Type == TaskSubType.Load)
    //            LoadCargo(item);
    //        else if (item.Type == TaskSubType.Unload)
    //            UnloadCargo(item);
    //    }
    //}

    private void LoadCargo(TaskSub taskSub)
    {
        foreach (var item in taskSub.tasks)
        {
            loadTasks.AddTask(item);
        }
    }

    private void UnloadCargo(TaskSub taskSub)
    {
        foreach (var item in taskSub.tasks)
        {
            _storageSystem.RemoveBoxes(item.boxes);
            unloadTasks.RemoveTask(item);
            item.DestroyBoxes();
            _wallet.Add(item.price);
        }
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            TaskData task = _tasksFactory.GetRandomTask(_gameClock.ToClockWithDays());
            activeTasks.AddTask(task);
            yield return new WaitForSeconds(Random.Range(5, 20));
        }
    }

}
