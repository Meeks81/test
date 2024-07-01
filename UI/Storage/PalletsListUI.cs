using UnityEngine;

public class PalletsListUI : StoragesListUI
{

    [SerializeField] private StorageSystem _storageSystem;

    protected override Storage[] Storages => _storageSystem.pallets.ToArray();

}
