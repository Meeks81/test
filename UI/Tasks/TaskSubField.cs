using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TaskSubField : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private TextMeshProUGUI _typeText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private UnityEngine.Gradient _backgroundGradientByTime;
    [Space]
    public UnityEvent OnClick;

    public TaskSub currentTaskSub {  get; private set; }

    public void SetTaskSub(TaskSub taskSub)
    {
        currentTaskSub = taskSub;

        _typeText.text = taskSub.Type == TaskSubType.Load ? Icons.LoadCargo : Icons.UnloadCargo;
        _timeText.text = taskSub.time.ToClock().ToString();
        _backgroundImage.color = _backgroundGradientByTime.Evaluate(1f / (24f * 60f) * taskSub.time.ToClock().GetTotalMinutes());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }

}
