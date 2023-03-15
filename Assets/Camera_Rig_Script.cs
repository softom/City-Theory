using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Rig_Script : MonoBehaviour
{
    public float moveTime = 10f;
    public float moveSpeed = 0.1f;
    public Vector3 newPosition;
    public Vector3 cameraPosition;
    public Camera cameraToWork;
    private Transform cameraTransform;
    
    public float rotationSpeed = 1f;
    public Quaternion rotationCamera;
    public Vector3 zoomAmmount;
    public float zoomMultiplay = 0.2f;

    public Vector3 currentMouseClick;
    public Vector3 clickMousePossition;

    public float mouseScrollSpeed = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        rotationCamera = transform.rotation;
        
        cameraTransform = cameraToWork.transform;
        cameraPosition = cameraTransform.localPosition;
        
        zoomAmmount = cameraTransform.TransformDirection(Vector3.forward).normalized;
        zoomAmmount *= zoomMultiplay;
        Debug.DrawRay(cameraTransform.position, zoomAmmount,Color.green);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
        HandleMouse();
    }

    public void HandleMouse()
    {
        float mouseScroll = Input.mouseScrollDelta.y;
        
        if (mouseScroll != 0)
        {
            cameraPosition += zoomAmmount*mouseScroll*mouseScrollSpeed;
            cameraToWork.transform.localPosition = cameraPosition;
            //cameraToWork.transform.localPosition = Vector3.Lerp(cameraToWork.transform.localPosition, cameraPosition, Time.deltaTime * moveTime);   
        }

        if (Input.GetMouseButtonDown(0))
        {
            Plane mousePlane = new Plane(Vector3.up, Vector3.zero);
            Debug.DrawRay(Vector3.zero, Vector3.up, Color.yellow, 30);
            Debug.DrawRay(Vector3.zero, Vector3.right, Color.green, 30);
            Debug.DrawRay(Vector3.zero, Vector3.forward, Color.green, 30);
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            Debug.DrawRay(cameraRay.origin, cameraRay.direction, Color.blue, 30f);
            //Debug.


            float entery;

            if (mousePlane.Raycast(cameraRay, out entery))
            {
                clickMousePossition = cameraRay.GetPoint(entery);
                Debug.DrawLine(cameraRay.origin, clickMousePossition, Color.red, 30f);
            }
        }

        if (Input.GetMouseButton(0))
        {
                Plane mousePlane = new Plane(Vector3.up, Vector3.zero);
                Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                float entery;

                if (mousePlane.Raycast(cameraRay, out entery))
                {
                    currentMouseClick = cameraRay.GetPoint(entery);
                    Vector3 newMousePosition = transform.position + clickMousePossition - currentMouseClick;
                    transform.position = newMousePosition;
                    newPosition = newMousePosition;
                }
        }
    }


    public void HandleInputs()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition -= (transform.forward * moveSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition -= (transform.right * moveSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * moveSpeed);
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * moveTime);
        
        //Вращение камерой
        if (Input.GetKey(KeyCode.Q))
        {
            rotationCamera *= Quaternion.Euler(Vector3.up * rotationSpeed);
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotationCamera *= Quaternion.Euler(Vector3.down * rotationSpeed);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, rotationCamera, Time.deltaTime * moveTime);

        //Пиблиэение и удаление
        if (Input.GetKey(KeyCode.R))
        {
            cameraPosition += zoomAmmount;
        }

        if (Input.GetKey(KeyCode.F))
        {
            cameraPosition -= zoomAmmount;
        }

        cameraToWork.transform.localPosition =
            Vector3.Lerp(cameraToWork.transform.localPosition, cameraPosition, Time.deltaTime * moveTime);
        

        //if (Physics.Raycast(t.position, look, out $$anonymous$$t, weaponlength, layerMask)) 
    }
}
