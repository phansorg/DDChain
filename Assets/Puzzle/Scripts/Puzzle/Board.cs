using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 盤面クラス
public class Board : MonoBehaviour
{
    // const
    private const float FillBlockDuration = 0.15f;
    private const float SwitchBlockCuration = 0.03f;
    private const float BonusBase = 1;
    private const int AllDeleteBonus = 3;
    private const float BoardHeight = 0.79f;
    private const float PaddingBottom = 0.05f;
    private const int GarbagePercent = 15;
    private const float GarbageScore = 0.1f;

    // serialize field.
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private Text allScoreText;
    [SerializeField]
    private Text singleScoreText;
    [SerializeField]
    private Text colorCountText;

    // public.
    public int blockWidth;
    public int chain;
    public int[] score;

    // private.
    private Block[,] board;
    private int width;
    private int height;
    private int randomSeed;

    private int machingCount;
    private int color;
    private int[] colorCount;
    private int garbageCount;
    private string[] colorString = { "R", "B", "G", "Y", "P"};
    private int[] chainScore;
    private int garbageDelete;
    private int countDisp;
    private int garbage;

    private List<Block> CheckingBlockList = new List<Block>();

    /*
    private int[] chainBonus = {   1,   2,   4,   8 , 12 , 16,  20,  24,  28,  32, 
                                  36,  40,  44,  48,  52,  56,  60,  64,  68,  72,
                                  76,  80,  84,  88,  92,  96, 100, 104, 108, 112,
                                 116, 120, 124, 128, 132, 136, 140, 144, 148, 152,
                               };
    */
    private int[] chainBonus = {   1,   2,   3,   4,   8,  16,  24,  32,  40,  48, 
                                  56,  64,  72,  80,  88,  96, 104, 112, 120, 128,
                                 136, 144, 152, 160, 168, 176, 184, 192, 200, 208,
                                 216, 224, 232, 240, 248, 256, 264, 272, 280, 288,
                               };


    //-------------------------------------------------------
    // Public Function
    //-------------------------------------------------------
    // 特定の幅と高さに盤面を初期化する
    public void InitializeBoard(int boardWidth, int boardHeight)
    {
        DataManager dataManager = DataManager.Instance;
        machingCount = dataManager.PuzzleData.Link;
        color = dataManager.PuzzleData.Color;
        colorCount = new int[color];
        score = new int[color];
        chainScore = new int[color];
        countDisp = dataManager.PuzzleData.CountDisp;
        garbage = dataManager.PuzzleData.Garbage;

        width = boardWidth;
        height = boardHeight;

        blockWidth = Screen.width / boardWidth;
        if (Screen.height * BoardHeight / boardHeight < blockWidth)
        {
            blockWidth = (int)(Screen.height * BoardHeight / boardHeight);
        }

        board = new Block[width, height];

        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                CreateBlock(new Vector2(i, j));
            }
        }

        while (color >= 5 && machingCount >= 4 && HasMatch())
        {
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    SetKind(board[i, j]);
                }
            }
        }

        DispScore();
    }

    // 入力されたクリック(タップ)位置から最も近いピースの位置を返す
    public Block GetNearestBlock(Vector3 input)
    {
        var x = Mathf.Min((int)(input.x / blockWidth), width - 1);
        if (x < 0)
        {
            x = 0;
        }

        var inputY = input.y - Screen.height * PaddingBottom;
        var y = Mathf.Min((int)(inputY / blockWidth), height - 1);
        if (y < 0)
        {
            y = 0;
        }

        return board[x, y];
    }

    // 盤面上のピースを交換する
    public void SwitchBlock(Block p1, Block p2)
    {

        // 位置と盤面データを取得する
        //Vector2 p1Pos = GetBlockWorldPos(GetBlockBoardPos(p1));
        Vector2 p2Pos = GetBlockWorldPos(GetBlockBoardPos(p2));
        var p1BoardPos = GetBlockBoardPos(p1);
        var p2BoardPos = GetBlockBoardPos(p2);

        // 途中経路のブロックを全て移動する
        Vector2 d = p2BoardPos - p1BoardPos;
        int idxMax;
        if (Mathf.Abs((int)d.x) >= Mathf.Abs((int)d.y))
        {
            idxMax = Mathf.Abs((int)d.x);
        }
        else
        {
            idxMax = Mathf.Abs((int)d.y);
        }

        Vector2 beforeBoardPos = p1BoardPos;
        for (int idx = 1; idx <= idxMax; idx++)
        {
            Vector2 currentBoardPos = p1BoardPos;

            currentBoardPos.x = currentBoardPos.x + Mathf.CeilToInt(d.x * idx / idxMax);

            if (currentBoardPos.x != beforeBoardPos.x)
            {
                Block switchBlock = board[(int)currentBoardPos.x, (int)beforeBoardPos.y];
                Vector2 MoveToPos = GetBlockWorldPos(beforeBoardPos);
                switchBlock.tweenFlag = true;
                iTween.MoveTo(switchBlock.gameObject, iTween.Hash("x", MoveToPos.x, 
                                                                  "y", MoveToPos.y, 
                                                                  "time", SwitchBlockCuration,
                                                                  "oncomplete", "OnTweenComplete",
                                                                  "oncompletetarget", switchBlock.gameObject));
                board[(int)beforeBoardPos.x, (int)beforeBoardPos.y] = switchBlock;
            }
            beforeBoardPos.x = currentBoardPos.x;

            currentBoardPos.y = currentBoardPos.y + Mathf.CeilToInt(d.y * idx / idxMax);

            if (currentBoardPos.y != beforeBoardPos.y)
            {
                Block switchBlock = board[(int)currentBoardPos.x, (int)currentBoardPos.y];
                Vector2 MoveToPos = GetBlockWorldPos(beforeBoardPos);
                switchBlock.tweenFlag = true;
                iTween.MoveTo(switchBlock.gameObject, iTween.Hash("x", MoveToPos.x, 
                                                                  "y", MoveToPos.y,
                                                                  "time", SwitchBlockCuration,
                                                                  "oncomplete", "OnTweenComplete",
                                                                  "oncompletetarget", switchBlock.gameObject));
                board[(int)beforeBoardPos.x, (int)beforeBoardPos.y] = switchBlock;
            }
            beforeBoardPos = currentBoardPos;
        }

        // 選択中ブロックの更新
        p1.tweenFlag = true;
        iTween.MoveTo(p1.gameObject, iTween.Hash("x", p2Pos.x, 
                                                 "y", p2Pos.y,
                                                 "time", SwitchBlockCuration,
                                                 "oncomplete", "OnTweenComplete",
                                                 "oncompletetarget", p1.gameObject));
        board[(int)p2BoardPos.x, (int)p2BoardPos.y] = p1;
    }

    // 盤面上にマッチングしているピースがあるかどうかを判断する
    public bool HasMatch()
    {
        bool match = false;

        ResetCheckFlag(null);

        for (int kind = 0; kind < colorCount.Length; kind++)
        {
            colorCount[kind] = 0;
        }
        garbageCount = 0;

        foreach (var block in board)
        {
            if (block != null)
            {
                if (block.garbageKind == GarbageKind.None || block.garbageKind == GarbageKind.Dark)
                {
                    colorCount[(int)block.GetKind()]++;
                }
                else
                {
                    garbageCount++;
                }
            }

            SetLinkDirection(block);

            if (IsMatchBlock(block))
            {
                match = true;
            }
        }

        string countString = string.Empty;
        if (countDisp != 0)
        {
            for (int kind = 0; kind < colorCount.Length; kind++)
            {
                countString += colorString[kind] + ":" + colorCount[kind].ToString("D2") + " ";
            }
        }
        colorCountText.text = countString;

        return match;
    }

    // 盤面上に削除したピースがあるかどうかを判断する
    public bool HasDeleteBlock()
    {
        var count = 0;

        foreach (var block in board)
        {
            if (block == null)
            {
                continue;
            }

            if (block.GetDeleteFlag() == false)
            {
                count++;
            }
        }

        return count != width * height;
    }

    // マッチングしているピースを削除する
    public IEnumerator DeleteMatchBlock(Action endCallBadk)
    {
        HasMatch();

        Action<Vector2> DeleteGarbage = (nextPos) =>
        {
            if (ExistBlock(nextPos))
            {
                Block nextBlock = board[(int)nextPos.x, (int)nextPos.y];
                if (nextBlock.garbageKind == GarbageKind.Garbage)
                {
                    garbageDelete++;
                    Destroy(nextBlock.gameObject);
                }
                else if (nextBlock.garbageKind == GarbageKind.Hard)
                {
                    garbageDelete++;
                    nextBlock.SetGarbageKind(GarbageKind.Garbage);
                }
                else if (nextBlock.garbageKind == GarbageKind.Dark)
                {
                    garbageDelete++;
                    nextBlock.SetGarbageKind(GarbageKind.None);
                }
            }
        };

        // 削除フラグが立っているオブジェクトを削除する
        foreach (Block block in board)
        {
            if (block != null && block.GetDeleteFlag())
            {
                chainScore[(int)block.GetKind()] += chainBonus[chain - 1];
                Destroy(block.gameObject);

                Vector2 blockPos = GetBlockBoardPos(block);
                DeleteGarbage(blockPos + Vector2.up);
                DeleteGarbage(blockPos + Vector2.right);
                DeleteGarbage(blockPos + Vector2.down);
                DeleteGarbage(blockPos + Vector2.left);

            }
        }

        yield return new WaitForSeconds(FillBlockDuration);
        endCallBadk();
    }

    // 削除した部分の上のピースを落とす
    public IEnumerator DropBlock(Action endCallBack)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (ExistBlock(new Vector2(i, j)) == false)
                {
                    DropBlock(new Vector2(i, j));
                }
            }
        }

        yield return new WaitForSeconds(FillBlockDuration);
        endCallBack();
    }

    // ピースが消えている場所を詰めて、新しいピースを生成する
    public IEnumerator FillBlock(Action endCallBack)
    {
        ResetCheckFlag(null);

        bool setKindFlag = false;
        if (AllDelete())
        {
            setKindFlag = true;
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                FillBlock(new Vector2(i, j));
            }
        }

        while (color >= 5 && machingCount >= 4 && setKindFlag && HasMatch())
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    SetKind(board[i, j]);
                }
            }
        }


        yield return new WaitForSeconds(FillBlockDuration);
        endCallBack();
    }

    public Block InstantiateBlock(Vector3 createPos)
    {
        var block = Instantiate(blockPrefab, createPos, Quaternion.identity).GetComponent<Block>();
        block.transform.SetParent(transform);
        return block;
    }

    public void DispScore()
    {
        float allDelete = BonusBase;

        if (AllDelete())
        {
            allDelete *= AllDeleteBonus;
        }

        int allScore = 0;
        string singleScoreString = string.Empty;

        for (int kind = 0; kind < colorCount.Length; kind++)
        {
            score[kind] += (int)(chainScore[kind] * allDelete * (1 + garbageDelete * GarbageScore));
            chainScore[kind] = 0;
            allScore += score[kind];
            singleScoreString += colorString[kind] + ":" + score[kind].ToString("D5") + " ";
        }
        garbageDelete = 0;
        allScoreText.text = "Total " + allScore.ToString("D6");
        singleScoreText.text = "Score " + singleScoreString;
    }

    //-------------------------------------------------------
    // Private Function
    //-------------------------------------------------------
    // 特定の位置にピースを作成する
    private void CreateBlock(Vector2 position)
    {
        // ピースの生成位置を求める
        var createPos = GetBlockWorldPos(position);

        // ピースを生成、ボードの子オブジェクトにする
        Block block = InstantiateBlock(createPos);
        block.SetSize(blockWidth);

        SetKind(block);

        // 盤面にピースの情報をセットする
        board[(int)position.x, (int)position.y] = block;
    }

    private void SetKind(Block block)
    {
        // 生成するピースの種類をランダムに決める
        BlockKind kind = (BlockKind)(UnityEngine.Random.Range(0, PuzzleDataV1.COLOR_MAX) % color);
        block.SetKind(kind);

        // お邪魔情報をセット
        if (garbage != 0 && UnityEngine.Random.Range(0, 100) < GarbagePercent)
        {
            block.SetGarbageKind((GarbageKind)garbage);
        }
        else
        {
            block.SetGarbageKind(GarbageKind.None);
        }
    }

    // 盤面上の位置からピースオブジェクトのワールド座標での位置を返す
    private Vector3 GetBlockWorldPos(Vector2 boardPos)
    {
        return new Vector3(boardPos.x * blockWidth + (blockWidth / 2), boardPos.y * blockWidth + (blockWidth / 2) + Screen.height * PaddingBottom, 0);
    }

    // ピースが盤面上のどの位置にあるのかを返す
    private Vector2 GetBlockBoardPos(Block block)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (board[i, j] == block)
                {
                    return new Vector2(i, j);
                }
            }
        }

        return Vector2.zero;
    }

    private void SetLinkDirection(Block block)
    {
        if (block == null)
        {
            return;
        }

        // Tween中はLinkしない
        if (block.IsLink() == false)
        {
            block.SetLinkDirection((int)LinkDirection.None);
            return;
        }

        // ピースの情報を取得
        Vector2 blockPos = GetBlockBoardPos(block);
        BlockKind kind = block.GetKind();
        int linkDirection = (int)LinkDirection.None;

        Action<Vector2, int> AddLinkDirection = (nextPos, add) =>
        {
            if (ExistBlock(nextPos))
            {
                Block nextBlock = board[(int)nextPos.x, (int)nextPos.y];
                if (nextBlock.IsLink() && nextBlock.GetKind() == kind)
                {
                    linkDirection += add;
                }
            }
        };

        AddLinkDirection(blockPos + Vector2.up, (int)LinkDirection.Up);
        AddLinkDirection(blockPos + Vector2.right, (int)LinkDirection.Right);
        AddLinkDirection(blockPos + Vector2.down, (int)LinkDirection.Down);
        AddLinkDirection(blockPos + Vector2.left, (int)LinkDirection.Left);

        block.SetLinkDirection(linkDirection);
    }

    // 対象のピースがマッチしているかの判定を行う
    private bool IsMatchBlock(Block block)
    {
        if (block == null)
        {
            return false;
        }

        // ピースの情報を取得
        Vector2 pos = GetBlockBoardPos(block);
        BlockKind kind = block.GetKind();

        if (block.checkFlag)
        {
            return false;
        }

        CheckingBlockList.Clear();
        CheckingBlockList.Add(block);

        var MatchCount = GetSameKindBlockNum(kind, pos) + 1;

        bool deleteFlag = (MatchCount >= machingCount);
        foreach (Block checkingBlock in CheckingBlockList)
        {
            checkingBlock.SetDeleteFlag(deleteFlag);
        }

        return deleteFlag;
    }

    // 対象の方向に引数で指定したの種類のピースがいくつあるかを返す
    private int GetSameKindBlockNum(BlockKind kind, Vector2 blockPos)
    {
        if (ExistBlock(blockPos) == false)
        {
            return 0;
        }

        Block block = board[(int)blockPos.x, (int)blockPos.y];
        if (block.checkFlag)
        {
            return 0;
        }

        block.checkFlag = true;

        if (block.IsLink() == false)
        {
            return 0;
        }

        var count = 0;

        Action<Vector2> ProcNextPos = (nextPos) =>
        {
            if (ExistBlock(nextPos))
            {
                Block nextBlock = board[(int)nextPos.x, (int)nextPos.y];
                BlockKind nextKind = nextBlock.GetKind();
                if (nextBlock.IsLink() == true && nextKind == kind && nextBlock.checkFlag == false)
                {
                    CheckingBlockList.Add(nextBlock);
                    count++;
                    count += GetSameKindBlockNum(nextKind, nextPos);
                }
            }
        };

        ProcNextPos(blockPos + Vector2.up);
        ProcNextPos(blockPos + Vector2.right);
        ProcNextPos(blockPos + Vector2.down);
        ProcNextPos(blockPos + Vector2.left);

        return count;
    }

    // 対象の座標がボードに存在するか(ボードからはみ出していないか)を判定する
    private bool IsInBoard(Vector2 pos)
    {
        if (pos.x < 0 || pos.y < 0 || pos.x >= width || pos.y >= height)
        {
            return false;
        }

        return true;
    }

    // 対象の座標にブロックが存在するかを判定する
    private bool ExistBlock(Vector2 pos)
    {
        if (IsInBoard(pos) == false)
        {
            return false;
        }

        if (board[(int)pos.x, (int)pos.y] == null)
        {
            return false;
        }

        return true;
    }


    // 特定のピースのが削除されているかを判断し、削除されているなら詰める
    private void DropBlock(Vector2 pos)
    {
        var block = board[(int)pos.x, (int)pos.y];

        // 対象のピースより上方向に有効なピースがあるかを確認、あるなら場所を移動させる
        var checkPos = pos + Vector2.up;
        while (IsInBoard(checkPos))
        {
            if (ExistBlock(checkPos))
            {
                /*
                var checkBlock = board[(int)checkPos.x, (int)checkPos.y];
                if (checkBlock != null && !checkBlock.GetDeleteFlag())
                {
                    checkBlock.transform.position = GetBlockWorldPos(pos);
                    board[(int)pos.x, (int)pos.y] = checkBlock;
                    board[(int)checkPos.x, (int)checkPos.y] = null;
                    return;
                }
                */
                var checkBlock = board[(int)checkPos.x, (int)checkPos.y];
                checkBlock.transform.position = GetBlockWorldPos(pos);
                board[(int)pos.x, (int)pos.y] = checkBlock;
                board[(int)checkPos.x, (int)checkPos.y] = null;
                return;

            }

            checkPos += Vector2.up;
        }
    }

    // 新しく生成する
    private void FillBlock(Vector2 pos)
    {
        var block = board[(int)pos.x, (int)pos.y];
        if (block != null && !block.GetDeleteFlag())
        {
            // ピースが削除されていなければ何もしない
            return;
        }

        // 有効なピースがなければ新しく作る
        CreateBlock(pos);
    }

    private void ResetCheckFlag(Block targetBlock)
    {
        if (targetBlock == null)
        {
            foreach (var block in board)
            {
                if (block == null)
                {
                    continue;
                }

                block.SetDeleteFlag(false);
                block.checkFlag = false;
            }
        }
        else
        {
            targetBlock.SetDeleteFlag(false);
            targetBlock.checkFlag = false;
        }
    }

    private bool AllDelete()
    {
        for (int kind = 0; kind < colorCount.Length; kind++)
        {
            if (colorCount[kind] != 0)
            {
                return false;
            }
        }

        if (garbageCount != 0)
        {
            return false;
        }

        return true;
    }

}