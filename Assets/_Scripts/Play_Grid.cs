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
    [SerializeField] const int _width = 50;
    [SerializeField] const int _depth = 50;

    public Vector2Int GridSize = new Vector2Int(_width, _depth);

    private Building[,] grid;
    private Building flyingBuilding;
    private Camera mainCamera;

    private GameObject[,] _PlayingGrid;


    private void Awake()
    {
        grid = new Building[GridSize.x, GridSize.y];
        mainCamera = Camera.main;
    }

    private void Start()
    {
        GameObject grid_holder = new GameObject();
        grid_holder.name = "Game_grid";
        for (int x = 0; x < _width; x++)
            for (int z = 0; z < _depth; z++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(x, -0.05f, z);
                cube.transform.localScale = new Vector3(0.9f, 0.1f, 0.9f);
                cube.transform.parent = grid_holder.transform;
            }
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        flyingBuilding = Instantiate(buildingPrefab);
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
            for (int x = 0; x < _width; x++)
            for (int z = 0; z < _depth; z++)
            {
                yield return new Vector3(x, 0, z);
            }
        }
    }
}


