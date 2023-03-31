using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Texture_Generator : MonoBehaviour
{
    // Start is called before the first frame update

    //[SerializeField] private Texture2D localTexture;
    [SerializeField] private Texture2D localTexture;
    [SerializeField] private int resolution;

    public Gradient GD;
    
    
    void Start()
    {
        if (resolution == 0) resolution = 1;
        RegenTexture();
    }

    private void RegenTexture()
    {
        localTexture = new Texture2D(resolution,resolution);
        GetComponent<Renderer>().material.mainTexture = localTexture;
        localTexture.filterMode = FilterMode.Point;
        
        localTexture.SetPixel(Convert.ToInt32(resolution/2-0.5f),Convert.ToInt32(resolution/2-0.5f),Color.red);

        localTexture.Apply();      
    }
    private void OnValidate()
    {
        RegenTexture();
    }

    private void OnMouseDown()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray Camera_to_Mouse = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit Points_of_Hit  ;
        Collider this_box = this.GetComponent<Collider>();
        if (this_box.Raycast(Camera_to_Mouse, out Points_of_Hit, 500f))
        {
            Vector3 point_of_click = Points_of_Hit.point;
            Vector2 texture_point_of_click = Points_of_Hit.textureCoord;
            //Vector3 local_point_of_click = this.transform.InverseTransformPoint(point_of_click);
            //Vector2 point_to_paint;
            print(texture_point_of_click.x);
            print(texture_point_of_click.y);
            print("---");
            //float x_prop = Mathf.InverseLerp(-transform.localScale.x/2, this.transform.localScale.x/2, local_point_of_click.x);
            //point_to_paint.x = resolution * (0.5f + local_point_of_click.x);
            //point_to_paint.y = resolution * (0.5f + local_point_of_click.y);
            //point_to_paint.y = resolution * Mathf.InverseLerp(-transform.localScale.x/2, this.transform.localScale.y/2, local_point_of_click.y);
            
            localTexture.SetPixel(Convert.ToInt32(resolution * texture_point_of_click.x), Convert.ToInt32(resolution * texture_point_of_click.y), Color.green);
            localTexture.Apply();
        }
        

    }
}
