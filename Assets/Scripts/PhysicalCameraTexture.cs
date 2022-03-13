using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalCameraTexture : MonoBehaviour
{
    public Text textOutput;
    public Material planeMat;
    private WebCamTexture webCamTexture;
    public Material pictureResultMat;
    public Image lastImage;

    private bool timer = false;


    private int currentCamera = 0;
    // Start is called before the first frame update
    void Start()
    {
        webCamTexture = new WebCamTexture(Screen.width, Screen.height, 60);
        ShowCameras();


        planeMat.mainTexture = webCamTexture;
        webCamTexture.Play();

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
            textOutput.text += d.name + (d.name == webCamTexture.deviceName ? "*" : "" + "\n");
        }
    }

    public void ToggleTimer()
    {
        if (timer)
        {
            !timer
        }
    }

    public void NextCamera()
    {
        currentCamera = (currentCamera + 1) %
            WebCamTexture.devices.Length;

        webCamTexture.Stop();
        webCamTexture.deviceName = WebCamTexture.devices[currentCamera].name;
        webCamTexture.Play();
        ShowCameras();


    }

    public Texture2D heightmap;
    public Vector3 size = new Vector3(100, 10, 100);



    // For saving to the _savepath

    int _CaptureCounter = 0;

    public void TakePicture()
    {
        if (timer)
        {
            StartCoroutine(TakePictureCoroutine());
        }

        SavePicture();
    }

    private void SavePicture(Texture2D texture)
    {
        textOutput.text = "Image Saved";
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "Pic" + _CaptureCounter.ToString() + ".png", texture.EncodeToPNG());
        ++_CaptureCounter;
    }

    IEnumerator TakePictureCoroutine()
    {
        Texture2D pictureResult = new Texture2D(webCamTexture.width, webCamTexture.height);
        pictureResult.SetPixels(webCamTexture.GetPixels());
        pictureResult.Apply();

        pictureResultMat.mainTexture = pictureResult;

        SavePicture();
    }



}
