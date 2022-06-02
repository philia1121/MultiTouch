using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam0 : MonoBehaviour
{
    [SerializeField]private int id; //指定要用的webcam序號
    [SerializeField]private Material mat;
    //public Quaternion baseRotation;
    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        //showDevicesName(devices);
        WebCamTexture webcamTexture = new WebCamTexture(devices[id].name);
        //Renderer renderer = GetComponent<Renderer>();
        //renderer.material.mainTexture = webcamTexture;
        mat.mainTexture = webcamTexture;
        webcamTexture.Play();
        
    }

    void showDevicesName(WebCamDevice[] devices)
    {
        foreach(var device in devices)
        {
            print("["+ System.Array.IndexOf (devices, device) +"], "+device.name); //用來看總共有多少可用webcam
        }
    }
}
