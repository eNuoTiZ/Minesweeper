using UnityEngine;
using UnityEngine.UI;

public class GeneralOptions : MonoBehaviour
{

    public Toggle FullScreenToggle;
    public Toggle ScreenTimeoutToggle;
    public Toggle VibrationsToggle;

    // Use this for initialization
    void Start()
    {
        FullScreenToggle.onValueChanged.AddListener((bool on) =>
        {
            FullScreenToggle.GetComponentInChildren<Image>().color = on ? new Color(0.000f, 0.462f, 1.000f, 1.000f) : new Color(1, 1, 1);
            SetFullScreen(on);
        });

        ScreenTimeoutToggle.onValueChanged.AddListener((bool on) =>
        {
            ScreenTimeoutToggle.GetComponentInChildren<Image>().color = on ? new Color(0.000f, 0.462f, 1.000f, 1.000f) : new Color(1, 1, 1);
            DisableScreenTimeout(on);
        });

        VibrationsToggle.onValueChanged.AddListener((bool on) =>
        {
            VibrationsToggle.GetComponentInChildren<Image>().color = on ? new Color(0.000f, 0.462f, 1.000f, 1.000f) : new Color(1, 1, 1);
            EnableVibrations(on);
        });
    }

    void SetFullScreen(bool fullScreenOn)
    {
        Options.Instance.FullScreen = fullScreenOn;
    }

    void DisableScreenTimeout(bool disableScreenTimeout)
    {
        if (disableScreenTimeout)
        {
            Options.Instance.ScrenTimeout = SleepTimeout.NeverSleep;
        }
        else
        {
            Options.Instance.ScrenTimeout = SleepTimeout.SystemSetting;
        }
    }

    void EnableVibrations(bool enableVibration)
    {
        Options.Instance.Vibrations = enableVibration;
    }
}

