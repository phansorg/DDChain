using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour {

    public Sprite[] blocks_0;
    public Sprite[] blocks_1;
    public Sprite[] blocks_2;
    public Sprite[] blocks_3;
    public Sprite[] blocks_4;
    public Sprite[] blocks_5;
    public Sprite[] blocks_6;
    public Sprite[] blocks_7;
    public Sprite[] blocks_8;
    public Sprite[] blocks_9;
    public Sprite[] blocks_10;
    public Sprite[] blocks_11;

    public static SpriteManager Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        blocks_0 = Resources.LoadAll<Sprite>("Block/Blocks_0");
        blocks_1 = Resources.LoadAll<Sprite>("Block/Blocks_1");
        blocks_2 = Resources.LoadAll<Sprite>("Block/Blocks_2");
        blocks_3 = Resources.LoadAll<Sprite>("Block/Blocks_3");
        blocks_4 = Resources.LoadAll<Sprite>("Block/Blocks_4");
        blocks_5 = Resources.LoadAll<Sprite>("Block/Blocks_5");
        blocks_6 = Resources.LoadAll<Sprite>("Block/Blocks_6");
        blocks_7 = Resources.LoadAll<Sprite>("Block/Blocks_7");
        blocks_8 = Resources.LoadAll<Sprite>("Block/Blocks_8");
        blocks_9 = Resources.LoadAll<Sprite>("Block/Blocks_9");
        blocks_10 = Resources.LoadAll<Sprite>("Block/Blocks_10");
        blocks_11 = Resources.LoadAll<Sprite>("Block/Blocks_11");
    }
}
