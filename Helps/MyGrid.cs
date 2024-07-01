using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class MyGrid : LayoutGroup
{

    public enum Alignment
    {
        left, center, right, fullWidth
    }

    [SerializeField] private Alignment alignment;
    [SerializeField] private Vector2 spacing;

    private RectTransform _rectTransform;
    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }

    private void UpdateUI()
    {
        float height = padding.top;
        List<Row> grid = new List<Row>() { new Row(padding.left, 0) };
        float xSpacing = spacing.x;
        if (alignment == Alignment.fullWidth)
            xSpacing = 0;
        foreach (RectTransform item in transform)
        {
            m_Tracker.Add(this, item,
                DrivenTransformProperties.Anchors |
                DrivenTransformProperties.AnchoredPosition |
                DrivenTransformProperties.Pivot);
            if (item.gameObject.activeSelf == false)
                continue;
            item.anchorMin = new Vector2(0f, 1f);
            item.anchorMax = new Vector2(0f, 1f);
            item.pivot = new Vector2(0f, 1f);
            if (grid[grid.Count - 1].width + item.rect.size.x + xSpacing + padding.right > RectTransform.rect.size.x)
            {
                height += grid[grid.Count - 1].height + spacing.y;
                grid.Add(new Row(padding.left, 0));
            }
            grid[grid.Count - 1].items.Add(item);
            grid[grid.Count - 1].width += item.rect.size.x + xSpacing;
            if (item.rect.size.y > grid[grid.Count - 1].height)
                grid[grid.Count - 1].height = item.rect.size.y;
        }
        height += grid[grid.Count - 1].height;
        RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, height + padding.bottom);

        height = padding.top;
        foreach (var row in grid)
        {
            float width = padding.left;
            if (alignment == Alignment.center)
                width = -row.width / 2f + spacing.x / 2f;
            else if (alignment == Alignment.fullWidth)
            {
                width = -RectTransform.rect.size.x / 2f;
                if (row.items.Count == 1)
                    width = -row.items[0].rect.size.x / 2f;
                xSpacing = (RectTransform.rect.size.x - row.width) / (row.items.Count - 1);
            }    
            foreach (var item in row.items)
            {
                SetPivot(item);
                float x = width + item.rect.size.x * item.pivot.x;
                item.anchoredPosition = new Vector2(alignment == Alignment.right ? -width : x, -height);
                width += item.rect.size.x + xSpacing;
            }
            height += row.height + spacing.y;
        }
    }

    private void SetPivot(RectTransform item)
    {
        switch (alignment)
        {
            case Alignment.left:
                item.anchorMin = new Vector2(0f, 1f);
                item.anchorMax = new Vector2(0f, 1f);
                item.pivot = new Vector2(0f, 1f);
                break;
            case Alignment.center:
                item.anchorMin = new Vector2(0.5f, 1f);
                item.anchorMax = new Vector2(0.5f, 1f);
                item.pivot = new Vector2(0.5f, 1f);
                break;
            case Alignment.right:
                item.anchorMin = new Vector2(1f, 1f);
                item.anchorMax = new Vector2(1f, 1f);
                item.pivot = new Vector2(1f, 1f);
                break;
            case Alignment.fullWidth:
                item.anchorMin = new Vector2(0.5f, 1f);
                item.anchorMax = new Vector2(0.5f, 1f);
                item.pivot = new Vector2(0.5f, 1f);
                break;
            default:
                break;
        }
    }

    public override void CalculateLayoutInputVertical()
    {
        UpdateUI();
    }

    public override void SetLayoutHorizontal()
    {
        UpdateUI();
    }

    public override void SetLayoutVertical()
    {
        UpdateUI();
    }

    private class Row
    {
        public float width;
        public float height;
        public List<RectTransform> items = new List<RectTransform>();

        public Row(float width, float height)
        {
            this.width = width;
            this.height = height;
        }
    }

}