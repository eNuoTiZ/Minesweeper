using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellComponent : MonoBehaviour
{
    //public GameObject ExplosionPrefab;
    public int row;
    public int col;

    public void OnClick(bool longPress)
    {
        Board _board = Board.Instance();
        if (!_board.gameStarted)
        {
            _board.gameStarted = true;
        }

        if (longPress || _board.flagMode) // flag the cell or unflag it
        {
            Debug.Log("Long Press! " + "x: " + row + " - y: " + col);

            if (_board.Cells[row, col].State == Cell.STATE.DISCOVERED)
            {
                return;
            }

            FlagCell(row, col);
        }
        else
        {
            Debug.Log("Click! " + "x: " + row + " - y: " + col);

            if (_board.Cells[row, col].State == Cell.STATE.FLAGGED)
            {
                return;
            }

            if (_board.Cells[row, col].Content == Cell.CONTENT.EMPTY && _board.Cells[row, col].State != Cell.STATE.DISCOVERED)
            {
                List<KeyValuePair<int, int>> listNeighbors = _board.SelectNeighbors(row, col);
                //StartCoroutine(RevealEmptyCells(listNeighbors, _board.EmptySprite));
                RevealEmptyCellsNormal(listNeighbors, PrefabHelper.Instance.EmptySprite);
            }
            else
            {
                DiscoverCell(row, col);
            }

        }

        //Debug.Log("Height: " + Board.Instance().Cells[x, y]._cell.GetComponent<RectTransform>().rect.height);
    }

    public IEnumerator RevealEmptyCells(List<KeyValuePair<int, int>> litsOfIndexes, Sprite emptySprite)
    {
        Board _board = Board.Instance();
        
        foreach (KeyValuePair<int, int> item in litsOfIndexes)
        {
            _board.Cells[item.Key, item.Value].State = Cell.STATE.DISCOVERED;

            yield return null;
            //yield return new WaitForSeconds(.01f);
        }
    }

    public void RevealEmptyCellsNormal(List<KeyValuePair<int, int>> litsOfIndexes, Sprite emptySprite)
    {
        Board _board = Board.Instance();

        foreach (KeyValuePair<int, int> item in litsOfIndexes)
        {
            if (_board.Cells[item.Key, item.Value].State != Cell.STATE.FLAGGED)
            {
                _board.Cells[item.Key, item.Value].State = Cell.STATE.DISCOVERED;
            }
        }
    }

    public void FlagCell(int row, int col)
    {
        Board _board = Board.Instance();

        if (_board.Cells[row, col].State == Cell.STATE.FLAGGED)
        {
            if (_board.Cells[row, col].Content == Cell.CONTENT.BOMB)
            {
                _board.nbGoodFlags--;
            }
            _board.nbFlags--;
            _board.Cells[row, col].State = Cell.STATE.COVERED;
        }
        else
        {
            if (_board.nbFlags == _board.BombCount) // cannot flag more cells than we have bombs
            {
                return;
            }

            if (_board.Cells[row, col].Content == Cell.CONTENT.BOMB)
            {
                _board.nbGoodFlags++;
            }
            _board.nbFlags++;
            _board.Cells[row, col].State = Cell.STATE.FLAGGED;
        }
    }

    public void DiscoverCell(int row, int col)
    {
        Board _board = Board.Instance();
        _board.Cells[row, col].State = Cell.STATE.DISCOVERED;

        if (_board.Cells[row, col].Content == Cell.CONTENT.BOMB)
        {
            AudioSource explosionAudioSource = PrefabHelper.Instance.ExplosionPrefab.GetComponent<AudioSource>();
            explosionAudioSource.volume = Options.Instance.SoundEffectsVolume;

            GameObject explosion = Instantiate(PrefabHelper.Instance.ExplosionPrefab, _board.Cells[row, col]._cell.transform, false);
            explosion.transform.localPosition = Vector3.zero;
            explosion.GetComponent<RectTransform>().anchoredPosition = new Vector2(.5f, .5f);
            explosion.GetComponent<RectTransform>().anchorMax = new Vector2(.5f, .5f);
            explosion.GetComponent<RectTransform>().anchorMin = new Vector2(.5f, .5f);
            
            _board.boardExploded = true;
            StartCoroutine(_board.RevealAllBombs(_board.Cells[row, col]._cell));
        }
    }

}
