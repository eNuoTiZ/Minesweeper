using UnityEngine;
using UnityEngine.UI;

public class FlagToggleSwitch : MonoBehaviour
{
    public Animator test;

    // Use this for initialization
    void Start()
    {
        GameObject.FindGameObjectWithTag("FlagToggle").GetComponent<Toggle>().onValueChanged.AddListener(delegate { SetFlagMode(); });
    }

    public void SwitchFlagMode()
    {
        GameObject.FindGameObjectWithTag("FlagToggle").GetComponent<Toggle>().isOn = !GameObject.FindGameObjectWithTag("FlagToggle").GetComponent<Toggle>().isOn;
    }

    public void SetFlagMode()
    {
        Toggle flagToggle = GameObject.FindGameObjectWithTag("FlagToggle").GetComponent<Toggle>();
        Board.Instance().flagMode = flagToggle.isOn;
        
        flagToggle.GetComponentInChildren<Image>().color = flagToggle.isOn ? new Color(0.000f, 0.462f, 1.000f, 1.000f) : new Color(1, 1, 1);

        if (flagToggle.isOn)
        {
            test.Play("LoadingPanelClose");
        }
        else
        {
            test.Play("LoadingPanelOpen");
        }
    }
}
