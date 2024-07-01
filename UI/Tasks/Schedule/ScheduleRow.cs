using System.Collections.Generic;
using UnityEngine;

public class ScheduleRow : MonoBehaviour
{

    [SerializeField] private ScheduleField _scheduleFieldPrefab;
    [SerializeField] private RectTransform _timeLine;

    public UnloadCargoZone currentCargoZone { get; private set; }

    private List<ScheduleField> _fields = new List<ScheduleField>();

    private GameClock _gameClock;

    public void SetUnloadCargoZone(UnloadCargoZone cargoZone, GameClock gameClock)
    {
        currentCargoZone = cargoZone;
        _gameClock = gameClock;

        foreach (var item in _fields)
            Destroy(item.gameObject);
        _fields.Clear();

        foreach (var item in cargoZone.tasks)
        {
            ScheduleField field = Instantiate(_scheduleFieldPrefab, _timeLine);
            field.taskField.SetTask(item);

            float size = TimeToPosition((item.taskTimeTo - item.taskTimeSince).ToClock());
            field.GetRectTransform().position = new Vector2(GetTimeLineStartPosition().x + TimeToPosition(item.taskTimeSince.ToClock()) + size * 0.5f, _timeLine.position.y) +
            (new Vector2(size, field.GetRectTransform().rect.size.y) * (field.GetRectTransform().pivot - new Vector2(0.5f, 0.5f)));
        }
    }

    public void PlaceField(ScheduleField field, Vector2 newPosition)
    {
        Clock startTime = RoundTime(GetStartTimeForField(field, newPosition.x), 10);

        Clock timeSize = RoundTime(PositionToTime(field.GetRectTransform().rect.size.x), 10);
        float size = TimeToPosition(timeSize);

        Debug.Log(startTime);

        field.GetRectTransform().position = new Vector2(GetTimeLineStartPosition().x + TimeToPosition(startTime) + size * 0.5f, _timeLine.position.y) + 
            (new Vector2(size, field.GetRectTransform().rect.size.y) * (field.GetRectTransform().pivot - new Vector2(0.5f, 0.5f)));
        field.UpdateSize(size);
    }

    public bool SetScheduleField(ScheduleField field)
    {
        _fields.Add(field);
        Clock startTime = GetStartTimeForField(field);
        Clock finishTime = GetFinishTimeForField(field);
        field.task.SetAction(field.task.taskType, new ClockWithDays(_gameClock.Day, startTime), new ClockWithDays(_gameClock.Day, finishTime));
        field.UpdateSize(TimeToPosition(finishTime - startTime));
        field.transform.SetParent(_timeLine);

        currentCargoZone.AddTask(field.task);

        return true;
    }

    public Clock GetStartTimeForField(ScheduleField field) => GetStartTimeForField(field, field.GetRectTransform().position.x);
    public Clock GetStartTimeForField(ScheduleField field, float position)
    {
        return PositionToTime(position - GetTimeLineStartPosition().x - field.GetRectTransform().rect.size.x * 0.5f);
    }

    public Clock GetFinishTimeForField(ScheduleField field) => GetFinishTimeForField(field, field.GetRectTransform().position.x);
    public Clock GetFinishTimeForField(ScheduleField field, float position)
    {
        return PositionToTime(position - GetTimeLineStartPosition().x + field.GetRectTransform().rect.size.x * 0.5f);
    }

    public float TimeToPosition(Clock time)
    {
        return _timeLine.rect.size.x / (24 * 60) * time.GetTotalMinutes();
    }

    public Clock PositionToTime(float pos)
    {
        float minutes = (24 * 60) / _timeLine.rect.size.x * pos;
        float hours = Mathf.Floor(minutes / 60f);
        minutes -= hours * 60f;
        return new Clock((int)hours, (int)minutes, 0);
    }

    private Clock RoundTime(Clock time, int divider)
    {
        return new Clock(time.hours, Mathf.FloorToInt(time.minutes / divider) * divider, time.seconds);
    }

    private Vector2 GetTimeLineStartPosition() => (Vector2)_timeLine.position - _timeLine.rect.size * 0.5f;

}
