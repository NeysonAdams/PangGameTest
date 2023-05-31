using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreatecontoller
{

    private List<Vector2> coords = new List<Vector2>
    {
        new Vector2(-100, 175),
        new Vector2(100, 175)
    };

    private SaweLoadDataHelper saweLoad = new SaweLoadDataHelper();
    private LevelModel current_level_model;

    public LevelModel CurrentLevel => current_level_model;


    public LevelModel TestBuildLevel()
    {
        return new LevelModel
        {
            balls = BuildBalls(),
            players = BuildPlayers(1)
        };

    }

    public LevelModel LoadLevel(int level)
    {
        LevelModel level_model = saweLoad.LoadLevelInfo(level);
        level_model.level = level;
        if (level <= 0)
            level_model.players = BuildPlayers(2);
        else if (level > 1)
            level_model.players = current_level_model.players;
        else
            level_model.players = BuildPlayers(1);

        current_level_model = level_model;
        return level_model;
    }

    private List<Player> BuildPlayers(int count)
    {
        List<Player> players = new List<Player>();
        int player_count = count;
        float x_coord = (count == 2) ? 100 : 0;
        for (int i = 0; i < player_count; i++)
        {
            int kof = (i == 1) ? -1 : 1;
            players.Add(new Player
            {
                id = i,
                name = "Player "+(i+1).ToString(),
                score = 0,
                extra_lives = 1,
                shoot_count = 1,
                velocity = 0,
                startPosition = new Vector2(kof * x_coord, -195),
                strike_power = 15,
                is_current = (i==1)?false:true
            });
        }
        return players;
    }

    private List<BallModel> BuildBalls()
    {
        List<BallModel> balls = new List<BallModel>(); 
        int ball_ccount = 2;
        for (int i = 0; i < ball_ccount; i++)
        {
            balls.Add(new BallModel {
                id = i,
                bounceForce = 35,
                velocity = 5,
                weight = 3,
                level = 5,
                startforce =0,
                startPosition = coords[i],
                scale = 1
            });
        }
        return balls;
    }

    /// <summary>
    /// Create 2 balls in a place of Destroyed and put them in level model
    /// </summary>
    /// <param name="ball_model">model of destroyed ball</param>
    /// <param name="level_model">current level model</param>
    /// <param name="position">last position of destroyed ball</param>
    public void AddBallsAfterDestroy(BallModel ball_model, LevelModel level_model, Vector2 position)
    {
        if (ball_model.level == 1)
            return;
        int ball_count = 2;
        for (int i = 0; i < ball_count; i++)
        {
            level_model.balls.Add(new BallModel
            {
                id = level_model.balls.Count,
                bounceForce = 35,
                velocity = (i==0)? 5 : -5,
                weight = ball_model.weight + 0.5f,
                level = ball_model.level-1,
                startforce = 20,
                startPosition = position,
                scale = ball_model.scale - 0.2f
            });
        }
    }

}
