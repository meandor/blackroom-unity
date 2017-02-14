using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(AudioSource))]
public class GvrMicLevel : MonoBehaviour
{
    private const string DISPLAY_TEXT_FORMAT = "{0} Mic";
    private Text textField;
    public AudioSource audio;
    public Camera cam;
    public float sensitivity = 100;
    public float loudness = 0;

    void Awake()
    {
        textField = GetComponent<Text>();
        audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        if (cam != null)
        {
            // Tie this to the camera, and do not keep the local orientation.
            transform.SetParent(cam.GetComponent<Transform>(), true);
        }

        //add the rest of the code like this
        audio.clip = Microphone.Start("Built-in Microphone", true, 10, 44100);
        audio.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { }
        audio.Play();

        //Do not Mute the audio Source.
    }

    float GetAveragedVolume()
    {
        float[] data = new float[256];
        float a = 0;
        audio.GetOutputData(data, 0);
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a / 256;
    }

    void LateUpdate()
    {
        loudness = GetAveragedVolume() * sensitivity;
        textField.text = string.Format(DISPLAY_TEXT_FORMAT, Mathf.RoundToInt(loudness));
    }
}
