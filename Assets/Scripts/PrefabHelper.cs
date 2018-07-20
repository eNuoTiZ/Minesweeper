using System.Collections.Generic;
using UnityEngine;

public class PrefabHelper
{

    public GameObject CellPrefab;
    public Sprite FacingDownSprite;
    public Sprite EmptySprite;
    public Sprite FlagSprite;
    public Sprite BombSprite;
    public Sprite ExplodedBombSprite;
    public Sprite BadBombGuessSprite;
    public GameObject ExplosionPrefab;
    public Sprite[] BombNumberSprite;

    public Sprite HappySmiley;
    public Sprite SadSmiley;
    public Sprite SunGlassesSmiley;
    public Sprite OhSmiley;

    // AudioSource
    public List<AudioSource> MenuSoundsAudioSourcesList;
    public List<AudioSource> SoundEffectsAudioSourcesList;
    public AudioSource BackgroundGameSound;

    private static PrefabHelper instance;
    private static object _lock = new Object();

    private PrefabHelper()
    {

    }

    public static PrefabHelper Instance
    {
        get
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new PrefabHelper();
                }
            }

            return instance;
        }

        set
        {
            instance = value;
        }
    }
}
