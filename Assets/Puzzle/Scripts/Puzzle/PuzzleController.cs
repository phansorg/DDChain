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

    private enum InputType
    {
        Select = 0,
        Move,
        Release,
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
    ReplayDataV1[] replayData;
    private int frame;
    private List<int> inputFrame;
    private List<byte> inputType;
    private List<byte> inputData1;
    private List<byte> inputData2;

    private AudioSource se1Sec;
//    private AudioSource se10Sec;
    private bool[] se1SecFlags;
    private bool[] se10SecFlags;

    private bool scoreSending;

    private bool replay;
    private int replayScoreKind;
    private int replayIdx;

    private bool practice;
    public bool writeBlock;
    public BlockKind writeBlockKind;
    public GarbageKind writeGarbageKind;

    int[] colorScore;

    public bool replayStop;

    //-------------------------------------------------------
    // MonoBehaviour Function
    //-------------------------------------------------------
    // ゲームの初期化処理
    private void Start()
    {
        DataManager dataManager = DataManager.Instance;
        if (dataManager.ReplayData.Version != 0)
        {
            // リプレイ
            replay = true;
            replayScoreKind = dataManager.ReplayData.ScoreKindValue;
            practice = false;
        }
        else
        {
            //リプレイでない
            replay = false;

            if (dataManager.PuzzleData.Practice != 0)
            {
                practice = true;
            }
            else
            {
                practice = false;
            }
        }
        Debug.Log("replay:" + replay);
        Debug.Log("practice:" + practice);

        Action<string> HideObject = (objectName) =>
        {
            Transform transform = GameObject.Find(objectName).GetComponent<Transform>();
            Vector3 pos;
            pos = transform.position;
            pos.x -= 2000;
            transform.position = pos;
        };
        if (replay == false)
        {
            HideObject("StopToggle");
        }
        if (practice)
        {
            HideObject("TimerText");
            HideObject("AllScoreText");
            HideObject("SingleScoreText");
        }
        else
        {
            HideObject("ObjectDropdown");
            HideObject("SaveButton");
            HideObject("LoadButton");
        }

        InitScoreData();

        board.InitializeBoard(dataManager.PuzzleData.Col, dataManager.PuzzleData.Row);
        board.HasMatch();

        startTime = dataManager.PuzzleData.Time;
//        startTime = 10;
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

        replayIdx = 0;

        writeBlock = false;
        replayStop = false;
    }

    // ゲームのメインループ
    private void Update()
    {
        if (replayStop)
        {
            return;
        }

        if (chainFlag)
        {
            if (currentState == PuzzleState.Idle)
            {
                chainFlag = false;
            }
        }
        else
        {
            if (currentState != PuzzleState.SendScoreData &&
                currentState != PuzzleState.Result)
            {
                countTime += Time.deltaTime;
                frame++;
            }
        }

        if (replay)
        {
            remainTime = replayData[0].FrameCount - frame;
            if (remainTime < 0)
            {
                remainTime = 0;
            }
            timerText.text = remainTime.ToString() + " Frame";
        }
        else
        {
            if (practice)
            {
                remainTime = startTime;
            }
            else
            {
                remainTime = startTime - countTime;
                if (remainTime < 0)
                {
                    remainTime = 0;
                }
                timerText.text = remainTime.ToString("F1") + " Sec";

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
            }
        }

        // currentStateによる処理の切り分け(遷移も発生)
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
        replayData = new ReplayDataV1[ScoreDataV1.SCORE_KIND_MAX];

        int seed;
        if (replay)
        {
            seed = dataManager.ReplayData.Seed;
        }
        else
        {
            seed = (int)((DateTime.Now.ToBinary() + dataManager.PuzzleData.WriteCount) % int.MaxValue);
        }

        ScoreManager scoreManager = ScoreManager.Instance;
        for (int scoreKind = 0; scoreKind < scoreData.Length; scoreKind++)
        {
            scoreData[scoreKind] = new ScoreDataV1();

            if (replay)
            {
                scoreData[scoreKind].Id = dataManager.ReplayData.Id;
                scoreData[scoreKind].Name = dataManager.ReplayData.Name;
                scoreData[scoreKind].Row = dataManager.ReplayData.Row;
                scoreData[scoreKind].Col = dataManager.ReplayData.Col;
                scoreData[scoreKind].Color = dataManager.ReplayData.Color;
                scoreData[scoreKind].Link = dataManager.ReplayData.Link;
                scoreData[scoreKind].Direction = dataManager.ReplayData.Direction;
                scoreData[scoreKind].Time = dataManager.ReplayData.Time;
                scoreData[scoreKind].Stop = dataManager.ReplayData.Stop;
                scoreData[scoreKind].CountDisp = dataManager.ReplayData.CountDisp;
                scoreData[scoreKind].Garbage = dataManager.ReplayData.Garbage;
                scoreData[scoreKind].Version = dataManager.ReplayData.Version;
            }
            else
            {
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
                scoreData[scoreKind].Version = CommonDefine.VERSION;
            }

            scoreData[scoreKind].ScoreKindValue = scoreKind;
            scoreManager.getScore(scoreData[scoreKind]);

            if (replay)
            {
                replayData[scoreKind] = dataManager.ReplayData;
                continue;
            }

            replayData[scoreKind] = new ReplayDataV1();
            replayData[scoreKind].Version = CommonDefine.VERSION;
            replayData[scoreKind].Id = dataManager.UserData.Id;
            replayData[scoreKind].ScoreKindValue = scoreKind;
            replayData[scoreKind].Seed = seed;
            replayData[scoreKind].Row = dataManager.PuzzleData.Row;
            replayData[scoreKind].Col = dataManager.PuzzleData.Col;
            replayData[scoreKind].Color = dataManager.PuzzleData.Color;
            replayData[scoreKind].Link = dataManager.PuzzleData.Link;
            replayData[scoreKind].Direction = dataManager.PuzzleData.Direction;
            replayData[scoreKind].Time = dataManager.PuzzleData.Time;
            replayData[scoreKind].Stop = dataManager.PuzzleData.Stop;
            replayData[scoreKind].CountDisp = dataManager.PuzzleData.CountDisp;
            replayData[scoreKind].Garbage = dataManager.PuzzleData.Garbage;
        }

        UnityEngine.Random.seed = seed;
        frame = 0;
        inputFrame = new List<int>();
        inputType = new List<byte>();
        inputData1 = new List<byte>();
        inputData2 = new List<byte>();
    }

    // プレイヤーの入力を検知し、ピースを選択状態にする
    private void Idle()
    {
        if (remainTime == 0)
        {
            CalcColorScore();

            if (replay)
            {
                ScoreManager scoreManager = ScoreManager.Instance;
                Debug.Log("colorScore[replayScoreKind]" + colorScore[replayScoreKind] + " scoreManager.highScore[replayScoreKind]" + scoreManager.highScore[replayScoreKind]);
                if (colorScore[replayScoreKind] == scoreManager.highScore[replayScoreKind])
                {
                    currentState = PuzzleState.Result;
                }
                else
                {
                    currentState = PuzzleState.SendScoreData;
                }
            }
            else
            {
                currentState = PuzzleState.SendScoreData;
            }
        }
        else
        {
            if (replay)
            {
                if (replayIdx < replayData[0].InputCount &&
                    replayData[0].InputFrame[replayIdx] == frame &&
                    replayData[0].InputType[replayIdx] == (byte)InputType.Select)
                {
                    SelectBlock();
                }
            }
            else
            {
                GodPhase phase = GodTouch.GetPhase();
                if (phase == GodPhase.Began)
                {
                    SelectBlock();
                }

            }
        }

        board.HasMatch();
    }

    // プレイヤーがピースを選択しているときの処理、入力終了を検知したらReleaseWaitに移行する
    private void BlockMove()
    {
        if (remainTime == 0)
        {
            ReleaseBlock();
        }
        else
        {
            if (replay)
            {
                if (replayIdx < replayData[0].InputCount &&
                    replayData[0].InputFrame[replayIdx] == frame)
                {
                    if (replayData[0].InputType[replayIdx] == (byte)InputType.Move)
                    {
                        // ボードの処理
                        Vector3 inputPos = board.GetBlockWorldPos(new Vector2(replayData[0].InputData1[replayIdx], replayData[0].InputData2[replayIdx]));
                        Block block = board.board[replayData[0].InputData1[replayIdx], replayData[0].InputData2[replayIdx]];
                        board.SwitchBlock(selectedBlock, block);
                        movingBlockObject.transform.position = inputPos + Vector3.up * 10;
                        replayIdx++;
                    }
                    else if (replayData[0].InputType[replayIdx] == (byte)InputType.Release)
                    {
                        ReleaseBlock();
                        replayIdx++;
                    }
                }
            }
            else
            {
                GodPhase phase = GodTouch.GetPhase();
                if (phase == GodPhase.Moved)
                {
                    Block block = board.GetNearestBlock(GodTouch.GetPosition());
                    if (block != selectedBlock)
                    {
                        // リプレイの処理
                        inputFrame.Add(frame);
                        inputType.Add((int)InputType.Move);
                        Vector2 pos = board.GetBlockBoardPos(block);
                        inputData1.Add((byte)pos.x);
                        inputData2.Add((byte)pos.y);

                        // ボードの処理
                        board.SwitchBlock(selectedBlock, block);
                    }
                    movingBlockObject.transform.position = GodTouch.GetPosition() + Vector3.up * 10;

                }
                else if (phase == GodPhase.Ended)
                {
                    ReleaseCount = 0;
                    currentState = PuzzleState.ReleaseWait;
                }
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
                ReleaseBlock();
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
        label = GameObject.Find("MenuText").GetComponent<Text>();
        label.text = "Wait...";

        StartCoroutine(ProcScoreData(() => currentState = PuzzleState.Result));
    }

    private void Result()
    {
        Text label;
        label = GameObject.Find("MenuText").GetComponent<Text>();
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
        Vector3 inputPos;
        if (replay)
        {
            inputPos = board.GetBlockWorldPos(new Vector2(replayData[0].InputData1[replayIdx], replayData[0].InputData2[replayIdx]));
            selectedBlock = board.board[replayData[0].InputData1[replayIdx], replayData[0].InputData2[replayIdx]];
            replayIdx++;
        }
        else
        {
            // クリック位置取得
            inputPos = GodTouch.GetPosition();
            // 盤外なら処理しない
            if (board.IsInputOut(inputPos))
            {
                return;
            }
            // 近くのブロックを取得
            selectedBlock = board.GetNearestBlock(inputPos);
            // 練習の書込モードの場合、書き込んで処理終了
            if (writeBlock)
            {
                selectedBlock.SetKind(writeBlockKind);
                selectedBlock.garbageKind = writeGarbageKind;
                return;
            }
            // お邪魔は選択できない
            if (selectedBlock.garbageKind != GarbageKind.None)
            {
                return;
            }
            // リプレイの処理
            inputFrame.Add(frame);
            inputType.Add((int)InputType.Select);
            Vector2 pos = board.GetBlockBoardPos(selectedBlock);
            inputData1.Add((byte)pos.x);
            inputData2.Add((byte)pos.y);
        }

        // ボードの処理
        selectedBlock.SetBlockAlpha(0f);
        selectedBlock.selectFlag = true;

        var block = board.InstantiateBlock(inputPos);
        block.SetKind(selectedBlock.GetKind());
        block.SetSize((int)(board.blockWidth * 1.2f));
        movingBlockObject = block.gameObject;

        currentState = PuzzleState.BlockMove;
    }

    // ピースをリリースする処理
    private void ReleaseBlock()
    {
        // リプレイの処理
        inputFrame.Add(frame);
        inputType.Add((int)InputType.Release);
        Vector2 pos = board.GetBlockBoardPos(selectedBlock);
        inputData1.Add((byte)pos.x);
        inputData2.Add((byte)pos.y);

        // ボードの処理
        selectedBlock.SetBlockAlpha(1f);
        selectedBlock.selectFlag = false;
        Destroy(movingBlockObject);
        currentState = PuzzleState.MatchCheck;
    }

    private void CalcColorScore()
    {
        // 各色のスコアデータを集計
        colorScore = new int[ScoreDataV1.SCORE_KIND_MAX];
        foreach (int score in board.score)
        {
            colorScore[(int)ScoreDataV1.ScoreKind.AllColor] += score;
            if (colorScore[(int)ScoreDataV1.ScoreKind.SingleColor] < score)
            {
                colorScore[(int)ScoreDataV1.ScoreKind.SingleColor] = score;
            }
        }
    }

    private IEnumerator ProcScoreData(Action endCallBack)
    {
        if (scoreSending == false)
        {
            DataManager dataManager = DataManager.Instance;

            long playDateTime = DateTime.Now.ToBinary();

            // スコア更新しているなら送信
            ScoreManager scoreManager = ScoreManager.Instance;
            for (int scoreKind = 0; scoreKind < scoreData.Length; scoreKind++)
            {
                // スコア
                if ((replay == false && colorScore[scoreKind] > scoreManager.highScore[scoreKind]) ||
                    (replay == true && replayScoreKind == scoreKind))
                {
                    scoreData[scoreKind].Score = colorScore[scoreKind];
                    scoreData[scoreKind].PlayDateTime = playDateTime;
                    scoreManager.save(scoreData[scoreKind]);
                }

                // リプレイ
                if (replay == false && colorScore[scoreKind] > scoreManager.highScore[scoreKind])
                {
                    replayData[scoreKind].PlayDateTime = playDateTime;
                    replayData[scoreKind].FrameCount = frame;
                    replayData[scoreKind].InputCount = inputType.Count;
                    replayData[scoreKind].InputFrame = inputFrame.ToArray();
                    replayData[scoreKind].InputType = inputType.ToArray();
                    replayData[scoreKind].InputData1 = inputData1.ToArray();
                    replayData[scoreKind].InputData2 = inputData2.ToArray();
                    /*
                    Debug.Log("FrameCount:" + replayData[scoreKind].FrameCount + " InputCount:" + replayData[scoreKind].InputCount);
                    for (int i = 0; i < replayData.InputType.Length; i++)
                    {
                        Debug.Log("i:" + i + " InputFrame:" + replayData[scoreKind].InputFrame[i] + " InputType:" + replayData[scoreKind].InputType[i] + " InputData1:" + replayData[scoreKind].InputData1[i] + " InputData2:" + replayData[scoreKind].InputData2[i]);
                    }
                     */
                    scoreManager.saveReplay(replayData[scoreKind]);

                }
            }
        }

        scoreSending = true;
        yield return new WaitForSeconds(1);
        endCallBack();
    }

    public void SavePractice()
    {
        board.SavePractice();
    }

    public void LoadPractice()
    {
        board.LoadPractice();
    }
}
