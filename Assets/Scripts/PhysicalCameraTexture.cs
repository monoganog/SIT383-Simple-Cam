using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalCameraTexture : MonoBehaviour
{
    [Header("Camera Settings")]
    public Text textOutput;
    public Material planeMat;
    private WebCamTexture webCamTexture;
    public Material pictureResultMat;

    public GameObject cameraPlaneMain, cameraPlanePreview;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip shutter, timeIncrement;

    [Header("UI Images")]
    public Image DelayImage;
    public Text countDownText;
    public Sprite noneSprite, threeSprite, tenSprite;

    // Private members
    private int enumIndex = 0;
    private float timeLeft;
    private float nextActionTime = 0.0f;
    private float period = 1f;
    private int captureCount = 0;
    private int currentCamera = 0;
    private bool countdownEnabled = false;


    public enum Delay
    {
        none = 0,
        three = 3,
        ten = 10
    }
    Delay delay = Delay.none;


    void Start()
    {
        countDownText.text = "";
        webCamTexture = new WebCamTexture();
        ShowCameras();
        PrintDebugText("Texture rotated to: " + webCamTexture.videoRotationAngle);
        PrintDebugText("Is Video Mirrored: " + webCamTexture.videoVerticallyMirrored);
        PrintDebugText("Video Dimensions: " + webCamTexture.dimension);

        planeMat.mainTexture = webCamTexture;
        webCamTexture.Play();

        PrintDebugText("Scene started");
    }

    // Update is called once per frame
    void Update()
    {
        if (countdownEnabled)
        {
            timeLeft -= Time.deltaTime;
            countDownText.text = (timeLeft).ToString("0");

            // Play Audio every second
            if (Time.time > nextActionTime)
            {
                nextActionTime += period;
                audioSource.PlayOneShot(timeIncrement);
            }

            // Time to take picture
            if (timeLeft < 1)
            {
                TakePicture();
            }
        }
    }

    public void RotateCamera()
    {

        Vector3 rotateAmmount = new Vector3(90, 0,0);
        cameraPlaneMain.transform.rotation *= Quaternion.Euler(0, 90, 0);
        cameraPlanePreview.transform.rotation *= Quaternion.Euler(0, 90, 0);
    }

    private void TakePicture()
    {
        audioSource.PlayOneShot(shutter);

        countdownEnabled = false;
        countDownText.text = "";

        Texture2D pictureResult = new Texture2D(webCamTexture.width, webCamTexture.height);
        pictureResult.SetPixels(webCamTexture.GetPixels());
        pictureResult.Apply();

        pictureResultMat.mainTexture = pictureResult;

        SavePicture(pictureResult);
    }

    public void PressDelay()
    {
        enumIndex = (enumIndex + 1) % 3;

        switch (enumIndex)
        {
            case 0:
                delay = Delay.none;

                DelayImage.sprite = noneSprite;
                break;

            case 1:
                delay = Delay.three;
                DelayImage.sprite = threeSprite;
                break;

            case 2:
                delay = Delay.ten;
                DelayImage.sprite = tenSprite;
                break;
        }
        Debug.Log(delay.ToString());
        PrintDebugText("Delay set to " + delay.ToString());
    }

    public void PrintDebugText(string message)
    {
        textOutput.text += "\n\n" + message;
    }

    private void ShowCameras()
    {
        foreach (WebCamDevice d in WebCamTexture.devices)
        {
            //textOutput.text += d.name + (d.name == webCamTexture.deviceName ? "*" : "" + "\n");
            //PrintDebugText(d.name + " Camera selected");
        }
    }
    public void NextCamera()
    {
        PrintDebugText("Switch camera pressed");

        
        currentCamera = (currentCamera + 1) %
            WebCamTexture.devices.Length;

        webCamTexture.Stop();
        PrintDebugText("Camera selected is: " + WebCamTexture.devices[currentCamera].name);

        webCamTexture.deviceName = WebCamTexture.devices[currentCamera].name;
        if (webCamTexture.isReadable && !webCamTexture.isPlaying)
        {
            
            webCamTexture.Play();
        }
   
        //ShowCameras();
    }

    public void PressTakePicture()
    {
        PrintDebugText("Take picture pressed");
        nextActionTime = Time.time;
        timeLeft = (int)delay;
        countdownEnabled = true;
       

    }

    private void SavePicture(Texture2D texture)
    {
        PrintDebugText("Image Saved at: " + Application.persistentDataPath);
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/Pic" + captureCount.ToString() + ".png", texture.EncodeToPNG());
        ++captureCount;
    }

}
