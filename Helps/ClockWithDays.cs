using UnityEngine;

[System.Serializable]
public struct ClockWithDays
{

    public int day;
    public int hours;
    public int minutes;
    public int seconds;

    public bool isFormat12;

    public ClockWithDays(int day, Clock clock)
        : this(day, clock.hours, clock.minutes, clock.seconds, clock.isFormat12)
    {
    }

    public ClockWithDays(int day, int hours, int minutes, int seconds, bool isFormat12 = false)
    {
        hours = Mathf.Clamp(hours, 0, isFormat12 ? 12 : 24);
        minutes = Mathf.Clamp(minutes, 0, 60);
        seconds = Mathf.Clamp(seconds, 0, 60);

        this.day = day;
        this.hours = hours;
        this.minutes = minutes;
        this.seconds = seconds;
        this.isFormat12 = isFormat12;
    }

    public Clock ToClock() => new Clock(hours, minutes, seconds, isFormat12);

    public static ClockWithDays operator +(ClockWithDays ClockWithDays1, ClockWithDays ClockWithDays2)
    {
        ClockWithDays1.AddDays(ClockWithDays2.day);
        ClockWithDays1.AddHours(ClockWithDays2.hours);
        ClockWithDays1.AddMinutes(ClockWithDays2.minutes);
        ClockWithDays1.AddSeconds(ClockWithDays2.seconds);
        return ClockWithDays1;
    }

    public static ClockWithDays operator -(ClockWithDays ClockWithDays1, ClockWithDays ClockWithDays2)
    {
        if (ClockWithDays1.isFormat12 != ClockWithDays2.isFormat12 && ClockWithDays1.isFormat12)
            ClockWithDays2.FormatTo(true);

        int day = ClockWithDays1.day;
        int hours = ClockWithDays1.hours;
        int minutes = ClockWithDays1.minutes;
        int seconds = ClockWithDays1.seconds;

        seconds -= ClockWithDays2.seconds;
        if (seconds < 0)
        {
            seconds += 60;
            minutes--;
        }

        minutes -= ClockWithDays2.minutes;
        if (minutes < 0)
        {
            minutes += 60;
            hours--;
        }

        hours -= ClockWithDays2.hours;
        if (hours < 0)
        {
            hours += (ClockWithDays1.isFormat12 ? 12 : 24);
            day--;
        }

        day -= ClockWithDays2.day;

        return new ClockWithDays(day, hours, minutes, seconds, ClockWithDays1.isFormat12);
    }

    public static bool operator >(ClockWithDays ClockWithDays1, ClockWithDays ClockWithDays2) => (ClockWithDays1.day == ClockWithDays2.day && ClockWithDays1.GetTotalSeconds() > ClockWithDays2.GetTotalSeconds()) || ClockWithDays1.day > ClockWithDays2.day;
    public static bool operator <(ClockWithDays ClockWithDays1, ClockWithDays ClockWithDays2) => (ClockWithDays1.day == ClockWithDays2.day && ClockWithDays1.GetTotalSeconds() < ClockWithDays2.GetTotalSeconds()) || ClockWithDays1.day < ClockWithDays2.day;
    public static bool operator >=(ClockWithDays ClockWithDays1, ClockWithDays ClockWithDays2) => (ClockWithDays1.day == ClockWithDays2.day && ClockWithDays1.GetTotalSeconds() >= ClockWithDays2.GetTotalSeconds()) || ClockWithDays1.day > ClockWithDays2.day;
    public static bool operator <=(ClockWithDays ClockWithDays1, ClockWithDays ClockWithDays2) => (ClockWithDays1.day == ClockWithDays2.day && ClockWithDays1.GetTotalSeconds() <= ClockWithDays2.GetTotalSeconds()) && ClockWithDays1.day <= ClockWithDays2.day;
    public static bool operator ==(ClockWithDays ClockWithDays1, ClockWithDays ClockWithDays2) => ClockWithDays1.GetTotalSeconds() == ClockWithDays2.GetTotalSeconds() && ClockWithDays1.day == ClockWithDays2.day;
    public static bool operator !=(ClockWithDays ClockWithDays1, ClockWithDays ClockWithDays2) => ClockWithDays1.GetTotalSeconds() != ClockWithDays2.GetTotalSeconds() || ClockWithDays1.day != ClockWithDays2.day;

    public void AddDays(int value)
    {
        day += value;
    }

    public void AddHours(int value)
    {
        int result = hours + value;
        if (result >= 24)
        {
            int plusDays = result / 24;
            AddDays(plusDays);
            result -= plusDays * 24;
        }
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

    public double GetTotalHours() => hours + day * 24;
    public double GetTotalMinutes() => minutes + hours * 60 + day * 1440;
    public double GetTotalSeconds() => seconds + minutes * 60 + hours * 3600 + day * 86400;

    public override string ToString()
    {
        return $"{day} день {ValueToString(hours)}:{ValueToString(minutes)}";
    }

    public static string ValueToString(int value) => (value < 10 ? "0" : "") + value;

    public override bool Equals(object obj)
    {
        return obj is ClockWithDays ClockWithDays &&
               day == ClockWithDays.day &&
               hours == ClockWithDays.hours &&
               minutes == ClockWithDays.minutes &&
               seconds == ClockWithDays.seconds &&
               isFormat12 == ClockWithDays.isFormat12;
    }

}
