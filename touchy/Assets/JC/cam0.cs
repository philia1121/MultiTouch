using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam0 : MonoBehaviour
{
    //public Quaternion baseRotation;
    // Start is called before the first frame update
    void Start()
    {
    
        WebCamTexture webcamTexture = new WebCamTexture();
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        //baseRotation = transform.rotation;
        webcamTexture.Play();
        //transform.rotation = baseRotation * Quaternion.AngleAxis(webcamTexture.videoRotationAngle, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
