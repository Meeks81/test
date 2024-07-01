using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CargoBox : MonoBehaviour
{

    [SerializeField] private string _title;
    [SerializeField] private float _weight;
    [SerializeField] private Sprite _sprite;

    private BoxCollider _boxCollider;

    public string title => _title;
    public float weight => _weight;
    public Sprite sprite => _sprite;

    public BoxCollider GetCollider()
    {
        if (_boxCollider == null)
            _boxCollider = GetComponent<BoxCollider>();
        return _boxCollider;
    }

}
