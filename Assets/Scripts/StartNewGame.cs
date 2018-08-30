using UnityEngine;
using UnityEngine.UI;

public class StartNewGame : MonoBehaviour {

    public Sprite HappySmiley;

    public void NewGame()
    {
        Board.Instance()._gameData = null;
        Board.Instance()._mono.StartCoroutine(Board.Instance().ResizeBoard(Board.Instance().CellRatio, true));
        //Board.Instance().ResizeBoard(Board.Instance().CellRatio, true);

        // Deprecated
        //Board.Instance().ResetBoard();

        GameObject.FindGameObjectWithTag("SmileyButton").GetComponent<Image>().sprite = HappySmiley;
    }
}
