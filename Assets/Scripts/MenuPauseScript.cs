using UnityEngine;

public class MenuPauseScript : MonoBehaviour
{

    // Use this for initialization
    //void Start()
    //{

    //}

    public void GamePaused(bool paused)
    {
        Board board = Board.Instance();
        if (board != null)
        {
            //if (board.CellRatio == Options.Instance.CellRatio && board.Level == Options.Instance.SelectedLevel)
            {
                Debug.Log("Pause" + paused);
                board.gamePaused = paused;
            }
        }
    }

}
