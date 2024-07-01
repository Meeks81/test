using System.Collections.Generic;
using UnityEngine;

public class TransparentBuilding : MonoBehaviour
{

    private List<Render> _renders;

    private void Awake()
    {
        UpdateRenderers();
    }

    public void UpdateRenderers()
    {
        _renders = new List<Render>();

        foreach (var item in GetComponentsInChildren<Renderer>(false))
        {
            _renders.Add(new Render(item));
        }
    }
    
    public void SetTransparent(Color color, float alpha)
    {
        foreach (var item in _renders)
            item.SetTransparent(color, alpha);
    }

    public void SetNormal()
    {
        foreach (var item in _renders)
            item.SetNormal();
    }

    private class Render
    {
        public Renderer renderer;

        private Color[] _colors;

        public Render(Renderer renderer)
        {
            this.renderer = renderer;

            _colors = new Color[renderer.materials.Length];
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                _colors[i] = renderer.materials[i].color;
            }
        }

        public void SetTransparent(Color color, float alpha)
        {
            color.a = alpha;
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                renderer.materials[i].color = color;
            }
        }

        public void SetNormal()
        {
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                renderer.materials[i].color = _colors[i];
            }
        }
    }

}
