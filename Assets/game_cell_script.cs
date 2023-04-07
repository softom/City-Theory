using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class game_cell_script : MonoBehaviour
{
    private GameObject _parent;
    private play_grid _parent_play;
    public Gradient _color_to_fill;
    public GameObject _mesh_to_render;
    public Renderer _mesh_renderer;
    public GameObject _parrant_grid;
    public Vector2 _grid_size;
    
    public float _p_ion;
    public float _p_dist;

    public Vector2 _pos;

    public float _color;
    public float _ion;
    public float _total_dist;
    
    // Start is called before the first frame update
    void Start()
    {
        _parent = GameObject.Find("GRID");
        _parent_play = _parent.GetComponent<play_grid>();
        _color_to_fill = _parent_play._cell_colors;

        _mesh_renderer.material.color = _color_to_fill.Evaluate(0.0f);
        
        _parrant_grid = GameObject.Find("GRID");
        
        _grid_size = _parrant_grid.GetComponent<play_grid>().GridSize;
        //_max_dist = _parrant_grid.GetComponent<play_grid>()._distance;
        // Update is called once per frame
    }

    void Update()
    {
        if (_parent_play._max_dist != 0)
            _mesh_renderer.material.color = _color_to_fill.Evaluate(_color / _parent_play._max_dist);
        else
            _mesh_renderer.material.color = _color_to_fill.Evaluate(0);
        
        if (_ion != null)
        {
            Vector3 _pos = this.transform.position;
            if (_parent_play._max_ion != 0)
                _pos.y = 0 - 10 * (_ion - _parent_play._min_ion);
            this.transform.position = _pos;
        }
    }
}
