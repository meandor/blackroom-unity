using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(AudioSource))]
public class GvrMicLevel : MonoBehaviour
{
    private const string DISPLAY_TEXT_FORMAT = "{0} Mic";
    private Text textField;
    private Camera playerCamera;
    private float loudness = 0;
    public float sensitivity = 100;
    public AudioSource audio;

    void Awake()
    {
        textField = GetComponent<Text>();
        audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        this.playerCamera = Camera.main;
        transform.SetParent(playerCamera.GetComponent<Transform>(), true);
        audio.clip = Microphone.Start("Built-in Microphone", true, 10, 44100);
        audio.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { }
        audio.Play();
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

    public float GetLoudness()
    {
        return this.loudness;
    }
}
