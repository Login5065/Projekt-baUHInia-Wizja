using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    float mainSpeed = 10.0f;  
    float camSens = 0.25f; 
    private Vector3 lastMouse = new Vector3(255, 255, 255); //srodek ekranu
    private float totalRun = 1.0f;

    private void Start()
    {
        lastMouse = Input.mousePosition;
    }
    void Update()
    {
        if(Input.GetMouseButton(2))
        {
            lastMouse = Input.mousePosition - lastMouse;
            lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
            transform.eulerAngles = lastMouse;

        }
        lastMouse = Input.mousePosition;

        Vector3 p = GetBaseInput();
        p = p * mainSpeed;

        p = p * Time.deltaTime;
        transform.Translate(p);
    }

    private Vector3 GetBaseInput()
    { 
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
}
