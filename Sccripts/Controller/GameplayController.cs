using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllEvent
{
    RIGHT,
    LEFT,
    IDLE,
    SHOOT
}

public class GameplayController
{
    private BallView ball_prefab;
    private Transform ballContainer;

    private PlayerView playerView;
    private Transform playerContiner;

    private WeaponView weapon_prefab;
    private List<GameObject> borders;

    private ScoreTabView score_tab_view;

    private LevelCreatecontoller levelBuilder;
    private GameUiController game_ui_contriller;
    private GamePlayMobileUIView game_play_mobile_ui;

    private List<BallView> balls = new List<BallView>();
    private List<PlayerView> players = new List<PlayerView>();
    private List<WeaponView> weapons = new List<WeaponView>();

    private CollisionIgnoreHelper collision_ignore_helper = new CollisionIgnoreHelper();
    private SaweLoadDataHelper sldh = new SaweLoadDataHelper();
    ControllEvent event_subject = ControllEvent.IDLE;


    public GameplayController(
        BallView _ballPrefab,
        PlayerView _playerView,
        WeaponView _weaponPrefab,
        ScoreTabView _score_tab_view,
        LevelCreatecontoller _levelBuilder,
        GameUiController _game_ui_contriller,
        GamePlayMobileUIView _game_play_mobile_ui,
        Transform _ballContainer,
        Transform _playerCcontiner,
        List<GameObject> _borders)
    {
        ball_prefab = _ballPrefab;
        levelBuilder = _levelBuilder;
        ballContainer = _ballContainer;
        playerView = _playerView;
        playerContiner = _playerCcontiner;
        weapon_prefab = _weaponPrefab;
        borders = _borders;
        score_tab_view = _score_tab_view;
        game_ui_contriller = _game_ui_contriller;
        game_play_mobile_ui = _game_play_mobile_ui;

        Initialize();
    }


    private void Initialize()
    {
        game_ui_contriller.StartGameAction += (is_reset) =>
        {
            ResetLevel();
            if (levelBuilder.CurrentLevel != null)
            {
                int current_level = is_reset ? 1 : levelBuilder.CurrentLevel.level + 1;
                InitializeLevel(current_level);
            }
            else
            {
                InitializeLevel(1);
            }
        };

        game_ui_contriller.StartVersusAction += () =>
        {
            InitializeLevel(0);
        };

        game_ui_contriller.GameRemove += ResetLevel;

        game_play_mobile_ui.RightAction += () =>
        {
            event_subject = ControllEvent.RIGHT;
        };

        game_play_mobile_ui.LeftAction += () =>
        {
            event_subject = ControllEvent.LEFT;
        };


        game_play_mobile_ui.PointerUPAcction += () =>
        {
            event_subject = ControllEvent.IDLE;
        };

        game_play_mobile_ui.ShootAction += () =>
        {
            event_subject = ControllEvent.SHOOT;
        };


    }

    private void ResetLevel()
    {
#if UNITY_ANDROID
        game_play_mobile_ui.gameObject.SetActive(false);
#endif
        for (int i =0; i< players.Count;i++)
        {
            players[i].RemoveAllActions();
            GameObject.Destroy(players[i].gameObject);
        }
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].RemoveAllActions();
            GameObject.Destroy(balls[i].gameObject);
        }

        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].RemoveAllActions();
            GameObject.Destroy(weapons[i].gameObject);
        }

        players.Clear();
        balls.Clear();
        weapons.Clear();
    }


    /// <summary>
    /// Reqursy method adding new balls to stack;
    /// </summary>
    /// <param name="start">the number from which to start adding the ball to the stack of swords </param>
    /// <param name="level_model"> Generated be LevelCreateContoller model of current level</param>

    private void ReqursivityBallsBuild(int start , LevelModel level_model)
    {
        for (int i = start; i < level_model.balls.Count; i++)
        {
            BallView view = GameObject.Instantiate<BallView>(ball_prefab, ballContainer);
            view.Model = level_model.balls[i];
            view.OnDead += (obj) =>
            {
                var ball_view = obj as BallView;
                var ball_model = ball_view.Model as BallModel;

                balls.Remove(ball_view);
                level_model.balls.Remove(ball_model);


                int start = level_model.balls.Count;

                levelBuilder.AddBallsAfterDestroy(ball_model, level_model, ball_view.transform.localPosition);

                

                ReqursivityBallsBuild(start, level_model);

                if (balls.Count == 0)
                {
                    game_ui_contriller.ShowGameOver(true);
                    SoundHolder.Instance.PlayMusik(SoundsMSK.WIN, true);
                    
                }

            };

            view.OnStart += (ball_view) =>
            {
                collision_ignore_helper.IgnoreCollision(ball_view, balls);
            };

            view.OnCollision += (ball_view, collised) =>
            {
                if (collised.name.Equals("Weapon"))
                {

                    GameObject.Destroy(collised);
                    GameObject.Destroy(ball_view.gameObject);

                    var m_ball_view = ball_view as BallView;
                    var weapon_view = collised.GetComponent<WeaponView>();
                    var player_model = level_model.players[weapon_view.Model.player_id];

                    var scr_tab = GameObject.Instantiate<ScoreTabView>(score_tab_view, ball_view.transform.parent);
                    scr_tab.transform.localPosition = ball_view.transform.localPosition;

                    float score = game_ui_contriller.GetBallScore(m_ball_view.Model.level);
                    scr_tab.Set(score);
                    player_model.score += score;
                    game_ui_contriller.AddScoreToUI(player_model.score, score);

                    SoundHolder.Instance.PlaySFx(SoundsSFX.EXPLOSION);
                }
            };

            balls.Add(view);
        }
    }


    private void InitializeLevel(int level)
    {
        LevelModel level_model = levelBuilder.LoadLevel(level);
        ReqursivityBallsBuild(0, level_model);

        SoundHolder.Instance.PlayMusik(SoundsMSK.GAME, true);
#if UNITY_ANDROID
        game_play_mobile_ui.gameObject.SetActive(true);
#endif

        for (int i = 0; i < level_model.players.Count; i++)
        {
            PlayerView view = GameObject.Instantiate<PlayerView>(playerView, playerContiner);
            view.Model = level_model.players[i];

            //Here we initialize a controll
            view.OnUpdate += (player_model) =>
            {
                var p_model = player_model as Player;
                if (p_model == null)
                    return;
#if !UNITY_ANDROID
               event_subject = GetEventSubject(p_model.is_current);
#endif

                switch (event_subject)
                {
                    case ControllEvent.IDLE:
                        p_model.velocity = 0;
                        break;
                    case ControllEvent.LEFT:
                        p_model.velocity = (-p_model.movement_speed);
                        break;
                    case ControllEvent.RIGHT:
                        p_model.velocity = p_model.movement_speed;
                        break;
                    case ControllEvent.SHOOT:
                        if (p_model.shoot  < p_model.shoot_count && !p_model.is_shoot)
                        {
                            p_model.is_shoot = true;
                            SoundHolder.Instance.PlaySFx(SoundsSFX.WEPON_PLAYER);

                        }
                        event_subject = ControllEvent.IDLE;
                        break;
                }
            };


            view.OnShoot += (player_view) =>
            {
                player_view.Model.shoot++;
                player_view.Model.is_shoot = false;
                WeaponView view = GameObject.Instantiate<WeaponView>(weapon_prefab, player_view.transform.parent);
                view.gameObject.name = "Weapon";

                view.Model = new WeaponModel
                {
                    player_id = player_view.Model.id,
                    strike_power = player_view.Model.strike_power
                };
                view.transform.localPosition = player_view.transform.localPosition + new Vector3 (0,10,0);

                view.OnStart += (weapon_view) =>
                {
                    collision_ignore_helper.IgnoreCollision(weapon_view, borders);
                    collision_ignore_helper.IgnoreCollision(weapon_view, players);
                };
                
                view.OnDead += (weapon) =>
                {
                    player_view.Model.shoot--;
                    weapons.Remove(weapon as WeaponView);
                };

                weapons.Add(view);
            };

            view.OnCollision += (player_view, collised) =>
            {
                if (collised.name.Equals("Ball(Clone)"))
                {
                    Debug.Log(level_model.players.Count);
                    if (level_model.players.Count == 1)
                        game_ui_contriller.SetLeaderBoard(level_model.players[0]);
                    game_ui_contriller.ShowGameOver(false);
                    SoundHolder.Instance.PlaySFx(SoundsSFX.EXPLOSION);
                    SoundHolder.Instance.PlayMusik(SoundsMSK.LOSE, false);
                    
                }
            };

            players.Add(view);
        }
    }


    private ControllEvent GetEventSubject(bool is_current = true)
    {
        ControllEvent event_subject = ControllEvent.IDLE;

        if (is_current)
        {
            if (Input.GetKey(KeyCode.RightArrow))
                event_subject = ControllEvent.RIGHT;
            if (Input.GetKey(KeyCode.LeftArrow))
                event_subject = ControllEvent.LEFT;
            if (Input.GetKeyDown(KeyCode.Space))
                event_subject = ControllEvent.SHOOT;
            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
                event_subject = ControllEvent.IDLE;

        }
        else
        {
            if (Input.GetKey(KeyCode.D))
                event_subject = ControllEvent.RIGHT;
            if (Input.GetKey(KeyCode.A))
                event_subject = ControllEvent.LEFT;
            if (Input.GetKeyDown(KeyCode.S))
                event_subject = ControllEvent.SHOOT;
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
                event_subject = ControllEvent.IDLE;
        }


        return event_subject;
    }

}
