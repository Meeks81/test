using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonUI : Selectable, IPointerClickHandler
{

    [SerializeField] private Image _iconImage;
    [SerializeField] private Sprite _iconSprite;
    [Space]
    public UnityEvent onClick;

    public Sprite iconSprite
    {
        get => _iconSprite;
        set
        {
            _iconSprite = value;
            if (_iconImage != null)
                _iconImage.sprite = value;
        }
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        if (_iconImage != null)
            _iconImage.sprite = _iconSprite;
    }
#endif

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick.Invoke();
    }

}
