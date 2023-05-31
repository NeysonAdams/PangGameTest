using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SocialPlatforms.Impl;

public class GameUiController
{
    private GameUIView game_ui_view;
    LevelCreatecontoller levelCreatecontoller;

    private List<float> score_line = new List<float>
    {
        350, 300, 250, 200, 150, 100, 50
    };

    private SaweLoadDataHelper saweload = new SaweLoadDataHelper();

    public Action<bool> StartGameAction;
    public Action StartVersusAction;
    public Action GameRemove;

    public GameUiController(GameUIView _game_ui_view, LevelCreatecontoller _levelCreatecontoller)
    {
        game_ui_view = _game_ui_view;
        levelCreatecontoller = _levelCreatecontoller;
        Initialize();
    }

    /// <summary>
    /// Return count of score depends forn wchtch size of Ball get Blowd
    /// </summary>
    /// <param name="ball_level"> Ball level </param>
    /// <returns></returns>
    public float GetBallScore(int ball_level)
    {
        return score_line[ball_level];
    }
    /// <summary>
    /// Here we add erned score to UI
    /// </summary>
    /// <param name="full_score">How many score User erned</param>
    /// <param name="score">Sore need to be add</param>
    public void AddScoreToUI(float full_score, float score)
    {
        game_ui_view.AddScore(full_score, score);
    }
    /// <summary>
    /// Show "You Lose" or "You win" sccreen if player lose or win
    /// </summary>
    /// <param name="is_win">true if win, false if lose</param>
    public void ShowGameOver(bool is_win)
    {
        Time.timeScale = 0f;
        game_ui_view.ShowGameOver(is_win);

    }

    /// <summary>
    /// Set Leaderboard and inset Player data into Leaderboard if need
    /// </summary>
    /// <param name="player_model">Player data</param>
    public void SetLeaderBoard(Player player_model)
    {
        if (player_model == null)
            return;
        int need_to_change = -1;
        LeaderBoardModel lb_model =  saweload.LoadLeaderBoard();
        lb_model.lines.Add(new LeaderboardLineModel
        {
            id = "",
            name = player_model.name,
            score = player_model.score
        });

        lb_model.lines.Sort((a, b) => b.score.CompareTo(a.score));

        if (lb_model.lines.Count >3)
        {
            lb_model.lines.RemoveAt(3);
        }

        for (int i =0; i<lb_model.lines.Count; i++)
        {
            if (player_model.score == lb_model.lines[i].score)
            {
                need_to_change = i;
                break;
            }
        }

        game_ui_view.SetLeaderBoard(lb_model);
        if(need_to_change != -1)
        {
            game_ui_view.AddLeaderBoardName();
            game_ui_view.SetNameAction = null;
            game_ui_view.SetNameAction += (value) =>
            {
                lb_model.lines[need_to_change].name = value;
                game_ui_view.SetLeaderBoard(lb_model);
                saweload.SaweLeaderboard(lb_model);
            };
        }

        saweload.SaweLeaderboard(lb_model);
    }


    private void Initialize()
    {
        //Prelevel();

        game_ui_view.ReplayLevelAction += () =>
        {
            
            game_ui_view.HideGameOver();
            StartGameAction?.Invoke(true);
            Prelevel();
        };

        game_ui_view.NextLevelAction += () =>
        {
            
            game_ui_view.HideGameOver();
            StartGameAction?.Invoke(false);
            Prelevel();
        };

        game_ui_view.GoToMenuAction += () =>
        {
            GameRemove?.Invoke();
        };

        game_ui_view.ArcadeGameAction += () =>
        {
            
            game_ui_view.HideMainMenu();
            StartGameAction?.Invoke(true);
            Prelevel();
        };

        game_ui_view.VersusGameAction += () =>
        {

            game_ui_view.HideMainMenu();
            StartVersusAction?.Invoke();
            Prelevel();
        };

    }

    private void Prelevel()
    {
        Time.timeScale = 0f;
        game_ui_view.SetLevel(levelCreatecontoller.CurrentLevel.level);
        game_ui_view.PushStartGameAnimation(() =>
        {
            Time.timeScale = 1f;
        });
    }
}
