using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoragesListUI : MonoBehaviour
{

    [SerializeField] private ObjectPool<StorageFieldUI> _pool;
    [Space]
    [SerializeField] private Map _map;

    protected abstract Storage[] Storages { get; }

    private void OnEnable()
    {
        _map.OnAddBuilding.AddListener(OnMapChanged);
        _map.OnDeleteBuilding.AddListener(OnMapChanged);

        UpdateStorageFields();
    }

    private void OnDisable()
    {
        _map.OnAddBuilding.RemoveListener(OnMapChanged);
        _map.OnDeleteBuilding.RemoveListener(OnMapChanged);
    }

    private void OnMapChanged(BuildingController building)
    {
        UpdateStorageFields();
    }

    private void UpdateStorageFields()
    {
        _pool.HideEverything();

        foreach (var item in Storages)
        {
            StorageFieldUI field = _pool.Get();
            field.SetStorage(item);

            field.gameObject.SetActive(true);
        }
    }

}
