using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageFieldUI : MonoBehaviour
{

    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _placeCountText;
    [SerializeField] private ObjectPool<Image> _cargosPool;

    public Storage currentStorage { get; private set; }

    public void SetStorage(Storage storage)
    {
        currentStorage = storage;

        UpdateCargos();
    }

    private void UpdateCargos()
    {
        _cargosPool.HideEverything();

        List<CargoBox> boxes = currentStorage.GetCargos();
        foreach (var item in boxes)
        {
            Image cargoIcon = _cargosPool.Get();
            cargoIcon.sprite = item.sprite;

            cargoIcon.gameObject.SetActive(true);
        }

        _placeCountText.text = $"{Icons.Count} {boxes.Count} / {currentStorage.GetPlaceCount(null)}";
    }

}
