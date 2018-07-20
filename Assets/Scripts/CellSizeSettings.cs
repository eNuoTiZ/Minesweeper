using UnityEngine;
using UnityEngine.UI;

public class CellSizeSettings : MonoBehaviour
{
    public GameObject PanelCellSize;
    public GameObject cellTemplate;
    public Slider CellSizeSlider;

    // Use this for initialization
    void Start()
    {
        CellSizeSlider.value = Options.Instance.CellRatio;// + (Options.Instance.CellRatio * 0.4f);
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(CellSizeSlider.value, CellSizeSlider.value, CellSizeSlider.value);
        CellSizeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        GameObject.FindGameObjectWithTag("CellSizeLabel").GetComponent<Text>().text = "Cell size (x" + CellSizeSlider.value.ToString("0.00") + ")";
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        //Debug.Log(CellSizeSlider.value);
        float ratiocCellOptions = CellSizeSlider.value;// + (CellSizeSlider.value * 0.4f);
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(CellSizeSlider.value, CellSizeSlider.value, CellSizeSlider.value);

        //Debug.Log("Template size cell: " + cellTemplate.GetComponent<RectTransform>().rect.width);

        GameObject.FindGameObjectWithTag("CellSizeLabel").GetComponent<Text>().text = "Tile size (x" + ratiocCellOptions.ToString("0.00") + ")";

        Options.Instance.CellRatio = ratiocCellOptions;
    }
}
