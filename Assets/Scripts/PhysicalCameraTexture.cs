using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalCameraTexture : MonoBehaviour
{

    public Material cubeMat;
    private WebCamTexture webcam;

    // Start is called before the first frame update
    void Start()
    {
        webcam = new WebCamTexture();
        cubeMat.mainTexture = webcam;
        webcam.Play();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation *= Quaternion.AngleAxis(30 * Time.deltaTime, new Vector3(1, 2, 3) * 10);
    }
}
