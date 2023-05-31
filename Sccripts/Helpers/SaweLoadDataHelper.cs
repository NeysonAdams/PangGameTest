using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaweLoadDataHelper
{
    private List<string> levels = new List<string>
    {
        "level1",
        "level2",
        "level3",
        "level4",
    };

    private const string llKey = "leaderboard";

    public void SaweToFile<T>(T model)
    {
        string json = JsonUtility.ToJson(model);
        Debug.Log(json);
    }

    public LevelModel LoadLevelInfo(int level)
    {
        if (level <= 0)
            level = 3;
        TextAsset myTextAsset = Resources.Load<TextAsset>(levels[level-1]);

        if (myTextAsset != null)
        {
            //Debug.Log(myTextAsset.text);

            return JsonUtility.FromJson<LevelModel>(myTextAsset.text);
        }
        else
        {
            //Debug.Log("File not found! " + levels[level - 1]);
            return null;
        }
    }

    public void SaweLeaderboard(LeaderBoardModel leaderboard)
    {
        string json = JsonUtility.ToJson(leaderboard);
        PlayerPrefs.SetString(llKey, json);
    }
    public LeaderBoardModel LoadLeaderBoard()
    {
        if (PlayerPrefs.HasKey(llKey))
            return JsonUtility.FromJson<LeaderBoardModel>(PlayerPrefs.GetString(llKey));
        return new LeaderBoardModel();
    }
}
