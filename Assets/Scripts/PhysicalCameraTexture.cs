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
        webCamTexture = new WebCamTexture(Screen.width, Screen.height, 60);
        ShowCameras();


        planeMat.mainTexture = webCamTexture;
        webCamTexture.Play();

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

    private void TakePicture()
    {
        audioSource.PlayOneShot(shutter);
        countdownEnabled = false;
        countDownText.text = "";
        // Delay over time to take picture
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
    }

    private void ShowCameras()
    {
        foreach (WebCamDevice d in WebCamTexture.devices)
        {
            textOutput.text += d.name + (d.name == webCamTexture.deviceName ? "*" : "" + "\n");
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





    public void PressTakePicture()
    {
        nextActionTime = Time.time;
        timeLeft = (int)delay;
        countdownEnabled = true;
    }

    private void SavePicture(Texture2D texture)
    {
        textOutput.text = "Image Saved";
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/Pic" + captureCount.ToString() + ".png", texture.EncodeToPNG());
        Debug.Log(Application.persistentDataPath);
        ++captureCount;
    }

}
