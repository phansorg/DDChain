using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GodTouches;

public class PuzzleController : MonoBehaviour {

    private const int ReleaseCountMax = -1;

    // enum.
    public enum PuzzleState
    {
        Idle,
        BlockMove,
        ReleaseWait,
        MatchCheck,
        DeleteBlock,
        DropBlock,
        FillBlock,
        Wait,
        SendScoreData,
        Result,
    }

    // serialize field.
    [SerializeField]
    private Board board;
    [SerializeField]
    private Text stateText;
    [SerializeField]
    private Text timerText;

    // private.
    public int MachingCount;
    public static PuzzleState currentState;
    private Block selectedBlock;
    private GameObject movingBlockObject;

    private float startTime;
    private float countTime;
    private float remainTime;
    private bool chainFlag;

    private int ReleaseCount;

    ScoreDataV1[] scoreData;
    private int seed;

    private AudioSource se1Sec;
//    private AudioSource se10Sec;
    private bool[] se1SecFlags;
    private bool[] se10SecFlags;

    private bool scoreSending;

    //-------------------------------------------------------
    // MonoBehaviour Function
    //-------------------------------------------------------
    // ゲームの初期化処理
    private void Start()
    {
        InitScoreData();

        DataManager dataManager = DataManager.Instance;
        board.InitializeBoard(dataManager.PuzzleData.Col, dataManager.PuzzleData.Row);
        board.HasMatch();

//        startTime = dataManager.PuzzleData.Time;
        startTime = 10;
        countTime = 0;
        remainTime = 0;
        chainFlag = false;

        currentState = PuzzleState.Idle;

        //AudioSourceコンポーネントを取得し、変数に格納
        AudioSource[] audioSources = GetComponents<AudioSource>();
        se1Sec = audioSources[0];
//        se10Sec = audioSources[1];
        se1SecFlags = new bool[6];
        se10SecFlags = new bool[dataManager.PuzzleData.Time / 10];

        scoreSending = false;
    }

    // ゲームのメインループ
    private void Update()
    {
        if (chainFlag)
        {
            if (currentState == PuzzleState.Idle)
            {
                chainFlag = false;
            }
        }
        else
        {
            countTime += Time.deltaTime;
        }

        remainTime = startTime - countTime;
        if (remainTime < 0)
        {
            remainTime = 0;
        }
        timerText.text = remainTime.ToString("F1");

        if (countTime >= 10)
        {
            int idx1Sec = Mathf.CeilToInt(remainTime);
            int idx10Sec = (idx1Sec + 9) / 10 - 1;

            if (idx1Sec <= 5)
            {
                if (se1SecFlags[idx1Sec] == false)
                {
                    se1SecFlags[idx1Sec] = true;
                    se1Sec.PlayOneShot(se1Sec.clip);
                }
            }
            else if (idx10Sec >= 0)
            {
                if (se10SecFlags[idx10Sec] == false)
                {
                    se10SecFlags[idx10Sec] = true;
                    //se10Sec.PlayOneShot(se10Sec.clip);
                    se1Sec.PlayOneShot(se1Sec.clip);
                }
            }
        }

        switch (currentState)
        {
            case PuzzleState.Idle:
                Idle();
                break;
            case PuzzleState.BlockMove:
                BlockMove();
                break;
            case PuzzleState.ReleaseWait:
                ReleaseWait();
                break;
            case PuzzleState.MatchCheck:
                MatchCheck();
                break;
            case PuzzleState.DeleteBlock:
                DeleteBlock();
                break;
            case PuzzleState.DropBlock:
                DropBlock();
                break;
            case PuzzleState.FillBlock:
                FillBlock();
                break;
            case PuzzleState.SendScoreData:
                SendScoreData();
                break;
            case PuzzleState.Result:
                Result();
                break;
            default:
                break;
        }

        //stateText.text = currentState.ToString();
    }

    //-------------------------------------------------------
    // Private Function
    //-------------------------------------------------------
    private void InitScoreData()
    {
        DataManager dataManager = DataManager.Instance;
        scoreData = new ScoreDataV1[ScoreDataV1.SCORE_KIND_MAX];

        ScoreManager scoreManager = ScoreManager.Instance;
        for (int scoreKind = 0; scoreKind < scoreData.Length; scoreKind++)
        {
            scoreData[scoreKind] = new ScoreDataV1();

            scoreData[scoreKind].Id = dataManager.UserData.Id;
            scoreData[scoreKind].Name = dataManager.UserData.Name;
            scoreData[scoreKind].Row = dataManager.PuzzleData.Row;
            scoreData[scoreKind].Col = dataManager.PuzzleData.Col;
            scoreData[scoreKind].Color = dataManager.PuzzleData.Color;
            scoreData[scoreKind].Link = dataManager.PuzzleData.Link;
            scoreData[scoreKind].Direction = dataManager.PuzzleData.Direction;
            scoreData[scoreKind].Time = dataManager.PuzzleData.Time;
            scoreData[scoreKind].Stop = dataManager.PuzzleData.Stop;
            scoreData[scoreKind].CountDisp = dataManager.PuzzleData.CountDisp;
            scoreData[scoreKind].Garbage = dataManager.PuzzleData.Garbage;

            scoreData[scoreKind].ScoreKindValue = scoreKind;
            scoreManager.getScore(scoreData[scoreKind]);
        }

        seed = (int)((DateTime.Now.ToBinary() + dataManager.PuzzleData.WriteCount) % int.MaxValue);
    }

    // プレイヤーの入力を検知し、ピースを選択状態にする
    private void Idle()
    {
        if (remainTime == 0)
        {
            currentState = PuzzleState.SendScoreData;
        }
        else
        {
            GodPhase phase = GodTouch.GetPhase();
            if (phase == GodPhase.Began)
            {
                SelectBlock();
            }
        }

        board.HasMatch();
    }

    // プレイヤーがピースを選択しているときの処理、入力終了を検知したらReleaseWaitに移行する
    private void BlockMove()
    {
        if (remainTime == 0)
        {
            selectedBlock.SetBlockAlpha(1f);
            selectedBlock.selectFlag = false;
            Destroy(movingBlockObject);
            currentState = PuzzleState.MatchCheck;
        }
        else
        {
            GodPhase phase = GodTouch.GetPhase();
            if (phase == GodPhase.Moved)
            {
                var Block = board.GetNearestBlock(GodTouch.GetPosition());
                if (Block != selectedBlock)
                {
                    board.SwitchBlock(selectedBlock, Block);
                }
                movingBlockObject.transform.position = GodTouch.GetPosition() + Vector3.up * 10;

            }
            else if (phase == GodPhase.Ended)
            {
                ReleaseCount = 0;
                currentState = PuzzleState.ReleaseWait;
            }
        }

        board.HasMatch();

    }

    // 再度タッチした場合はBlockMoveに移行、タッチされなければ盤面のチェックの状態に移行する
    private void ReleaseWait()
    {
        GodPhase phase = GodTouch.GetPhase();
        if (phase == GodPhase.Began || phase == GodPhase.Moved)
        {
            currentState = PuzzleState.BlockMove;
        }
        else
        {
            stateText.text = string.Format("RC:{0} RM:{1} itween:{2}", ReleaseCount, ReleaseCountMax, iTween.tweens.Count);
            if (ReleaseCount > ReleaseCountMax && iTween.tweens.Count == 0)
            {
                selectedBlock.SetBlockAlpha(1f);
                selectedBlock.selectFlag = false;
                Destroy(movingBlockObject);
                currentState = PuzzleState.MatchCheck;
            }
            else
            {
                ReleaseCount++;
            }
        }

        board.HasMatch();
    }

    // 盤面上にマッチングしているピースがあるかどうかを判断する
    private void MatchCheck()
    {
        if (board.HasMatch())
        {
            board.chain++;
            currentState = PuzzleState.DeleteBlock;
            chainFlag = true;
        }
        else if (board.HasDeleteBlock())
        {
            board.DispScore();
            currentState = PuzzleState.FillBlock;
        }
        else
        {
            board.chain = 0;
            currentState = PuzzleState.Idle;
        }
    }

    // マッチングしているピースを削除する
    private void DeleteBlock()
    {
        currentState = PuzzleState.Wait;
        StartCoroutine(board.DeleteMatchBlock(() => currentState = PuzzleState.DropBlock));

        board.HasMatch();
    }

    // 削除した部分の上のピースを落とす
    private void DropBlock()
    {
        currentState = PuzzleState.Wait;
        StartCoroutine(board.DropBlock(() => currentState = PuzzleState.MatchCheck));

        board.HasMatch();
    }

    // 盤面上のかけている部分にピースを補充する
    private void FillBlock()
    {
        currentState = PuzzleState.Wait;
        board.chain = 0;
        StartCoroutine(board.FillBlock(() => currentState = PuzzleState.Idle));

        board.HasMatch();
    }

    private void SendScoreData()
    {
        Text label;
        label = GameObject.Find("Text").GetComponent<Text>();
        label.text = "Wait...";

        StartCoroutine(ProcScoreData(() => currentState = PuzzleState.Result));
    }

    private void Result()
    {
        Text label;
        label = GameObject.Find("Text").GetComponent<Text>();
        label.text = "Menu";

        GodPhase phase = GodTouch.GetPhase();
        if (phase == GodPhase.Began)
        {

            iTween.tweens.Clear();
            SceneManager.LoadScene("MainMenu");
        }
    }

    // ピースを選択する処理
    private void SelectBlock()
    {
        selectedBlock = board.GetNearestBlock(GodTouch.GetPosition());
        if (selectedBlock.garbageKind != GarbageKind.None)
        {
            return;
        }

        selectedBlock.SetBlockAlpha(0f);
        selectedBlock.selectFlag = true;

        var block = board.InstantiateBlock(GodTouch.GetPosition());
        block.SetKind(selectedBlock.GetKind());
        block.SetSize((int)(board.blockWidth * 1.2f));
        movingBlockObject = block.gameObject;

        currentState = PuzzleState.BlockMove;
    }

    private IEnumerator ProcScoreData(Action endCallBack)
    {
        if (scoreSending == false)
        {
            DataManager dataManager = DataManager.Instance;

            int[] colorScore = new int[ScoreDataV1.SCORE_KIND_MAX];
            long playDateTime = DateTime.Now.ToBinary();

            foreach (int score in board.score)
            {
                colorScore[(int)ScoreDataV1.ScoreKind.AllColor] += score;
                if (colorScore[(int)ScoreDataV1.ScoreKind.SingleColor] < score)
                {
                    colorScore[(int)ScoreDataV1.ScoreKind.SingleColor] = score;
                }
            }

            ScoreManager scoreManager = ScoreManager.Instance;
            for (int scoreKind = 0; scoreKind < scoreData.Length; scoreKind++)
            {
                if (colorScore[scoreKind] > scoreManager.highScore[scoreKind])
                {
                    scoreData[scoreKind].Score = colorScore[scoreKind];
                    scoreData[scoreKind].PlayDateTime = playDateTime;
                    scoreManager.save(scoreData[scoreKind]);
                }
            }
            
        }

        scoreSending = true;
        yield return new WaitForSeconds(1);
        endCallBack();
    }

}
