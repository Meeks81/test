using System.Collections.Generic;
using UnityEngine;

public class ScheduleUI : MonoBehaviour
{

    [SerializeField] private ObjectPool<ScheduleRow> _rowsPool;
    [Space]
    [SerializeField] private GameClock _gameClock;

    public UnloadCargoZone currentCargoZone { get; private set; }

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (currentCargoZone == null)
            return;

        transform.position = _mainCamera.WorldToScreenPoint(currentCargoZone.transform.position);
    }

    public void SetCargoZone(UnloadCargoZone cargoZone)
    {
        currentCargoZone = cargoZone;

        SpawnRows(new List<UnloadCargoZone>() { cargoZone });
    }

    private void SpawnRows(List<UnloadCargoZone> cargoZones)
    {
        _rowsPool.HideEverything();

        foreach (var item in cargoZones)
        {
            ScheduleRow row = _rowsPool.Get();
            row.SetUnloadCargoZone(item, _gameClock);

            row.gameObject.SetActive(true);
        }
    }

}
