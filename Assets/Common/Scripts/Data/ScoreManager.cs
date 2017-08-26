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

    public const int RANK_MAX = 10;

    public FetchData[] fetchData;

    public int[] highScore;

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

    // サーバーからトップ10を取得 ---------------    
    public void fetchTopRankers(int scoreKind, ScoreDataV1 param)
    {
        fetchData[scoreKind].flag = false;

        // データストアの「HighScore」クラスから検索
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
        //if (param.Garbage == 0)
        //{
        //    query.WhereNotEqualTo("Garbage", 1);
        //}
        //else
        //{
        //    query.WhereEqualTo("Garbage", param.Garbage);
        //}
        query.WhereEqualTo("Garbage", param.Garbage);

        query.OrderByDescending("Score");
        query.Limit = RANK_MAX;
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
                // 取得したレコードをHighScoreクラスとして保存
                foreach (NCMBObject obj in objList)
                {
                    ScoreDataV1 scoreData = new ScoreDataV1();
                    scoreData.Score = System.Convert.ToInt32(obj["Score"]);
                    scoreData.Name = System.Convert.ToString(obj["Name"]);
                    fetchData[scoreKind].scoreDataList.Add(scoreData);
                }
                for (int idx = objList.Count; idx < RANK_MAX; idx++)
                {
                    ScoreDataV1 scoreData = new ScoreDataV1();
                    scoreData.Score = 0;
                    scoreData.Name = "----------";
                    fetchData[scoreKind].scoreDataList.Add(scoreData);
                }
                fetchData[scoreKind].flag = true;
            }
        });
    }

    // サーバーにハイスコアを保存 -------------------------
    public void save(ScoreDataV1 param)
    {
        // データストアの「HighScore」クラスから、Nameをキーにして検索
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
        /*
        if (param.Garbage == 0)
        {
            query.WhereNotEqualTo("Garbage", 1);
        }
        else
        {
            query.WhereEqualTo("Garbage", param.Garbage);
        }
        */
        query.WhereEqualTo("Garbage", param.Garbage);


        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {

            //検索成功したら    
            if (e == null)
            {
                objList[0]["Name"] = param.Name;
                objList[0]["PlayDateTime"] = param.PlayDateTime;
                objList[0]["Score"] = param.Score;
                objList[0].SaveAsync();
            }
        });
    }

    // サーバーからハイスコアを取得  -----------------
    public void getScore(ScoreDataV1 param)
    {
        highScore[param.ScoreKindValue] = 0;

        // データストアの「HighScore」クラスから、Nameをキーにして検索
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
        /*
        if (param.Garbage == 0)
        {
            query.WhereNotEqualTo("Garbage", 1);
        }
        else
        {
            query.WhereEqualTo("Garbage", param.Garbage);
        }
        */
        query.WhereEqualTo("Garbage", param.Garbage);

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
                    obj["PlayDateTime"] = 0;
                    obj["Score"] = 0;
                    obj.SaveAsync();
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
}
