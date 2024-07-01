using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargosManager : MonoBehaviour
{

    [SerializeField] private StorageArea _outsideArea;
    [Space]
    [SerializeField] private TasksSystem _tasksSystem;
    [SerializeField] private StorageSystem _storageSystem;

    private List<CargoBox> _loadCargos = new List<CargoBox>();
    private List<CargoBox> _unloadCargos = new List<CargoBox>();

    private void OnEnable()
    {
        StartCoroutine(Timer());

        _tasksSystem.storageTasks.OnAdded.AddListener(OnStorageAdded);
        _tasksSystem.unloadTasks.OnAdded.AddListener(OnUnloadAdded);
    }

    private void OnDisable()
    {
        _tasksSystem.storageTasks.OnAdded.RemoveListener(OnStorageAdded);
        _tasksSystem.unloadTasks.OnAdded.RemoveListener(OnUnloadAdded);
    }

    private void OnStorageAdded(TaskData task)
    {
        if (task.boxes == null)
            task.SpawnBoxes(_outsideArea);

        _loadCargos.AddRange(task.boxes);

        List<CargoBox> leftBoxes = _storageSystem.AddBoxes(task.boxes, _storageSystem.pallets.ToArray());

        if (leftBoxes.Count > 0)
        {
            Debug.LogError("Boxes left");
        }
    }

    private void OnUnloadAdded(TaskData task)
    {
        _unloadCargos.AddRange(task.boxes);
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);

            if (_loadCargos.Count > 0)
            {
                CargoBox box = _loadCargos[0];
                foreach (var item in _storageSystem.pallets)
                {
                    if (item.Contains(box))
                    {
                        List<CargoBox> leftBoxes = _storageSystem.AddBoxes(new List<CargoBox>() { box }, _storageSystem.shelves.ToArray());
                        if (leftBoxes.Count == 0)
                        {
                            item.RemoveBox(box);
                            _loadCargos.Remove(box);
                        }
                        break;
                    }
                }
            }
            else if (_unloadCargos.Count > 0)
            {
                CargoBox box = _unloadCargos[0];
                foreach (var item in _storageSystem.shelves)
                {
                    if (item.Contains(box))
                    {
                        List<CargoBox> leftBoxes = _storageSystem.AddBoxes(new List<CargoBox>() { box }, _storageSystem.pallets.ToArray());
                        if (leftBoxes.Count == 0)
                        {
                            item.RemoveBox(box);
                            _unloadCargos.Remove(box);
                        }
                        break;
                    }
                }
            }
        }
    }

}
