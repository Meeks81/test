using UnityEngine;
using UnityEngine.Events;

public class MenuUI : MonoBehaviour
{

    [SerializeField] private ObjectPool<ButtonUI> _buttonsPool;

    public void ClearButtons()
    {
        _buttonsPool.HideEverything();
    }

    public void AddButton(Sprite sprite, UnityAction action)
    {
        ButtonUI btn = _buttonsPool.Get();
        btn.iconSprite = sprite;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(action);

        btn.gameObject.SetActive(true);
    }

}
