using NCMB;
using System.Collections.Generic;

public class LeaderBoard
{
    public const int RANK_MAX = 10;

    public bool fetchFlag = false;
    public List<ScoreDataV1> scoreDataList = new List<ScoreDataV1>();

    // サーバーからトップ10を取得 ---------------    
    public void fetchTopRankers(ScoreDataV1 param)
    {
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
                scoreDataList.Clear();
                // 取得したレコードをHighScoreクラスとして保存
                foreach (NCMBObject obj in objList)
                {
                    ScoreDataV1 scoreData = new ScoreDataV1();
                    scoreData.Score = System.Convert.ToInt32(obj["Score"]);
                    scoreData.Name = System.Convert.ToString(obj["Name"]);
                    scoreDataList.Add(scoreData);
                }
                for (int idx = objList.Count; idx < RANK_MAX; idx++)
                {
                    ScoreDataV1 scoreData = new ScoreDataV1();
                    scoreData.Score = 0;
                    scoreData.Name = "----------";
                    scoreDataList.Add(scoreData);
                }
                fetchFlag = true;
            }
        });
    }

}
