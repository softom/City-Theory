using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class play_grid : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool _to_draw_Gizmos;

    public GameObject _cell_to_fill;
    public Gradient _cell_colors;
    

    public Vector2Int GridSize;

    public float _distance;

    private Building[,] grid;
    private GameObject[,] _to_grid;

    private Building flyingBuilding;
    private Camera mainCamera;

    private GameObject[,] _PlayingGrid;
    
    public float _min_dist, _max_dist;
    public float _min_ion, _max_ion;

    public float[][,] _education_fild;

    private void GridUpdate()
    {
        GameObject[] _People_to_find;
        _People_to_find = GameObject.FindGameObjectsWithTag("People");
  
        float _dist ;
        float _ion ;

       
        _min_dist = 10000f;
        _max_dist = 0f;
        _min_ion = 10000f;
        _max_ion = 0f;
        
        
        for (int x = 0; x < GridSize.x; x++)
            for (int y = 0; y < GridSize.y; y++)
            {
                _dist = 0f;
                _ion = 0f;

                
                for (int i = 0; i < _People_to_find.Length; i++)
                {
                    float _prom = Vector3.Distance(_to_grid[x, y].transform.position, _People_to_find[i].transform.position);
                    _dist += Mathf.Abs(_prom);
                    if (_prom > 0.01)
                        _ion += Mathf.Log(_prom, 10);
                    else
                        _ion += 0;
                }

                if (_ion < 0)
                    _ion = 0;

                if (_max_dist < _dist)
                    _max_dist = _dist;
                if (_min_dist > _dist)
                    _min_dist = _dist;
                
                if (_max_ion < _ion)
                    _max_ion = _ion;
                if (_min_ion > _ion)
                    _min_ion = _ion;
                
                _to_grid[x, y].GetComponent<game_cell_script>()._total_dist = _dist;
                _to_grid[x, y].GetComponent<game_cell_script>()._ion = _ion;
                _to_grid[x, y].GetComponent<game_cell_script>()._color = _dist ;
            }
    }

        private void Awake()
    {
        grid = new Building[GridSize.x, GridSize.y];
        _to_grid = new GameObject[GridSize.x, GridSize.y];
        _education_fild = new float[][,] {
            new float[GridSize.x, GridSize.y],
            new float[GridSize.x, GridSize.y],
            new float[GridSize.x, GridSize.y]
        };
        
        mainCamera = Camera.main;
    }

    private void Start()
    {
        GameObject _cell;

        for (int x = 0; x < GridSize.x; x++)
            for (int z = 0; z < GridSize.y; z++)
            {
                _cell = Instantiate(_cell_to_fill, new Vector3(x, 0, z), new Quaternion(0,0,0,0) ,this.transform);
                String _name = "x" + x.ToString() + "_y" + z.ToString();
                _to_grid[x, z] = _cell;
                _to_grid[x,z].name = _name;
            }
        //GridUpdate();
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        flyingBuilding = Instantiate(buildingPrefab);
        Animator flying_persot = flyingBuilding.GetComponentInChildren<Animator>();
        flying_persot.SetBool("Trowing",true);
        print("Throw_person");
        
    }

    private void Update()
    {
        if (flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                bool available = true;

                if (x < 0 || x > GridSize.x - flyingBuilding.Size.x) available = false;
                if (y < 0 || y > GridSize.y - flyingBuilding.Size.y) available = false;

                if (available && IsPlaceTaken(x, y)) available = false;

                flyingBuilding.transform.position = new Vector3(x, 0, y);
                flyingBuilding.SetTransparent(available);

                if (available && Input.GetMouseButtonDown(0))
                {
                    PlaceFlyingBuilding(x, y);
                }
            }
        }
        
        GridUpdate();
        
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                if (grid[placeX + x, placeY + y] != null) return true;
            }
        }

        return false;
    }

    private void PlaceFlyingBuilding(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                grid[placeX + x, placeY + y] = flyingBuilding;
            }
        }
        Animator flying_persot = flyingBuilding.GetComponentInChildren<Animator>();
        flying_persot.SetBool("Trowing",false);
        print("Throw_person_out");
        
        flyingBuilding.SetNormal();
        flyingBuilding = null;
        

    }


    private void OnDrawGizmos()
    {
        if (!_to_draw_Gizmos || Application.isPlaying) return;

        foreach (var point in EvaluateGridPoints())
        {
            Gizmos.DrawWireCube(point, new Vector3(1, 0, 1));
        }

        IEnumerable<Vector3> EvaluateGridPoints()
        {
            for (int x = 0; x < GridSize.x; x++)
            for (int z = 0; z < GridSize.y; z++)
            {
                yield return new Vector3(x, 0, z);
            }
        }
    }
}


