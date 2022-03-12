using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalCameraTexture : MonoBehaviour
{
    public Text cameraName;
    public Material cubeMat;
    private WebCamTexture webcam;
    private WebCamTexture webCamTexture;
    // Start is called before the first frame update
    void Start()
    {
        webcam = new WebCamTexture(Screen.width, Screen.height, 60);
        cubeMat.mainTexture = webcam;
        webcam.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.rotation *= Quaternion.AngleAxis(30 * Time.deltaTime, new Vector3(1, 2, 3) * 10);
    }

    private void ShowCameras()
    {
        foreach (WebCamDevice d in WebCamTexture.devices)
        {
            cameraName.text += d.name + (d.name == webCamTexture.deviceName ? "*" : "" + "\n");
        }
    }
}
