using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Options
{
    private const int MaxRankingsNumber = 10;

    public enum Level
    {
        Beginner = 1,
        Intermadiate = 2,
        Expert = 3,
        Custom = 4
    }

    [SerializeField]
    private float _cellRatio;

    public float CellRatio
    {
        get { return _cellRatio; }
        set { _cellRatio = value; }
    }

    [SerializeField]
    public float MusicVolume;
    [SerializeField]
    public float MenuSoundsVolume;

    [SerializeField]
    private float _soundEffectsVolume;
    public float SoundEffectsVolume
    {
        get { return _soundEffectsVolume; }
        set
        {
            _soundEffectsVolume = value;
            SetSoundEffectsVolumeForAllCell();
        }
    }

    [SerializeField]
    public bool Mute;

    [SerializeField]
    private bool _fullScreen;
    public bool FullScreen
    {
        get { return _fullScreen; }
        set
        {
            _fullScreen = value;
            Screen.fullScreen = value;
        }
    }

    [SerializeField]
    public bool Vibrations;

    [SerializeField]
    private int _screenTimeout;
    public int ScrenTimeout
    {
        get { return _screenTimeout; }
        set
        {
            _screenTimeout = value;
            Screen.sleepTimeout = value;
        }
    }

    [SerializeField]
    private Level _selectedLevel;

    public Level SelectedLevel
    {
        get { return _selectedLevel; }
        set
        {
            _selectedLevel = value;
        }
    }

    [SerializeField]
    public struct KeyValuePair<K, V>
    {
        public K Key { get; set; }
        public V Value { get; set; }
    }

    [SerializeField]
    public DictionaryRankings BeginnerRankings;

    [SerializeField]
    public DictionaryRankings IntermediateRankings;

    [SerializeField]
    public DictionaryRankings ExpertRankings;

    [SerializeField]
    private string _playerName;
    public string PlayerName
    {
        get { return _playerName; }
        set
        {
            _playerName = value;
            GameObject.FindGameObjectWithTag("PlayerNameText").GetComponent<InputField>().text = _playerName;
        }
    }

    //private static Options instance;
    private static Options instance;
    private static object _lock = new Object();

    private Options()
    {
        PlayerName = "Player1";
        _selectedLevel = Level.Beginner;
        CellRatio = 1.84f;
        MusicVolume = 1.0f;
        MenuSoundsVolume = 1.0f;
        SoundEffectsVolume = 1.0f;
        Mute = false;

        FullScreen = true;
        ScrenTimeout = SleepTimeout.SystemSetting;
        Vibrations = true;

        BeginnerRankings = new DictionaryRankings();
        IntermediateRankings = new DictionaryRankings();
        ExpertRankings = new DictionaryRankings();
    }

    public static Options Instance
    {
        get
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new Options();
                }
            }

            return instance;
        }

        set
        {
            instance = value;
        }
    }

    void SetSoundEffectsVolumeForAllCell()
    {
        Board _board = Board.Instance();

        if (_board == null)
        {
            return;
        }

        for (int row = 0; row < _board.Height; row++)
        {
            for (int col = 0; col < _board.Width; col++)
            {
                if (_board.Cells[row, col]._cell != null)
                {
                    AudioSource clickAudioSource = _board.Cells[row, col]._cell.GetComponent<AudioSource>();
                    clickAudioSource.volume = _soundEffectsVolume;
                }
            }
        }
    }

    public void AddNewRankItem(float completedTime)
    {
        switch (SelectedLevel)
        {
            case Level.Beginner:
                Tuple beginnerTuple = new Tuple
                {
                    BoardSize = Board.Instance().Width + "x" + Board.Instance().Height,
                    item1 = PlayerName,
                    item2 = completedTime
                };

                if (BeginnerRankings.Find(x => x.item1 == PlayerName && x.item2 == completedTime && x.BoardSize == beginnerTuple.BoardSize) == null)
                {
                    BeginnerRankings.Add(beginnerTuple);
                }

                BeginnerRankings.Sort(SortByScore);
                if (BeginnerRankings.Count > MaxRankingsNumber)
                {
                    BeginnerRankings.RemoveRange(0, MaxRankingsNumber);
                }

                break;
            case Level.Intermadiate:
                Tuple intermediateTuple = new Tuple
                {
                    BoardSize = Board.Instance().Width + "x" + Board.Instance().Height,
                    item1 = PlayerName,
                    item2 = completedTime
                };
                if (IntermediateRankings.Find(x => x.item1 == PlayerName && x.item2 == completedTime && x.BoardSize == intermediateTuple.BoardSize) == null)
                {
                    IntermediateRankings.Add(intermediateTuple);
                }

                IntermediateRankings.Sort(SortByScore);
                if (IntermediateRankings.Count > MaxRankingsNumber)
                {
                    IntermediateRankings.RemoveRange(0, MaxRankingsNumber);
                }

                break;
            case Level.Expert:
                Tuple expertTuple = new Tuple
                {
                    BoardSize = Board.Instance().Width + "x" + Board.Instance().Height,
                    item1 = PlayerName,
                    item2 = completedTime
                };
                if (ExpertRankings.Find(x => x.item1 == PlayerName && x.item2 == completedTime && x.BoardSize == expertTuple.BoardSize) == null)
                {
                    ExpertRankings.Add(expertTuple);
                }

                ExpertRankings.Sort(SortByScore);
                if (ExpertRankings.Count > MaxRankingsNumber)
                {
                    ExpertRankings.RemoveRange(0, MaxRankingsNumber);
                }

                break;
            case Level.Custom:
                break;
            default:
                break;
        }
    }

    static int SortByScore(Tuple f1, Tuple f2)
    {
        return f1.item2.CompareTo(f2.item2);
    }
}

[System.Serializable]
public class Tuple : GenericTuple<string, float>
{
    [SerializeField]
    public string BoardSize;
}

[System.Serializable] public class DictionaryRankings : SerializableList<Tuple> { }

[System.Serializable]
public class SerializableList<TValue> : List<TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TValue> listOfEntries = new List<TValue>();

    public void OnBeforeSerialize()
    {
        listOfEntries.Clear();

        foreach (var pair in this)
        {
            listOfEntries.Add(pair);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        //if (keys.Count != values.Count)
        //    throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

        for (int i = 0; i < listOfEntries.Count; i++)
        {
            this.Add(listOfEntries[i]);
        }
    }
}
