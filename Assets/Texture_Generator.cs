using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Texture_Generator : MonoBehaviour
{
    // Start is called before the first frame update

    //[SerializeField] private Texture2D localTexture;
    public Texture2D localTexture;
    void Start()
    {
        localTexture = new Texture2D(256,256);
        GetComponent<Renderer>().material.mainTexture = localTexture;
        localTexture.filterMode = FilterMode.Point;
        
        localTexture.SetPixel(128,128,Color.red);
        //localTexture.
        localTexture.Apply();
    }

    private void OnMouseDown()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray Camera_to_Mouse = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit Points_of_Hit;
        Collider this_box = this.GetComponent<Collider>();
        if (this_box.Raycast(Camera_to_Mouse, out Points_of_Hit, 500f))
        {
            Vector3 point_of_click = Points_of_Hit.point;
            localTexture.SetPixel(Convert.ToInt32(point_of_click.x), Convert.ToInt32(point_of_click.y), Color.green);
            localTexture.Apply();
        }
        

    }
}
