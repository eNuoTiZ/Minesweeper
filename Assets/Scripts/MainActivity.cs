using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainActivity : MonoBehaviour
{
    public GameObject CellPrefab;
    public GameObject ExplosionPrefab;
    public Sprite FacingDownSprite;
    public Sprite EmptySprite;
    public Sprite FlagSprite;
    public Sprite BombSprite;
    public Sprite ExplodedBombSprite;
    public Sprite BadBombGuessSprite;

    public Sprite[] BombNumberSprite;

    public GameObject BoardPanel;
    public GameObject SmileyPanel;
    public GameObject BackgroundBlackPanel;
    public GameObject LoadingPanel;

    public Sprite HappySmiley;
    public Sprite SadSmiley;
    public Sprite SunGlassesSmiley;
    public Sprite OhSmiley;

    public Text RemainingBombText;
    public Text TimerText;

    public List<AudioSource> MenuSoundsAudioSourcesList;
    public List<AudioSource> SoundEffectsAudioSourcesList;
    public AudioSource BackgroundGameSound;

    internal Board _board;
    private GameData _gameData;

    private ScreenOrientation _lastScreenOrientation;
    private int _lastScreenWidth;
    private int _lastScreenHeight;
    
    private static MainActivity _Instance;

    public static MainActivity Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new GameObject("MainActivity").AddComponent<MainActivity>();
            }

            return _Instance;
        }
    }

    //Options _options;

    // Use this for initialization
    void Start()
    {
        if (_board == null)
        {
            LoadingPanel.GetComponent<Animator>().Play("LoadingPanelOpen");
            BackgroundBlackPanel.GetComponent<Animator>().Play("BackgroundBlackPanelActivate");

            _board = Board.Instance(BoardPanel, _gameData, this);
        }
        

    }

    private void Awake()
    {
        LoadOptions();

        _lastScreenOrientation = Screen.orientation;
        _lastScreenHeight = Screen.height;
        _lastScreenWidth = Screen.width;

        PrefabHelper.Instance.CellPrefab = CellPrefab;
        PrefabHelper.Instance.FacingDownSprite = FacingDownSprite;
        PrefabHelper.Instance.EmptySprite = EmptySprite;
        PrefabHelper.Instance.FlagSprite = FlagSprite;
        PrefabHelper.Instance.BombSprite = BombSprite;
        PrefabHelper.Instance.ExplodedBombSprite = ExplodedBombSprite;
        PrefabHelper.Instance.BadBombGuessSprite = BadBombGuessSprite;
        PrefabHelper.Instance.ExplosionPrefab = ExplosionPrefab;
        PrefabHelper.Instance.BombNumberSprite = BombNumberSprite;
        PrefabHelper.Instance.SoundEffectsAudioSourcesList = SoundEffectsAudioSourcesList;
        PrefabHelper.Instance.MenuSoundsAudioSourcesList = MenuSoundsAudioSourcesList;
        PrefabHelper.Instance.BackgroundGameSound = BackgroundGameSound;

        PrefabHelper.Instance.HappySmiley = HappySmiley;
        PrefabHelper.Instance.OhSmiley = OhSmiley;
        PrefabHelper.Instance.SadSmiley = SadSmiley;
        PrefabHelper.Instance.SunGlassesSmiley = SunGlassesSmiley;


        Screen.fullScreen = true;
        Canvas.ForceUpdateCanvases();
    }

    //public void RotateBoard()
    //{
    //    _board.ResizeBoard(Options.Instance.CellRatio, false);
    //}

    float pauseTimer = 0;

    //private void OnRectTransformDimensionsChange()
    //{
    //    UnityEngine.Debug.Log("CHANGED!");

    //    if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
    //    {
    //        UnityEngine.Debug.Log("we landscape now.");
    //    }
    //    else if (Input.deviceOrientation == DeviceOrientation.Portrait)
    //    {
    //        UnityEngine.Debug.Log("we portrait now");
    //    }
    //}

    //private void OnLevelWasLoaded(int level)
    //{
    //    if (level == 0)
    //    {
    //        _board = Board.Instance(BoardPanel);
    //    }
    //}

    bool gameWasPaused = false;
    bool needBoardUpdate = false;
    // Update is called once per frame
    void Update()
    {
        if (_lastScreenWidth != Screen.width)
        {
            UnityEngine.Debug.Log("Changed!!!");
            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
        }

        if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            UnityEngine.Debug.Log("landscape now.");
        }
        else if (Input.deviceOrientation == DeviceOrientation.Portrait)
        {
            UnityEngine.Debug.Log("portrait now");
        }

        if (_board != null)
        {
            if (_board.initializing)
            {
                return;
            }

            if (_board.gamePaused || _board.gameEnded)
            {
                gameWasPaused = _board.gamePaused;

                if (_board.gamePaused && _board.gameStarted && !_board.gameEnded)
                {
                    pauseTimer += Time.deltaTime;
                    //UnityEngine.Debug.Log("Pause time: " + pauseTimer);
                }

                return;
            }

            if (needBoardUpdate)
            {
                needBoardUpdate = false;
                _board.initializing = true;
                
                BackgroundBlackPanel.GetComponent<Animator>().Play("BackgroundBlackPanelActivate");
                LoadingPanel.GetComponent<Animator>().Play("LoadingPanelOpen");

                UnityEngine.Debug.Log("Updating the board!");
                StartCoroutine(Board.Instance().ResizeBoard(Options.Instance.CellRatio));

                return;
            }

            if (gameWasPaused)
            {
                gameWasPaused = false;
                pauseTimer = 0;

                if (_board.CellRatio != Options.Instance.CellRatio || _board.Level != Options.Instance.SelectedLevel)
                {
                    needBoardUpdate = true;
                    UnityEngine.Debug.Log("needBoardUpdate!");
                    return;
                }
            }

            if (_board.IsBoardComplete())
            {
                GameObject.FindGameObjectWithTag("NewBoardPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

                UnityEngine.Debug.Log("You win!");
                SmileyPanel.GetComponent<Image>().sprite = SunGlassesSmiley;

                _board.gameEnded = true;

                Options.Instance.AddNewRankItem(float.Parse(TimerText.text));
                //EditorUtility.DisplayDialog("Minesweeper", "You win!", "OK", "");
            }

            if (_board.boardExploded)
            {
                // Game Over!
                SmileyPanel.GetComponent<Image>().sprite = SadSmiley;

                GameObject.FindGameObjectWithTag("NewBoardPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;
                
                _board.gameEnded = true;
                return;
            }

            if (_board.gameStarted)
            {
                if (_board.startTime == 0)
                {
                    _board.startTime = Time.time;
                }
                else
                {
                    if (!_board.boardExploded)
                    {
                        if (pauseTimer != 0)
                        {
                            _board.startTime = _board.startTime + pauseTimer;
                            pauseTimer = 0;
                        }
                    }
                }

                float t = Time.time - _board.startTime;
                TimerText.text = t.ToString("000");
            }
            else
            {
                TimerText.text = "000";
            }

            RemainingBombText.text = (_board.BombCount - _board.nbFlags).ToString("000");
        }
    }

    public void SaveOptions()
    {
        string jsonSaveData = JsonUtility.ToJson(Options.Instance);
        //UnityEngine.Debug.Log("Save: " + jsonSaveData);

        PlayerPrefs.SetString("Options", jsonSaveData);

        // Save the game state if not finished
        if (!_board.boardExploded)
        {
            FileStream file;

            string saveFilePath = Application.persistentDataPath + "/minesweeper.dat";

            if (File.Exists(saveFilePath)) file = File.OpenWrite(saveFilePath);
            else file = File.Create(saveFilePath);

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, _board._gameData);
            file.Close();
        }
    }

    public void LoadOptions()
    {
        try
        {
            FileStream file;
            string saveFilePath = Application.persistentDataPath + "/minesweeper.dat";
            if (File.Exists(saveFilePath))
            {
                file = File.OpenRead(saveFilePath);

                BinaryFormatter bf = new BinaryFormatter();
                _gameData = (GameData)bf.Deserialize(file);
                file.Close();
            }
            else
            {
                UnityEngine.Debug.LogError("File not found");
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError(e.ToString());
        }

        string jsonSaveData = PlayerPrefs.GetString("Options");
        //UnityEngine.Debug.Log("Load: " + jsonSaveData);

        Options.Instance = JsonUtility.FromJson<Options>(jsonSaveData);

        GameObject.FindGameObjectWithTag("PlayerNameText").GetComponent<InputField>().text = Options.Instance.PlayerName;

        // Sound options
        AudioListener.pause = Options.Instance.Mute;
        Toggle muteToggle = GameObject.FindGameObjectWithTag("MuteToggle").GetComponent<Toggle>();
        muteToggle.isOn = Options.Instance.Mute;
        muteToggle.GetComponentInChildren<Image>().color = muteToggle.isOn ? new Color(0.000f, 0.462f, 1.000f, 1.000f) : new Color(1, 1, 1);

        Slider musicVolumeSlider = GameObject.FindGameObjectWithTag("MusicVolumeSlider").GetComponent<Slider>();
        musicVolumeSlider.value = Options.Instance.MusicVolume;
        BackgroundGameSound.volume = Options.Instance.MusicVolume;

        Slider menuSoundSlider = GameObject.FindGameObjectWithTag("MenuSoundSlider").GetComponent<Slider>();
        menuSoundSlider.value = Options.Instance.MenuSoundsVolume;
        foreach (AudioSource source in MenuSoundsAudioSourcesList)
        {
            source.volume = Options.Instance.MenuSoundsVolume;
        }

        Slider soundEffectsVolumeSlider = GameObject.FindGameObjectWithTag("SoundEffectsVolumeSlider").GetComponent<Slider>();
        soundEffectsVolumeSlider.value = Options.Instance.SoundEffectsVolume;
        foreach (AudioSource source in SoundEffectsAudioSourcesList)
        {
            source.volume = Options.Instance.SoundEffectsVolume;
        }

        // General Options
        Toggle fullScreenToggle = GameObject.FindGameObjectWithTag("FullScreenToggle").GetComponent<Toggle>();
        fullScreenToggle.isOn = Options.Instance.FullScreen;
        fullScreenToggle.GetComponentInChildren<Image>().color = fullScreenToggle.isOn ? new Color(0.000f, 0.462f, 1.000f, 1.000f) : new Color(1, 1, 1);

        Toggle screenTimeoutToggle = GameObject.FindGameObjectWithTag("ScreenTimeoutToggle").GetComponent<Toggle>();
        screenTimeoutToggle.isOn = (Options.Instance.ScrenTimeout == SleepTimeout.NeverSleep) ? true : false;
        screenTimeoutToggle.GetComponentInChildren<Image>().color = screenTimeoutToggle.isOn ? new Color(0.000f, 0.462f, 1.000f, 1.000f) : new Color(1, 1, 1);

        Toggle vibrationsToggle = GameObject.FindGameObjectWithTag("VibrationsToggle").GetComponent<Toggle>();
        vibrationsToggle.isOn = Options.Instance.Vibrations;
        vibrationsToggle.GetComponentInChildren<Image>().color = vibrationsToggle.isOn ? new Color(0.000f, 0.462f, 1.000f, 1.000f) : new Color(1, 1, 1);

        // Tiles options
        GameObject.FindGameObjectWithTag("CellSizeSlider").GetComponent<Slider>().value = Options.Instance.CellRatio;

        // Level options
        switch (Options.Instance.SelectedLevel)
        {
            case Options.Level.Beginner:
                Toggle beginnerToggle = GameObject.FindGameObjectWithTag("BeginnerToggle").GetComponent<Toggle>();
                if (beginnerToggle != null)
                {
                    beginnerToggle.isOn = true;
                }
                //else
                //{
                //    UnityEngine.Debug.Log("Toggle null!");
                //}

                break;
            case Options.Level.Intermadiate:
                Toggle intermediateToggle = GameObject.FindGameObjectWithTag("IntermediateToggle").GetComponent<Toggle>();
                if (intermediateToggle != null)
                {
                    intermediateToggle.isOn = true;
                }
                //else
                //{
                //    UnityEngine.Debug.Log("Toggle null!");
                //}

                break;
            case Options.Level.Expert:
                Toggle expertToggle = GameObject.FindGameObjectWithTag("ExpertToggle").GetComponent<Toggle>();
                if (expertToggle != null)
                {
                    expertToggle.isOn = true;
                }
                //else
                //{
                //    UnityEngine.Debug.Log("Toggle null!");
                //}
                break;
            case Options.Level.Custom:
                break;
            default:
                break;
        }

        //Options.Instance.BeginnerRankings.Clear();

    }

    private void OnApplicationQuit()
    {
        SaveOptions();
    }

    
}
