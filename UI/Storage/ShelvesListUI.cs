using UnityEngine;

public class ShelvesListUI : StoragesListUI
{

    [SerializeField] private StorageSystem _storageSystem;

    protected override Storage[] Storages => _storageSystem.shelves.ToArray();

}
