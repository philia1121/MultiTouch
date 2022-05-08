using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class button : MonoBehaviour
{
    public void quit()
    {
        Application.Quit();
    }

    public GameObject[] triangles;
    public void clearall()
    {
        triangles = GameObject.FindGameObjectsWithTag("triangle");
        foreach(var tri in triangles)
        {
            Destroy(tri);
        }
    }

    public GameObject CursorController;
    [SerializeField]private bool on;
    public int counting = 0;
    public void CursorOn()
    {
        counting++;
        if(counting%2 != 0)
        {
            CursorController.SetActive(on);
            on = !on;
        }
        
    }

    public bool camOn = false;
    public int c2 = 0;
    public void openCam()
    {
        c2++;
        if(c2%2 != 0)
        {
            camOn = !camOn;
        }
        
    }
}
