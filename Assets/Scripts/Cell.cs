using UnityEngine;
using UnityEngine.UI;

public class Cell
{
    //internal Sprite FacingDownSprite;
    //internal Sprite EmptySprite;
    //internal Sprite FlagSprite;
    //internal Sprite ExplodedBombSprite;
    //internal Sprite[] BombNumberSprite;

    internal GameObject _cell;

    //internal int _index;
    internal int _row;
    internal int _col;
    internal int NbBomb;

    public enum CONTENT
    {
        EMPTY, // cell contains nothing, no bomb, no danger zone number
        DANGER_ZONE, // cell is adjacent to at least one bomb
        BOMB // cell contains a bomb
    }

    public enum STATE
    {
        COVERED, // cell content is hidden
        DISCOVERED, // cell content is discovered
        FLAGGED, // cell is flagged as bomb
    }

    internal bool Selected { get; set; }

    private STATE _state;
    internal STATE State
    {
        get { return _state; }
        set
        {
            _state = value;

            switch (value)
            {
                case STATE.COVERED:
                    _cell.GetComponent<Image>().sprite = Object.Instantiate(PrefabHelper.Instance.FacingDownSprite);
                    break;

                case STATE.DISCOVERED:
                    //_cell.GetComponent<Button>().interactable = false;
                    switch (_content)
                    {
                        case CONTENT.BOMB:
                            _cell.GetComponent<Image>().sprite = Object.Instantiate(PrefabHelper.Instance.ExplodedBombSprite);
                            break;
                        case CONTENT.DANGER_ZONE:
                            _cell.GetComponent<Image>().sprite = Object.Instantiate(PrefabHelper.Instance.BombNumberSprite[NbBomb]);
                            break;
                        case CONTENT.EMPTY:
                            _cell.GetComponent<Image>().sprite = Object.Instantiate(PrefabHelper.Instance.EmptySprite);
                            break;
                        default:
                            break;
                    }
                    break;

                case STATE.FLAGGED:
                    _cell.GetComponent<Image>().sprite = Object.Instantiate(PrefabHelper.Instance.FlagSprite);
                    break;

                default:
                    break;
            }
        }
    }

    private CONTENT _content;
    internal CONTENT Content
    {
        get { return _content; }
        set
        {
            _content = value;
        }
    }


    public Cell(GameObject prefabInstance, int width, int row, int col)
    {
        _cell = prefabInstance;
        //_index = (row * width) + col;

        _row = row;
        _col = col;
    }

}
