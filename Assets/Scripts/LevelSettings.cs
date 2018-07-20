using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSettings : MonoBehaviour
{
    //public ToggleGroup toggleGroup;
    public List<Toggle> toggleList;

    // Use this for initialization
    void Start()
    {
        foreach (Toggle currentToggle in toggleList)
        {
            currentToggle.onValueChanged.AddListener((bool on) =>
            {
                //if (on)
                {
                    SetLevel(currentToggle);
                }
            }
            );
        }
    }

    void SetLevel(Toggle toggle)
    {
        toggle.GetComponentInChildren<Image>().color = toggle.isOn ? new Color(0.000f, 0.462f, 1.000f, 1.000f) : new Color(1, 1, 1);

        if (toggle.isOn)
        {
            string toggleName = toggle.GetComponentInChildren<Text>().text;

            switch (toggleName)
            {
                case "Beginner":
                    Options.Instance.SelectedLevel = Options.Level.Beginner;
                    break;
                case "Intermediate":
                    Options.Instance.SelectedLevel = Options.Level.Intermadiate;
                    break;
                case "Expert":
                    Options.Instance.SelectedLevel = Options.Level.Expert;
                    break;
                default:
                    break;
            }
        }

    }

}
