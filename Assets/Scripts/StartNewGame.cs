using UnityEngine;
using UnityEngine.UI;

public class StartNewGame : MonoBehaviour {

    public Sprite HappySmiley;

    public void NewGame()
    {
        //Board.Mono.StartCoroutine(Board.Instance().ResizeBoard(Board.Instance().CellRatio, true));
        Board.Instance().ResizeBoard(Board.Instance().CellRatio, true);

        // Deprecated
        //Board.Instance().ResetBoard();

        GameObject.FindGameObjectWithTag("SmileyButton").GetComponent<Image>().sprite = HappySmiley;
    }
}
