using TMPro;
using UnityEngine;

public class MoveTaskUI : MonoBehaviour
{

    [SerializeField] private TaskFieldUI _taskField;
    [SerializeField] private TextMeshProUGUI _placeCountText;
    [Space]
    [SerializeField] private TasksSystem _tasksSystem;
    [SerializeField] private StorageSystem _storageSystem;

    public TaskData currentTask => _taskField.currentTask;

    public void SetTask(TaskData task)
    {
        _taskField.SetTask(task);

        int freePlace = _storageSystem.GetPalletsFreePlaceCount(task.box);
        int totalPlace = _storageSystem.GetPalletsTotalPlaceCount(task.box);

        _placeCountText.text = $"{Icons.Count} {totalPlace - freePlace} / {totalPlace}";

        gameObject.SetActive(true);
    }

    public void Load()
    {
        if (currentTask == null)
            return;

        int freePlace = _storageSystem.GetPalletsFreePlaceCount(currentTask.box);
        if (freePlace < currentTask.count)
            return;

        _tasksSystem.loadTasks.RemoveTask(currentTask);
        _tasksSystem.storageTasks.AddTask(currentTask);

        gameObject.SetActive(false);
    }

    public void Unload()
    {
        if (currentTask == null)
            return;

        int freePlace = _storageSystem.GetPalletsFreePlaceCount(currentTask.box);
        if (freePlace < currentTask.count)
            return;

        _tasksSystem.storageTasks.RemoveTask(currentTask);
        _tasksSystem.unloadTasks.AddTask(currentTask);

        gameObject.SetActive(false);
    }

}
