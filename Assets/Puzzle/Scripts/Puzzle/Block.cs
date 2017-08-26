using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BlockKind
{
    Red = 0,
    Blue,
    Green,
    Yellow,
    Purple,
}

public enum GarbageKind
{
    None = 0,
    Garbage,
    Hard,
    Dark,
}

public enum LinkDirection
{
    None = 0,
    Up = 1,
    Right = 2,
    Down = 4,
    Left = 8,
}

public class Block : MonoBehaviour {

    public bool selectFlag;
    public bool tweenFlag;
    public bool checkFlag;
    public GarbageKind garbageKind;

    private bool deleteFlag;
    private Image thisImage;
    private RectTransform thisRectTransform;
    private BlockKind kind;
    private int linkDirection;

    public void Update()
    {
        SetImage();
    }

    //-------------------------------------------------------
    // MonoBehaviour Function
    //-------------------------------------------------------
    // 初期化処理
    private void Awake()
    {
        // アタッチされている各コンポーネントを取得
        thisImage = GetComponent<Image>();
        thisRectTransform = GetComponent<RectTransform>();

        selectFlag = false;
        tweenFlag = false;
        checkFlag = false;
        deleteFlag = false;
        linkDirection = 0;
        garbageKind = GarbageKind.None;
    }

    //-------------------------------------------------------
    // Public Function
    //-------------------------------------------------------
    public void SetKind(BlockKind blockKind)
    {
        kind = blockKind;
        SetImage();
    }

    // ピースの種類を返す
    public BlockKind GetKind()
    {
        return kind;
    }

    // ピースのサイズをセットする
    public void SetSize(int size)
    {
        this.thisRectTransform.sizeDelta = Vector2.one * size;
    }

    public void SetLinkDirection(int linkDirection)
    {
        this.linkDirection = linkDirection;
        SetImage();
    }

    public void SetDeleteFlag(bool deleteFlag)
    {
        this.deleteFlag = deleteFlag;
    }

    public bool GetDeleteFlag()
    {
        return deleteFlag;
    }

    public void SetGarbageKind(GarbageKind garbageKind)
    {
        this.garbageKind = garbageKind;
        SetImage();
    }

    //-------------------------------------------------------
    // Private Function
    //-------------------------------------------------------
    // ピースの色を自身の種類の物に変える
    private void SetImage()
    {
        SpriteManager spriteManager = SpriteManager.Instance;

        switch (garbageKind)
        {
            case GarbageKind.Garbage:
                thisImage.sprite = spriteManager.blocks_5[linkDirection];
                return;
            case GarbageKind.Hard:
                thisImage.sprite = spriteManager.blocks_11[linkDirection];
                return;
            case GarbageKind.Dark:
                switch (kind)
                {
                    case BlockKind.Red:
                        thisImage.sprite = spriteManager.blocks_6[linkDirection];
                        return;
                    case BlockKind.Blue:
                        thisImage.sprite = spriteManager.blocks_7[linkDirection];
                        return;
                    case BlockKind.Green:
                        thisImage.sprite = spriteManager.blocks_8[linkDirection];
                        return;
                    case BlockKind.Yellow:
                        thisImage.sprite = spriteManager.blocks_9[linkDirection];
                        return;
                    case BlockKind.Purple:
                        thisImage.sprite = spriteManager.blocks_10[linkDirection];
                        return;

                }
                return;
        }

        switch (kind)
        {
            case BlockKind.Red:
                thisImage.sprite = spriteManager.blocks_0[linkDirection];
                break;
            case BlockKind.Blue:
                thisImage.sprite = spriteManager.blocks_1[linkDirection];
                break;
            case BlockKind.Green:
                thisImage.sprite = spriteManager.blocks_2[linkDirection];
                break;
            case BlockKind.Yellow:
                thisImage.sprite = spriteManager.blocks_3[linkDirection];
                break;
            case BlockKind.Purple:
                thisImage.sprite = spriteManager.blocks_4[linkDirection];
                break;

        }
    }

    // ピースの透過を設定する
    public void SetBlockAlpha(float alpha)
    {
        var col = thisImage.color;
        col.a = alpha;
        thisImage.color = col;
    }

    public void OnTweenComplete()
    {
        tweenFlag = false;
    }

    public bool IsLink()
    {
        if (tweenFlag == false &&
            selectFlag == false &&
            garbageKind == GarbageKind.None
            )
        {
            return true;
        }

        return false;
    }
}
