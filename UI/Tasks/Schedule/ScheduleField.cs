using UnityEngine;
using UnityEngine.EventSystems;

public class ScheduleField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{

    [SerializeField] private TaskFieldUI _taskFieldUI;

    public TaskData task => _taskFieldUI.currentTask;
    public TaskFieldUI taskField => _taskFieldUI;

    private Vector3 _cursorSpace;
    private bool _isDragging;
    private Vector2 _lastSize;
    private Vector2 _lastPosition;
    private Transform _lastParent;
    private int _lastChildCount;

    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public RectTransform GetRectTransform() => _rectTransform ??= GetComponent<RectTransform>();

    public void OnPointerDown(PointerEventData eventData)
    {
        _cursorSpace = transform.position - Input.mousePosition;
        _isDragging = true;
        _lastSize = _rectTransform.rect.size;
        _lastPosition = _rectTransform.position;
        _lastParent = transform.parent;
        _lastChildCount = transform.GetSiblingIndex();

        transform.SetParent(TasksSystem.HandFieldParent);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (_isDragging == false)
            return;

        Vector2 position = Input.mousePosition + _cursorSpace;

        ScheduleRow row = GetRow();
        if (row != null)
        {
            row.PlaceField(this, position);
        }
        else
            transform.position = position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;

        ScheduleRow row = GetRow();
        if (row == null || row.SetScheduleField(this) == false)
        {
            _rectTransform.sizeDelta = _lastSize;
            _rectTransform.position = _lastPosition;
            transform.parent = _lastParent;
            transform.SetSiblingIndex(_lastChildCount);
        }
    }

    private ScheduleRow GetRow()
    {
        RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
        if (hit.transform != null && hit.transform.TryGetComponent(out ScheduleRow row))
        {
            return row;
        }
        return null;
    }

    private void UpdateTime()
    {
        float timeSpan = (_taskFieldUI.currentTask.taskTimeTo - _taskFieldUI.currentTask.taskTimeSince).ToClock().GetTotalMinutes();
        if (timeSpan <= 0)
            return;
        Vector2 size = new Vector2(timeSpan, _rectTransform.rect.size.y);
        _rectTransform.sizeDelta = size;
    }

    public void UpdateSize(float size)
    {
        _rectTransform.sizeDelta = new Vector2(size, _rectTransform.rect.size.y);
    }

}
