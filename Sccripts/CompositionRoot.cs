using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositionRoot : MonoBehaviour
{

    [Header("Prefabs")]
    [SerializeField] private BallView ball_prefab;
    [SerializeField] private PlayerView player_prefab;
    [SerializeField] private WeaponView weapon_prefab;
    [SerializeField] private ScoreTabView score_tab_prefab;

    [Header("UIViews")]
    [SerializeField] private GameUIView game_ui_view;
    [SerializeField] private GamePlayMobileUIView game_play_mobile_ui;

    [Header("Containers")]
    [SerializeField] private Transform ball_container;
    [SerializeField] private Transform player_container;

    [Header("Borders")]
    [SerializeField] private List<GameObject> borders;




    private LevelCreatecontoller levelCreateController;
    private GameplayController gamePlayController;
    private GameUiController gameUIcontroller;

    // Start is called before the first frame update
    void Awake()
    {
        levelCreateController = new LevelCreatecontoller();
        gameUIcontroller = new GameUiController(game_ui_view, levelCreateController);
        gamePlayController = new GameplayController(ball_prefab, player_prefab, weapon_prefab, score_tab_prefab, levelCreateController, gameUIcontroller, game_play_mobile_ui, ball_container, player_container, borders);
    }
}
