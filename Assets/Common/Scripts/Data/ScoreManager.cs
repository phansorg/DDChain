using NCMB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public struct FetchData
    {
        public bool flag;
        public List<ScoreDataV1> scoreDataList;
    }

    public struct FetchReplayData
    {
        public bool flag;
        public ReplayDataV1 replayData;
    }

    public FetchData[] fetchData;
    public int version;

    public int[] highScore;

    public FetchReplayData fetchReplayData;

    public static ScoreManager Instance
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

        fetchData = new FetchData[ScoreDataV1.SCORE_KIND_MAX];
        for (int scoreKind = 0; scoreKind < ScoreDataV1.SCORE_KIND_MAX; scoreKind++)
        {
            fetchData[scoreKind].flag = false;
            fetchData[scoreKind].scoreDataList = new List<ScoreDataV1>();
        }

        highScore = new int[ScoreDataV1.SCORE_KIND_MAX];
    }

    // ============================================================
    // Score
    // ============================================================
    // サーバーからトップ10を取得 ---------------    
    public void fetchTopRankers(int scoreKind, ScoreDataV1 param, int queryLimit)
    {
        fetchData[scoreKind].flag = false;

        version = param.Version;

        // データストアの「ScoreDataV1」クラスから検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("ScoreDataV1");

        query.WhereEqualTo("ScoreKindValue", param.ScoreKindValue);
        //query.WhereEqualTo("Id", param.Id);
        query.WhereEqualTo("Row", param.Row);
        query.WhereEqualTo("Col", param.Col);
        query.WhereEqualTo("Color", param.Color);
        query.WhereEqualTo("Link", param.Link);
        query.WhereEqualTo("Direction", param.Direction);
        query.WhereEqualTo("Time", param.Time);
        query.WhereEqualTo("Stop", param.Stop);
        query.WhereEqualTo("CountDisp", param.CountDisp);
        query.WhereEqualTo("Garbage", param.Garbage);
        if (param.Version != 0)
        {
//            query.WhereEqualTo("Version", param.Version);
            query.WhereGreaterThan("Version", 1);
        }

        query.OrderByDescending("Score");
        query.Limit = queryLimit;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {

            if (e != null)
            {
                //検索失敗時の処理
            }
            else
            {
                //検索成功時の処理
                fetchData[scoreKind].scoreDataList.Clear();
                // 取得したレコードをScoreDataV1クラスとして保存
                foreach (NCMBObject obj in objList)
                {
                    ScoreDataV1 data = new ScoreDataV1();
                    data.Score = System.Convert.ToInt32(obj["Score"]);
                    data.Name = System.Convert.ToString(obj["Name"]);
                    if (version != 0)
                    {
                        data.Version = System.Convert.ToInt32(obj["Version"]);
                        data.Id = System.Convert.ToString(obj["Id"]);
                        data.PlayDateTime = System.Convert.ToInt64(obj["PlayDateTime"]);
                        data.ScoreKindValue = System.Convert.ToInt32(obj["ScoreKindValue"]);

                        data.Row = System.Convert.ToInt32(obj["Row"]);
                        data.Col = System.Convert.ToInt32(obj["Col"]);
                        data.Color = System.Convert.ToInt32(obj["Color"]);
                        data.Link = System.Convert.ToInt32(obj["Link"]);
                        data.Direction = System.Convert.ToInt32(obj["Direction"]);
                        data.Time = System.Convert.ToInt32(obj["Time"]);
                        data.Stop = System.Convert.ToInt32(obj["Stop"]);
                        data.CountDisp = System.Convert.ToInt32(obj["CountDisp"]);
                        data.Garbage = System.Convert.ToInt32(obj["Garbage"]);

                    }
                    fetchData[scoreKind].scoreDataList.Add(data);
                }
                for (int idx = objList.Count; idx < queryLimit; idx++)
                {
                    ScoreDataV1 data = new ScoreDataV1();
                    data.Score = 0;
                    data.Name = "----------";
                    fetchData[scoreKind].scoreDataList.Add(data);
                }
                fetchData[scoreKind].flag = true;
            }
        });
    }

    // サーバーにハイスコアを保存 -------------------------
    public void save(ScoreDataV1 param)
    {
        // データストアの「ScoreDataV1」クラスから、Nameをキーにして検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("ScoreDataV1");
        query.WhereEqualTo("ScoreKindValue", param.ScoreKindValue);
        query.WhereEqualTo("Id", param.Id);
        query.WhereEqualTo("Row", param.Row);
        query.WhereEqualTo("Col", param.Col);
        query.WhereEqualTo("Color", param.Color);
        query.WhereEqualTo("Link", param.Link);
        query.WhereEqualTo("Direction", param.Direction);
        query.WhereEqualTo("Time", param.Time);
        query.WhereEqualTo("Stop", param.Stop);
        query.WhereEqualTo("CountDisp", param.CountDisp);
        query.WhereEqualTo("Garbage", param.Garbage);
        query.WhereGreaterThan("Version", 1);

        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {

            //検索成功したら    
            if (e == null)
            {
                // ハイスコアが未登録だったら
                if (objList.Count == 0)
                {
                    NCMBObject obj = new NCMBObject("ScoreDataV1");
                    obj["ScoreKindValue"] = param.ScoreKindValue;
                    obj["Id"] = param.Id;
                    obj["Row"] = param.Row;
                    obj["Col"] = param.Col;
                    obj["Color"] = param.Color;
                    obj["Link"] = param.Link;
                    obj["Direction"] = param.Direction;
                    obj["Time"] = param.Time;
                    obj["Stop"] = param.Stop;
                    obj["CountDisp"] = param.CountDisp;
                    obj["Garbage"] = param.Garbage;

                    obj["Name"] = param.Name;
                    obj["PlayDateTime"] = param.PlayDateTime;
                    obj["Score"] = param.Score;
                    obj["Version"] = param.Version;
                    obj.SaveAsync();
                }
                // ハイスコアが登録済みだったら
                else
                {
                    objList[0]["Name"] = param.Name;
                    objList[0]["PlayDateTime"] = param.PlayDateTime;
                    objList[0]["Score"] = param.Score;
                    objList[0]["Version"] = param.Version;
                    objList[0].SaveAsync();
                }
            }
        });
    }

    // サーバーからハイスコアを取得  -----------------
    public void getScore(ScoreDataV1 param)
    {
        highScore[param.ScoreKindValue] = 0;

        // データストアの「ScoreDataV1」クラスから、Nameをキーにして検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("ScoreDataV1");
        query.WhereEqualTo("ScoreKindValue", param.ScoreKindValue);
        query.WhereEqualTo("Id", param.Id);
        query.WhereEqualTo("Row", param.Row);
        query.WhereEqualTo("Col", param.Col);
        query.WhereEqualTo("Color", param.Color);
        query.WhereEqualTo("Link", param.Link);
        query.WhereEqualTo("Direction", param.Direction);
        query.WhereEqualTo("Time", param.Time);
        query.WhereEqualTo("Stop", param.Stop);
        query.WhereEqualTo("CountDisp", param.CountDisp);
        query.WhereEqualTo("Garbage", param.Garbage);
        query.WhereGreaterThan("Version", 1);

        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {

            //検索成功したら  
            if (e == null)
            {
                // ハイスコアが未登録だったら
                if (objList.Count == 0)
                {
                    highScore[param.ScoreKindValue] = 0;
                }
                // ハイスコアが登録済みだったら
                else
                {
                    highScore[param.ScoreKindValue] = System.Convert.ToInt32(objList[0]["Score"]);
                }
            }
        });
    }

    // ============================================================
    // Replay
    // ============================================================
    public void fetchReplay(ReplayDataV1 param)
    {
        fetchReplayData.flag = false;

        version = param.Version;

        // データストアの「ReplayDataV1」クラスから検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("ReplayDataV1");

        query.WhereEqualTo("Version", param.Version);
        query.WhereEqualTo("Id", param.Id);
        query.WhereEqualTo("PlayDateTime", param.PlayDateTime);
        query.WhereEqualTo("ScoreKindValue", param.ScoreKindValue);
        query.WhereEqualTo("Row", param.Row);
        query.WhereEqualTo("Col", param.Col);
        query.WhereEqualTo("Color", param.Color);
        query.WhereEqualTo("Link", param.Link);
        query.WhereEqualTo("Direction", param.Direction);
        query.WhereEqualTo("Time", param.Time);
        query.WhereEqualTo("Stop", param.Stop);
        query.WhereEqualTo("CountDisp", param.CountDisp);
        query.WhereEqualTo("Garbage", param.Garbage);

        query.Limit = 1;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {

            if (e != null)
            {
                //検索失敗時の処理
            }
            else
            {
                //検索成功時の処理

                // 取得したレコードをScoreDataV1クラスとして保存
                foreach (NCMBObject obj in objList)
                {
                    ReplayDataV1 data = new ReplayDataV1();
                    if (version != 0)
                    {
                        data.Version = System.Convert.ToInt32(obj["Version"]);
                        data.Id = System.Convert.ToString(obj["Id"]);
                        data.PlayDateTime = System.Convert.ToInt64(obj["PlayDateTime"]);
                        data.ScoreKindValue = System.Convert.ToInt32(obj["ScoreKindValue"]);

                        data.Row = System.Convert.ToInt32(obj["Row"]);
                        data.Col = System.Convert.ToInt32(obj["Col"]);
                        data.Color = System.Convert.ToInt32(obj["Color"]);
                        data.Link = System.Convert.ToInt32(obj["Link"]);
                        data.Direction = System.Convert.ToInt32(obj["Direction"]);
                        data.Time = System.Convert.ToInt32(obj["Time"]);
                        data.Stop = System.Convert.ToInt32(obj["Stop"]);
                        data.CountDisp = System.Convert.ToInt32(obj["CountDisp"]);
                        data.Garbage = System.Convert.ToInt32(obj["Garbage"]);

                        data.Seed = System.Convert.ToInt32(obj["Seed"]);
                        data.FrameCount = System.Convert.ToInt32(obj["FrameCount"]);
                        data.InputCount = System.Convert.ToInt32(obj["InputCount"]);
                        data.InputFrame = new int[data.InputCount];
                        data.InputType = new byte[data.InputCount];
                        data.InputData1 = new byte[data.InputCount];
                        data.InputData2 = new byte[data.InputCount];
                        for (int idx = 0; idx < data.InputCount; idx++)
                        {
                            ArrayList listData;
                            listData = (ArrayList)obj["InputFrame"];
                            data.InputFrame[idx] = System.Convert.ToInt32(listData[idx]);
                            listData = (ArrayList)obj["InputType"];
                            data.InputType[idx] = System.Convert.ToByte(listData[idx]);
                            listData = (ArrayList)obj["InputData1"];
                            data.InputData1[idx] = System.Convert.ToByte(listData[idx]);
                            listData = (ArrayList)obj["InputData2"];
                            data.InputData2[idx] = System.Convert.ToByte(listData[idx]);
                        }
                    }
                    fetchReplayData.replayData = data;
                }
                fetchReplayData.flag = true;
            }
        });
    }

    public void saveReplay(ReplayDataV1 param)
    {
        // データストアの「ReplayData」クラスから、Nameをキーにして検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("ReplayDataV1");
        query.WhereEqualTo("Id", param.Id);
        query.WhereEqualTo("ScoreKindValue", param.ScoreKindValue);
        query.WhereEqualTo("Row", param.Row);
        query.WhereEqualTo("Col", param.Col);
        query.WhereEqualTo("Color", param.Color);
        query.WhereEqualTo("Link", param.Link);
        query.WhereEqualTo("Direction", param.Direction);
        query.WhereEqualTo("Time", param.Time);
        query.WhereEqualTo("Stop", param.Stop);
        query.WhereEqualTo("CountDisp", param.CountDisp);
        query.WhereEqualTo("Garbage", param.Garbage);

        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {

            //検索成功したら    
            if (e == null)
            {
                // リプレイが未登録だったら
                if (objList.Count == 0)
                {
                    NCMBObject obj = new NCMBObject("ReplayDataV1");
                    obj["Version"] = param.Version;
                    obj["Id"] = param.Id;
                    obj["PlayDateTime"] = param.PlayDateTime;
                    obj["ScoreKindValue"] = param.ScoreKindValue;
                    obj["Seed"] = param.Seed;
                    obj["FrameCount"] = param.FrameCount;
                    obj["InputCount"] = param.InputCount;
                    obj["InputFrame"] = param.InputFrame;
                    obj["InputType"] = param.InputType;
                    obj["InputData1"] = param.InputData1;
                    obj["InputData2"] = param.InputData2;

                    obj["Row"] = param.Row;
                    obj["Col"] = param.Col;
                    obj["Color"] = param.Color;
                    obj["Link"] = param.Link;
                    obj["Direction"] = param.Direction;
                    obj["Time"] = param.Time;
                    obj["Stop"] = param.Stop;
                    obj["CountDisp"] = param.CountDisp;
                    obj["Garbage"] = param.Garbage;

                    obj.SaveAsync();
                }
                // リプレイが登録済みだったら
                else
                {
                    objList[0]["PlayDateTime"] = param.PlayDateTime;
                    objList[0]["Seed"] = param.Seed;
                    objList[0]["Version"] = param.Version;
                    objList[0]["FrameCount"] = param.FrameCount;
                    objList[0]["InputCount"] = param.InputCount;
                    objList[0]["InputFrame"] = param.InputFrame;
                    objList[0]["InputType"] = param.InputType;
                    objList[0]["InputData1"] = param.InputData1;
                    objList[0]["InputData2"] = param.InputData2;
                    objList[0].SaveAsync();
                }
            }
        });
    }
}
