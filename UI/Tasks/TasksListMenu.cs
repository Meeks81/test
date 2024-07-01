using System.Collections.Generic;
using UnityEngine;

public class TasksListMenu : MonoBehaviour
{

    [SerializeField] private TaskFieldUI _fieldPrefab;
    [SerializeField] private Transform _fieldsParent;
    [Space]
    [SerializeField] private TasksSystem _tasksSystem;

    private List<TaskFieldUI> _fields = new List<TaskFieldUI>();

    private void OnEnable()
    {
        UpdateTasks();
        _tasksSystem.activeTasks.OnAdded.AddListener(AddTask);
        _tasksSystem.activeTasks.OnRemoved.AddListener(RemoveTask);
    }

    private void OnDisable()
    {
        _tasksSystem.activeTasks.OnAdded.RemoveListener(AddTask);
        _tasksSystem.activeTasks.OnRemoved.RemoveListener(RemoveTask);
    }

    private void AddTask(TaskData task)
    {
        TaskFieldUI field = Instantiate(_fieldPrefab, _fieldsParent);
        field.SetTask(task);
        _fields.Add(field);
    }

    private void RemoveTask(TaskData task)
    {
        TaskFieldUI findedField = _fields.Find(e => e.currentTask == task);
        _fields.Remove(findedField);
        Destroy(findedField.gameObject);
    }

    private void UpdateTasks()
    {
        foreach (var item in _fields)
            Destroy(item.gameObject);
        _fields.Clear();

        foreach (var item in _tasksSystem.activeTasks.tasks)
            AddTask(item);
    }

}
