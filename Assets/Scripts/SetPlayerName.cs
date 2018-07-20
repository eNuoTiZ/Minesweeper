using UnityEngine;
using UnityEngine.UI;

public class SetPlayerName : MonoBehaviour {

    public Text PlayerNameText;

    public void SavePlayerName()
    {
        Options.Instance.PlayerName = PlayerNameText.text;
    }
}
