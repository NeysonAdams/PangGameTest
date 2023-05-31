using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class Models
{
}
[Serializable]
public class BallModel: Models
{
    public int id;
    public float bounceForce = 12.5f;
    public float velocity = 5;

    public float weight;
    public float scale;
    public Vector2 startPosition;
    public float startforce;
    public int level = 5;
}

[Serializable]
public class Player: Models
{
    public int id;
    public string name;
    public float score;
    public int extra_lives;
    public float velocity;
    public Vector2 startPosition;
    public float movement_speed = 5;
    public bool is_current = true;
    public int shoot_count = 1;
    public int shoot = 0;
    public float strike_power = 3;
    public bool is_shoot = false;
}
[Serializable]
public class WeaponModel : Models
{
    public int player_id;
    public float strike_power = 3;
}

[Serializable]
public class LevelModel
{
    public int level = 0;
    public List<BallModel> balls = new List<BallModel>();
    public List<Player> players = new List<Player>();
}

[Serializable]
public class LeaderboardLineModel
{
    public string id;
    public string name;
    public float score;
}

[Serializable]
public class LeaderBoardModel
{
    public List<LeaderboardLineModel> lines = new List<LeaderboardLineModel>();
}
