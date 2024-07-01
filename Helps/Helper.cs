using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Helper
{

    public static List<string> ReduceChars { get; private set; } = new List<string>() { "K", "M", "B", "T", "Q" };

    public static Vector2 GetVectorByAngle(float angle)
    {
        return new Vector2((float)Mathf.Cos(Mathf.Deg2Rad * angle), (float)Mathf.Sin(Mathf.Deg2Rad * angle));
    }
    public static float GetAngleByDirection(Vector2 direction)
    {
        return (float)(Mathf.Atan2(direction.normalized.y, direction.normalized.x) / (2 * Mathf.PI)) * 360f;
    }
    public static void AddEventTrigger(EventTrigger trigger, EventTriggerType type, UnityAction<BaseEventData> method)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(method);
        trigger.triggers.Add(entry);
    }
    public static Vector2 Normalize(Vector2 vec)
    {
        if (Mathf.Abs(vec.x) + Mathf.Abs(vec.y) < 1f)
            vec *= Mathf.Ceil(1f / (Mathf.Abs(vec.x) + Mathf.Abs(vec.y)));
        float length = (float)Mathf.Sqrt((vec.x * vec.x) + (vec.y * vec.y));
        if (length != 0)
            vec = new Vector2(vec.x / length, vec.y / length);
        return vec;
    }

    public static float RangeTo(float min1, float max1, float min2, float max2, float value, bool clamp = false)
    {
        float res = 1f / (max1 - min1) * (value - min1) * (max2 - min2) + min2;
        if (clamp)
            return Mathf.Clamp(res, min2, max2);
        else
            return res;
    }

    public static void Flip<T>(ref T value1, ref T value2)
    {
        T temp = value1;
        value1 = value2;
        value2 = temp;
    }

    public static int GetDecimalDigitsCount(float value)
    {
        float v = (float)System.Math.Round(value % 1, 5);
        if (v == 0)
            return 0;
        return v.ToString().Substring(2).Length;
    }

    public static float GetMaxFloatInList(this List<float> list)
    {
        float maxNow = float.MinValue;
        foreach (var item in list)
            if (item > maxNow)
                maxNow = item;
        return maxNow;
    }
    public static float GetMinFloatInList(this List<float> list)
    {
        float minNow = float.MaxValue;
        foreach (var item in list)
            if (item < minNow)
                minNow = item;
        return minNow;
    }

    public static int MaxFloatInList(List<float> list)
    {
        int index = -1;
        float maxNow = float.MinValue;
        for (int i = 0; i < list.Count; i++)
            if (list[i] > maxNow)
            {
                maxNow = list[i];
                index = i;
            }
        return index;
    }
    public static int MinFloatInList(List<float> list)
    {
        int index = -1;
        float minNow = float.MaxValue;
        for (int i = 0; i < list.Count; i++)
            if (list[i] < minNow)
            {
                minNow = list[i];
                index = i;
            }
        return index;
    }

    public static string ToTimeText(int value)
    {
        int h = value / 3600;
        int m = value / 60 - h * 60;
        int s = value - m * 60 - h * 3600;
        return (h > 0 ? $"{h}:" : "") + (m > 0 ? $"{(m < 10 ? "0" : "")}{m}:" : "") + $"{(s < 10 ? "0" : "")}{s}";
    }

    public static string ValueReduce(double value, int roundCount = 2)
    {
        string valueString = value.ToString();
        int bitDepth = (valueString.Split(',')[0].Length - 1) / 3;
        if (bitDepth == 0)
            return System.Math.Round(value, roundCount).ToString();

        valueString = value.ToString().Split(',')[0];

        string integer = valueString.Remove(valueString.Length - bitDepth * 3);
        string remainder = valueString.Remove(0, integer.Length).Remove(roundCount);
        int reduceCharIndex = Mathf.Clamp(bitDepth - 1, 0, ReduceChars.Count - 1);
        return $"{integer},{remainder}{ReduceChars[reduceCharIndex]}";
    }

    public static int GetRandomByCoef(int min, int max, float coef) => (int)System.Math.Round(GetRandomByCoef((float)min, (float)max, coef), 0);
    public static float GetRandomByCoef(float min, float max, float coef)
    {
        coef = Mathf.Clamp(coef, 0f, 1f);
        float randValue = Random.Range(min, max);
        float coefValue = 0;
        if (coef > 0.5f)
            coefValue = randValue + (max - randValue) * Mathf.Lerp(coef - 0.5f, 0f, 1f / (max - min) * (randValue - min));
        else
            coefValue = randValue - (randValue - min) * Mathf.Lerp(0.5f - coef, 0f, 1f / (max - min) * (max - randValue));
        return Random.Range(randValue, coefValue);
    }

    public static float ToMult(this float value) => 1f + value / 100f;
    public static float ToMinusMult(this float value) => 1f - value / 100f;
    public static float ToPercent(this float value) => value * 100f - 1f;

    public static int GetMin(List<int> values)
    {
        int currentMin = int.MaxValue;
        foreach (var item in values)
            if (item < currentMin)
                currentMin = item;
        return currentMin;
    }
    public static float GetMin(List<float> values)
    {
        float currentMin = float.MaxValue;
        foreach (var item in values)
            if (item < currentMin)
                currentMin = item;
        return currentMin;
    }
    public static double GetMin(List<double> values)
    {
        double currentMin = double.MaxValue;
        foreach (var item in values)
            if (item < currentMin)
                currentMin = item;
        return currentMin;
    }

    public static int GetMax(List<int> values)
    {
        int currentMax = int.MinValue;
        foreach (var item in values)
            if (item > currentMax)
                currentMax = item;
        return currentMax;
    }
    public static float GetMax(List<float> values)
    {
        float currentMax = float.MinValue;
        foreach (var item in values)
            if (item > currentMax)
                currentMax = item;
        return currentMax;
    }
    public static double GetMax(List<double> values)
    {
        double currentMax = double.MinValue;
        foreach (var item in values)
            if (item > currentMax)
                currentMax = item;
        return currentMax;
    }

    public static void CorrectGrid(GridLayoutGroup grid, int itemsCount)
    {
        RectTransform gridRect = grid.GetComponent<RectTransform>();
        if (grid.startAxis == GridLayoutGroup.Axis.Horizontal)
        {
            float spaceForItemsInRow = gridRect.rect.size.x - grid.padding.left - grid.padding.right;
            int maxItemsInRow = (int)(spaceForItemsInRow / ((grid.cellSize.x + grid.spacing.x) - grid.spacing.x));
            if (maxItemsInRow <= 0)
                maxItemsInRow = 1;

            int rowsCount = Mathf.CeilToInt((float)itemsCount / maxItemsInRow);
            gridRect.sizeDelta = new Vector2(gridRect.sizeDelta.x, rowsCount * (grid.cellSize.y + grid.spacing.y));
        }
        else
        {
            float spaceForItemsInRow = gridRect.rect.size.y - grid.padding.top - grid.padding.bottom;
            int maxItemsInRow = (int)(spaceForItemsInRow / ((grid.cellSize.y + grid.spacing.y) - grid.spacing.y));
            if (maxItemsInRow <= 0)
                maxItemsInRow = 1;

            int rowsCount = Mathf.CeilToInt((float)itemsCount / maxItemsInRow);
            gridRect.sizeDelta = new Vector2(rowsCount * (grid.cellSize.x + grid.spacing.x), gridRect.sizeDelta.y);
        }
    }

    public class Triangle
    {
        public static float GetAngle(float leftSite, float rightSite, float footing)
        {
            float a = rightSite;
            float c = leftSite;
            float b = footing;
            return Mathf.Acos((a * a + c * c - b * b) / (2 * a * c)) * 180f / Mathf.PI;
        }
    }

    public class RightTriangle
    {
        public static float GetAngle(float cathet, float hypotenuse)
        {
            return Mathf.Acos(cathet / hypotenuse) * 180 / Mathf.PI;
        }
    }

}
