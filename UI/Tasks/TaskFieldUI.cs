using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TaskFieldUI : MonoBehaviour
{

    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private TextMeshProUGUI _weightText;
    [SerializeField] private TextMeshProUGUI _unloadTimeText;
    [SerializeField] private TextMeshProUGUI _loadTimeText;
    [SerializeField] private TextMeshProUGUI _priceText;

    public TaskData currentTask { get; private set; }

    public void SetTask(TaskData task)
    {
        currentTask = task;
        if (_icon != null)
            _icon.sprite = task.box.sprite;
        if (_titleText != null)
            _titleText.text = task.box.title;
        if (_countText != null)
            _countText.text = $"{Icons.Count} {task.count}";
        if (_weightText != null)
            _weightText.text = $"{Icons.Weight} {task.weight}";
        if (_unloadTimeText != null)
            _unloadTimeText.text = $"{Icons.UnloadCargo} {task.unloadTime.ToString()}";
        if (_loadTimeText != null)
            _loadTimeText.text = $"{Icons.LoadCargo} {task.availableUntil.ToString()}";
        if (_priceText != null)
            _priceText.text = $"{task.price}$";
    }

}
