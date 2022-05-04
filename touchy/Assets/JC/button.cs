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
    private bool on;
    public void CursorOn()
    {
        CursorController.SetActive(on);
        on = !on;
    }
}
