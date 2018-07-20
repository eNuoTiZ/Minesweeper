using UnityEngine;
using UnityEngine.UI;

public class Mute : MonoBehaviour
{

    public Toggle currentToggle;

    // Use this for initialization
    void Start()
    {
        if (currentToggle != null)
        {
            currentToggle.onValueChanged.AddListener(delegate { MuteAll(); });
        }
    }

    // Update is called once per frame
    void MuteAll()
    {
        currentToggle.GetComponentInChildren<Image>().color = currentToggle.isOn ? new Color(0.000f, 0.462f, 1.000f, 1.000f) : new Color(1, 1, 1);

        AudioListener.pause = currentToggle.isOn;
        Options.Instance.Mute = currentToggle.isOn;
    }
}
