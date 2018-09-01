using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Collections;

public class Board : ScriptableObject
{
    internal bool initializing = false;
    public int Width { get; set; }
    public int Height { get; set; }
    internal int BombCount { get; private set; }

    //internal GameObject CellPrefab;
    //public Sprite FacingDownSprite;
    //internal Sprite EmptySprite;
    //internal Sprite FlagSprite;
    //internal Sprite BombSprite;
    //internal Sprite ExplodedBombSprite;
    //internal Sprite BadBombGuessSprite;
    //internal GameObject ExplosionPrefab;
    //Sprite[] BombNumberSprite;

    internal Cell[,] Cells;
    internal int nbGoodFlags;
    internal int nbFlags;
    internal bool boardExploded;
    internal bool gameEnded;
    internal bool gameStarted;
    internal bool gamePaused;
    internal float startTime;
    internal static bool firstInit;

    private static Board _board;
    internal GameObject _boardPanel;
    internal MonoBehaviour _mono;

    internal GameData _gameData;

    internal float CellRatio { get; set; }

    internal Options.Level Level { get; set; }

    internal bool flagMode = false;

    Slider loadingSlider;

    public static Board Instance()
    {
        if (_board == null)
        {
            //throw new Exception("Object not created");
        }
        return _board;
    }

    public static Board Instance(GameObject boardPanel, GameData gameData, MonoBehaviour mono)
    {
        if (_board == null)
        {
            firstInit = true;
            _board = new Board(boardPanel, gameData, mono);
        }
        return _board;
    }

    private Board(GameObject boardPanel, GameData gameData, MonoBehaviour mono)
    {
        _boardPanel = boardPanel;
        _mono = mono;
        _gameData = gameData;
        CellRatio = Options.Instance.CellRatio;

        loadingSlider = GameObject.FindGameObjectWithTag("LoadingSlider").GetComponent<Slider>();

        //CellPrefab = Instantiate(prefabInstance);
        //FacingDownSprite = Instantiate(PrefabHelper.Instance.FacingDownSprite);
        //EmptySprite = Instantiate(PrefabHelper.Instance.EmptySprite);
        //FlagSprite = Instantiate(PrefabHelper.Instance.FlagSprite);
        //BombSprite = Instantiate(bombSprite);
        //ExplodedBombSprite = Instantiate(explodedBombSprite);
        //BadBombGuessSprite = Instantiate(badBombGuessSprite);
        //BombNumberSprite = bombNumberSprite;
        //ExplosionPrefab = explosionPrefab;

        //ResizeBoard(CellRatio);
        _mono.StartCoroutine(ResizeBoard(CellRatio));

        //ResizeBoard(CellRatio);

        //ResetBoard();
    }

    private void DestroyAllCells(bool destroyCell = false)
    {
        for (int row = 0; row < Height; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                Cells[row, col].Selected = false;
                Cells[row, col].State = Cell.STATE.COVERED;
                Cells[row, col].NbBomb = 0;
                Cells[row, col].Content = Cell.CONTENT.EMPTY;
                //Cells[row, col]._cell.GetComponent<Image>().raycastTarget = true;

                foreach (Transform child in Cells[row, col]._cell.transform)
                {
                    Destroy(child.gameObject);
                }

                if (destroyCell)
                {
                    Destroy(_board.Cells[row, col]._cell);
                }
            }
        }
    }

    public IEnumerator ResizeBoard(float newCellRatio, bool reset = true)
    {
        //Debug.Log("RESIZEBOARD!");
        loadingSlider.value = 0;

        initializing = true;

        int currentWidth = Width;
        int currentHeight = Height;

        bool cellRatioChanged = CellRatio != newCellRatio;
        bool levelChanged = Level != Options.Instance.SelectedLevel;

        CellRatio = newCellRatio;

        float startBoardPanelHeight = _boardPanel.GetComponent<RectTransform>().rect.height;
        float startBoardPanelWidth = _boardPanel.GetComponent<RectTransform>().rect.width;
        float cellHeight = PrefabHelper.Instance.CellPrefab.GetComponent<RectTransform>().rect.height * CellRatio;
        float cellWidth = PrefabHelper.Instance.CellPrefab.GetComponent<RectTransform>().rect.width * CellRatio;
        //Debug.Log("Height: " + startBoardPanelHeight + " - Width: " + startBoardPanelWidth);
        //Debug.Log("Cell dimension: " + cellHeight);

        float widthOffset = 0;

        System.Diagnostics.Stopwatch sw;

        if (reset)
        {
            nbGoodFlags = 0;
            nbFlags = 0;
            boardExploded = false;
            gameStarted = false;
            gameEnded = false;
            gamePaused = false;
            startTime = 0;

            sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            if (currentWidth != Width || currentHeight != Height || cellRatioChanged)
            {
                DestroyAllCells(true);
            }
            else
            {
                DestroyAllCells();
            }
            sw.Stop();
            //Debug.Log("DestroyAllCells() took " + sw.Elapsed);

            Width = (int)Mathf.Floor(startBoardPanelWidth / cellWidth);
            Height = (int)Mathf.Floor(startBoardPanelHeight / cellHeight);

            if (currentWidth != Width || currentHeight != Height || cellRatioChanged)
            {
                widthOffset = (startBoardPanelWidth - (Width * cellWidth)) / 2;

                //Debug.Log("Nb width: " + Width + " - Nb height " + Height);

                Cells = new Cell[Height, Width];
            }

            Level = Options.Instance.SelectedLevel;
            BombCount = GetBombNumber();
        }
        //else
        //{
        //    Cell[,] tempCellArray = RotateBoardClockwise(_board.Cells);
        //    int oldWidth = Width;
        //    Width = Height;
        //    Height = oldWidth;

        //    widthOffset = (startBoardPanelHeight - (Width * cellWidth)) / 2;

        //    //DestroyAllCells();

        //    _board.Cells = new Cell[Height, Width];
        //    Array.Copy(tempCellArray, _board.Cells, tempCellArray.Length);
        //}

        //Debug.Log("Offset: " + widthOffset);

        sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        int nbCells = Width * Height;
        loadingSlider.minValue = 0;
        loadingSlider.maxValue = nbCells;
        int loadingCounter = 1;

        for (int row = 0; row < Height; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                if (reset)
                {
                    //System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();
                    //sw2.Start();

                    if (currentWidth != Width || currentHeight != Height || cellRatioChanged)
                    {
                        GameObject newCell = Instantiate(PrefabHelper.Instance.CellPrefab);
                        newCell.transform.localScale = new Vector3(CellRatio, CellRatio, CellRatio);
                        newCell.transform.position = new Vector3((cellWidth * col) + widthOffset, -cellHeight * row, newCell.transform.position.z);
                        newCell.transform.SetParent(_boardPanel.transform, false);

                        Button button = newCell.GetComponent<Button>();

                        newCell.AddComponent(typeof(CellComponent));

                        var cell = newCell.GetComponent<CellComponent>();
                        cell.row = row;
                        cell.col = col;

                        Cells[row, col] = new Cell(newCell, Width, row, col);
                    }
                    
                    if (firstInit && _gameData != null && !cellRatioChanged)
                    {
                        InitializeCellFromSaveGame(Cells[row, col], row, col);
                    }
                    else
                    {
                        Cells[row, col].NbBomb = 0;
                        Cells[row, col].Content = Cell.CONTENT.EMPTY;
                        Cells[row, col].State = Cell.STATE.COVERED;
                        Cells[row, col].Selected = false;
                    }

                    //sw2.Stop();
                    //Debug.Log("Cell initialization took " + sw2.Elapsed);
                }
                else
                {
                    _board.Cells[row, col]._cell.transform.position = new Vector3((cellWidth * col) + widthOffset, -cellHeight * row, _board.Cells[row, col]._cell.transform.position.z);
                }

                loadingSlider.value = loadingCounter;
                loadingCounter++;
            }

            yield return null;
        }
        sw.Stop();
        //Debug.Log("Init board " + sw.Elapsed);

        if (reset)
        {
            if (_gameData == null || cellRatioChanged || levelChanged)
            {
                sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                _gameData = new GameData(Width, Height);
                InitializeBoardBombs();

                sw.Stop();
                //Debug.Log("InitializeBoardBombs() took " + sw.Elapsed);
            }

            GameObject.FindGameObjectWithTag("LoadingPanel").GetComponent<Animator>().Play("LoadingPanelClose");
            GameObject.FindGameObjectWithTag("BackgroundBlackPanel").GetComponent<Animator>().Play("BackgroundBlackPanelDisable");
        }

        initializing = false;
        firstInit = false;

        if (gamePaused)
        {
            gamePaused = false;
        }
    }

    public void ResetBoard()
    {
        nbGoodFlags = 0;
        nbFlags = 0;
        boardExploded = false;
        gameStarted = false;
        gameEnded = false;
        gamePaused = false;
        startTime = 0;

        for (int row = 0; row < Height; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                Cells[row, col].Selected = false;
                Cells[row, col].State = Cell.STATE.COVERED;
                Cells[row, col].NbBomb = 0;
                Cells[row, col].Content = Cell.CONTENT.EMPTY;
                //Cells[row, col]._cell.GetComponent<Image>().raycastTarget = true;

                foreach (Transform child in Cells[row, col]._cell.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        int bombCountLeft = BombCount;
        while (bombCountLeft > 0)
        {
            int rndRow = Mathf.RoundToInt(UnityEngine.Random.Range(0, Height));
            int rndCol = Mathf.RoundToInt(UnityEngine.Random.Range(0, Width));

            if (Cells[rndRow, rndCol].Content == Cell.CONTENT.BOMB)
            {
                continue;
            }

            //Debug.Log("Random row: " + rndRow + " - Random col: " + rndCol);
            Cells[rndRow, rndCol].Content = Cell.CONTENT.BOMB;

            int rowMin = (rndRow - 1 < 0) ? rndRow : rndRow - 1;
            int rowMax = (rndRow + 1 >= Height) ? rndRow : rndRow + 1;
            int colMin = (rndCol - 1 < 0) ? rndCol : rndCol - 1;
            int colMax = (rndCol + 1 >= Width) ? rndCol : rndCol + 1;

            for (int row = rowMin; row <= rowMax; row++)
            {
                for (int col = colMin; col <= colMax; col++)
                {
                    if (Cells[row, col].Content == Cell.CONTENT.BOMB)
                    {
                        continue;
                    }

                    //Debug.Log("Row: " + row + " - Col: " + col);
                    Cells[row, col].NbBomb++;
                    Cells[row, col].Content = Cell.CONTENT.DANGER_ZONE;
                }
            }

            bombCountLeft--;
        }

        GameObject.FindGameObjectWithTag("NewBoardPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void InitializeCellFromSaveGame(Cell cell, int row, int col)
    {
        if (_gameData.bombNumberCells[row, col] == -1)
        {
            cell.Content = Cell.CONTENT.BOMB;
        }
        else
        {
            cell.NbBomb = _gameData.bombNumberCells[row, col];

            if (_gameData.bombNumberCells[row, col] == 0)
            {
                cell.Content = Cell.CONTENT.EMPTY;
            }
            else
            {
                cell.Content = Cell.CONTENT.DANGER_ZONE;
            }
        }

        switch (_gameData.stateCells[row, col])
        {
            case 1:
                cell.State = Cell.STATE.COVERED;
                break;
            case 2:
                cell.State = Cell.STATE.DISCOVERED;
                //Debug.Log(string.Format("Cell {0}-{1} discovered!", row, col));
                break;
            case 3:
                cell.State = Cell.STATE.FLAGGED;
                if (cell.Content == Cell.CONTENT.BOMB)
                {
                    nbGoodFlags++;
                }
                nbFlags++;
                break;
            default:
                break;
        }

        cell.Selected = false;
    }

    public void InitializeBoardBombs()
    {
        int bombCountLeft = BombCount;
        while (bombCountLeft > 0)
        {
            int rndRow = Mathf.RoundToInt(UnityEngine.Random.Range(0, Height));
            int rndCol = Mathf.RoundToInt(UnityEngine.Random.Range(0, Width));

            if (Cells[rndRow, rndCol].Content == Cell.CONTENT.BOMB)
            {
                continue;
            }

            //Debug.Log("Random row: " + rndRow + " - Random col: " + rndCol);
            Cells[rndRow, rndCol].Content = Cell.CONTENT.BOMB;

            int rowMin = (rndRow - 1 < 0) ? rndRow : rndRow - 1;
            int rowMax = (rndRow + 1 >= Height) ? rndRow : rndRow + 1;
            int colMin = (rndCol - 1 < 0) ? rndCol : rndCol - 1;
            int colMax = (rndCol + 1 >= Width) ? rndCol : rndCol + 1;

            for (int row = rowMin; row <= rowMax; row++)
            {
                for (int col = colMin; col <= colMax; col++)
                {
                    if (Cells[row, col].Content == Cell.CONTENT.BOMB)
                    {
                        _gameData.bombNumberCells[row, col] = -1;
                        continue;
                    }

                    //Debug.Log("Row: " + row + " - Col: " + col);
                    Cells[row, col].NbBomb++;
                    Cells[row, col].Content = Cell.CONTENT.DANGER_ZONE;

                    _gameData.bombNumberCells[row, col] = Cells[row, col].NbBomb;
                }
            }

            bombCountLeft--;
        }

        GameObject.FindGameObjectWithTag("NewBoardPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public List<KeyValuePair<int, int>> SelectNeighbors(int row, int col)
    {
        List<KeyValuePair<int, int>> listOfIndexes = new List<KeyValuePair<int, int>>();

        // Add at least the current empty cell
        listOfIndexes.Add(new KeyValuePair<int, int>(row, col));

        int rowMin = (row - 1 < 0) ? row : row - 1;
        int rowMax = (row + 1 >= Height) ? row : row + 1;
        int colMin = (col - 1 < 0) ? col : col - 1;
        int colMax = (col + 1 >= Width) ? col : col + 1;

        for (int i = rowMin; i <= rowMax; i++)
        {
            for (int j = colMin; j <= colMax; j++)
            {
                if (!Cells[i, j].Selected && Cells[i, j].Content == Cell.CONTENT.EMPTY)
                {
                    // Add tuple i,j to the returned array
                    Cells[i, j].Selected = true;
                    listOfIndexes.Add(new KeyValuePair<int, int>(i, j));
                    listOfIndexes.AddRange(SelectNeighbors(i, j));
                }
            }
        }

        return listOfIndexes;
    }

    public IEnumerator RevealAllBombs(GameObject clickedCell)
    {
        for (int row = 0; row < Height; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                if (_board.Cells[row, col]._cell != clickedCell)
                {
                    if (_board.Cells[row, col].Content == Cell.CONTENT.BOMB)
                    {
                        if (_board.Cells[row, col].State != Cell.STATE.FLAGGED)
                        {
                            GameObject explosion = Instantiate(PrefabHelper.Instance.ExplosionPrefab, _board.Cells[row, col]._cell.transform, false);
                            explosion.transform.localPosition = Vector3.zero;
                            explosion.GetComponent<RectTransform>().anchoredPosition = new Vector2(.5f, .5f);
                            explosion.GetComponent<RectTransform>().anchorMax = new Vector2(.5f, .5f);
                            explosion.GetComponent<RectTransform>().anchorMin = new Vector2(.5f, .5f);

                            _board.Cells[row, col]._cell.GetComponent<Image>().sprite = Instantiate(PrefabHelper.Instance.BombSprite);
                        }
                    }
                    else
                    {
                        if (_board.Cells[row, col].State == Cell.STATE.FLAGGED)
                        {
                            _board.Cells[row, col]._cell.GetComponent<Image>().sprite = Instantiate(PrefabHelper.Instance.BadBombGuessSprite);
                        }
                    }
                }
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    public bool IsBoardComplete()
    {
        bool isComplete = _board.nbGoodFlags == _board.BombCount;
        if (isComplete)
        {
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    if (_board.Cells[row, col].State == Cell.STATE.COVERED && _board.Cells[row, col].Content != Cell.CONTENT.BOMB)
                    {
                        isComplete = false;
                        break;
                    }
                }
                if (!isComplete)
                {
                    break;
                }
            }
        }

        return isComplete;
    }

    private int GetBombNumber()
    {
        switch (Level)
        {
            case Options.Level.Beginner:
                return Mathf.FloorToInt((float)(Height * Width * 0.10));
            case Options.Level.Intermadiate:
                return Mathf.FloorToInt((float)(Height * Width * 0.15));
            case Options.Level.Expert:
                return Mathf.FloorToInt((float)(Height * Width * 0.22));
            case Options.Level.Custom:
                break;
            default:
                return 0;
        }

        return 0;
    }

    public Cell[,] RotateBoardClockwise(Cell[,] srcArray)
    {
        int width;
        int height;
        Cell[,] dst;

        height = srcArray.GetUpperBound(0) + 1;
        width = srcArray.GetUpperBound(1) + 1;
        dst = new Cell[width, height];

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                int newRow;
                int newCol;

                newRow = col;
                newCol = height - (row + 1);

                //dst[newCol, newRow] = srcArray[col, row];
                dst[newRow, newCol] = srcArray[row, col];
                dst[newRow, newCol]._row = newRow;
                dst[newRow, newCol]._col = newCol;
            }
        }

        return dst;
    }

    public Cell[,] RotateBoardCounterClockwise(Cell[,] srcArray)
    {
        int width;
        int height;
        Cell[,] dst;

        width = srcArray.GetUpperBound(0) + 1;
        height = srcArray.GetUpperBound(1) + 1;
        dst = new Cell[height, width];

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                int newRow;
                int newCol;

                newRow = width - (col + 1);
                newCol = row;

                dst[newCol, newRow] = srcArray[col, row];
            }
        }

        return dst;
    }

    //[MenuItem("Tools/MyTool/Do It in C#")]
    //static void DoIt()
    //{
    //    EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
    //}


}