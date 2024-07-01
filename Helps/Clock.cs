using UnityEngine;

[System.Serializable]
public struct Clock
{

    public int hours;
    public int minutes;
    public int seconds;

    public bool isFormat12;

    public Clock(int hours, int minutes, int seconds, bool isFormat12 = false)
    {
        hours = Mathf.Clamp(hours, 0, isFormat12 ? 12 : 24);
        minutes = Mathf.Clamp(minutes, 0, 60);
        seconds = Mathf.Clamp(seconds, 0, 60);

        this.hours = hours;
        this.minutes = minutes;
        this.seconds = seconds;
        this.isFormat12 = isFormat12;
    }

    public static Clock operator +(Clock clock1, Clock clock2)
    {
        clock1.AddSeconds(clock2.GetTotalSeconds());
        return clock1;
    }

    public static Clock operator -(Clock clock1, Clock clock2)
    {
        if (clock1.isFormat12 != clock2.isFormat12 && clock1.isFormat12)
            clock2.FormatTo(true);

        int hours = clock1.hours;
        int minutes = clock1.minutes;
        int seconds = clock1.seconds;

        seconds -= clock2.seconds;
        if (seconds < 0)
        {
            seconds += 60;
            minutes--;
        }

        minutes -= clock2.minutes;
        if (minutes < 0)
        {
            minutes += 60;
            hours--;
        }

        hours -= clock2.hours;
        if (hours < 0)
            hours += (clock1.isFormat12 ? 12 : 24);

        return new Clock(hours, minutes, seconds, clock1.isFormat12);
    }

    public static bool operator >(Clock clock1, Clock clock2) => clock1.GetTotalSeconds() > clock2.GetTotalSeconds();
    public static bool operator <(Clock clock1, Clock clock2) => clock1.GetTotalSeconds() < clock2.GetTotalSeconds();
    public static bool operator >=(Clock clock1, Clock clock2) => clock1.GetTotalSeconds() >= clock2.GetTotalSeconds();
    public static bool operator <=(Clock clock1, Clock clock2) => clock1.GetTotalSeconds() <= clock2.GetTotalSeconds();
    public static bool operator ==(Clock clock1, Clock clock2) => clock1.GetTotalSeconds() == clock2.GetTotalSeconds();
    public static bool operator !=(Clock clock1, Clock clock2) => clock1.GetTotalSeconds() != clock2.GetTotalSeconds();

    public void AddHours(int value)
    {
        int result = hours + value;
        result -= result / (isFormat12 ? 12 : 24) * (isFormat12 ? 12 : 24);
        hours = result;
    }

    public void AddMinutes(int value)
    {
        int result = minutes + value;
        if (result >= 60)
        {
            int plusHours = result / 60;
            AddHours(plusHours);
            result -= plusHours * 60;
        }
        minutes = result;
    }

    public void AddSeconds(int value)
    {
        int result = seconds + value;
        if (result >= 60)
        {
            int plusMinutes = result / 60;
            AddMinutes(plusMinutes);
            result -= plusMinutes * 60;
        }
        seconds = result;
    }

    public void FormatTo(bool isFormat12)
    {
        if (isFormat12 == this.isFormat12)
            return;

        if (isFormat12 && hours > 12)
            hours -= 12;
    }

    public int GetTotalMinutes() => minutes + hours * 60;
    public int GetTotalSeconds() => seconds + minutes * 60 + hours * 3600;

    public override string ToString()
    {
        return $"{ValueToString(hours)}:{ValueToString(minutes)}";
    }

    public static string ValueToString(int value) => (value < 10 ? "0" : "") + value;

    public override bool Equals(object obj)
    {
        return obj is Clock clock &&
               hours == clock.hours &&
               minutes == clock.minutes &&
               seconds == clock.seconds &&
               isFormat12 == clock.isFormat12;
    }
}
