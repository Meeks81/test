using UnityEngine;
using UnityEngine.Events;

public class RegionAreaTrigger : MonoBehaviour
{

    public Vector2Int area;

    public UnityEvent<Vector2Int> onClick;

    private void OnMouseDown()
    {
        onClick.Invoke(area);
    }

}
