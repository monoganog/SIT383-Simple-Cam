using UnityEngine;
using System.Collections;

public class WebcamTest : MonoBehaviour
{
    // testing github in the browser
    // editing even more directly
    public string deviceName;
    WebCamTexture wct;
    public Material cubeMat;

    // Use this for initialization
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        deviceName = devices[0].name;
        wct = new WebCamTexture(deviceName, 400, 300, 12);
        cubeMat.mainTexture = wct;
        wct.Play();
    }

    // For photo varibles


    public Texture2D heightmap;
    public Vector3 size = new Vector3(100, 10, 100);


    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
            TakeSnapshot();

    }

    // For saving to the _savepath
    private string _SavePath = Application.persistentDataPath + "Pic"; //Change the path here!
    int _CaptureCounter = 0;

    void TakeSnapshot()
    {
        Texture2D snap = new Texture2D(wct.width, wct.height);
        snap.SetPixels(wct.GetPixels());
        snap.Apply();

        Debug.Log("here");
        System.IO.File.WriteAllBytes(_SavePath + _CaptureCounter.ToString() + ".png", snap.EncodeToPNG());
        ++_CaptureCounter;
    }
}
