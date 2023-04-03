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
    
    // Start is called before the first frame update
    void Start()
    {
        _parent = GameObject.Find("GRID");
        _parent_play = _parent.GetComponent<play_grid>();
        _color_to_fill = _parent_play._cell_colors;

        _mesh_renderer.material.color = _color_to_fill.Evaluate(0.0f);
        
        _parrant_grid = GameObject.Find("GRID");
        
        _grid_size = _parrant_grid.GetComponent<play_grid>().GridSize;
        // Update is called once per frame
    }

    void Update()
    {
        GameObject[] _People_to_find;
        _People_to_find = GameObject.FindGameObjectsWithTag("People");
  
        //float _max_dist = Mathf.Sqrt(Mathf.Pow(_grid_size.x , 2f) + Mathf.Pow(_grid_size.y, 2f));

        float _max_dist = 50f;
        
        float _dist = 0f;
        for (int i = 0; i < _People_to_find.Length; i++)
        {
            _dist += Vector3.Distance(this.transform.position, _People_to_find[i].transform.position);
        }

        float _color = 0.0f;
        if (_People_to_find.Length > 0)
        {
            _color = _dist / _max_dist / _People_to_find.Length;
        }

        //_color = Mathf.Lerp(0f, _max_dist, _dist);
        _mesh_renderer.material.color = _color_to_fill.Evaluate(_color);
    }

    //float GetDistance(GameObject)
    //{
    //    
    //}
}
