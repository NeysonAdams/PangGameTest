using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameUIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score_label;
    [SerializeField] private PreLevelUIView preLevel_animation;

    [Header("GameOver")]
    [SerializeField] private GameObject game_over_view;
    [SerializeField] private TextMeshProUGUI game_over_label;
    [SerializeField] private Button replay_button;
    [SerializeField] private Button next_lvl_button;
    [SerializeField] private Button go_to_menu_button;

    [Header("MainMenu")]
    [SerializeField] private GameObject main_menu_view;
    [SerializeField] private Button arcade_button;
    [SerializeField] private Button versus_button;
    [SerializeField] private Button leaderboard_button;

    [Header("SoundCControll")]
    [SerializeField] private Slider musik_slider;
    [SerializeField] private Slider sfx_slider;

    [Header("Leaderboard")]
    [SerializeField] private LeaderBoardUIView leaderboard_view;




    public Action ReplayLevelAction;
    public Action NextLevelAction;
    public Action GoToMenuAction;
    public Action ArcadeGameAction;
    public Action VersusGameAction;
    public Action LeaderBoard;
    public Action<string> SetNameAction;

    private void Start()
    {
#if UNITY_ANDROID
        versus_button.gameObject.SetActive(false);
#endif
        replay_button.onClick.AddListener(() =>
        {
            ReplayLevelAction?.Invoke();
        });

        next_lvl_button.onClick.AddListener(() =>
        {
            NextLevelAction?.Invoke();
        });

        go_to_menu_button.onClick.AddListener(() =>
        {
            main_menu_view.SetActive(true);
            game_over_view.SetActive(false);
            GoToMenuAction?.Invoke();
        });

        arcade_button.onClick.AddListener(() =>
        {
            main_menu_view.SetActive(false);
            ArcadeGameAction?.Invoke();
        });

        versus_button.onClick.AddListener(() =>
        {
            main_menu_view.SetActive(false);
            VersusGameAction?.Invoke();
        });

        leaderboard_button.onClick.AddListener(() =>
        {
            main_menu_view.SetActive(true);
            LeaderBoard?.Invoke();
        });

        musik_slider.onValueChanged.AddListener((value) =>
        {
            SoundHolder.Instance.ChangeMusikVolume(value);
        });
        sfx_slider.onValueChanged.AddListener((value) =>
        {
            SoundHolder.Instance.ChangeSFXVolume(value);
        });

        leaderboard_view.SetNameAction += (value) =>
        {
            SetNameAction?.Invoke(value);
        };
    }


    public void SetLeaderBoard(LeaderBoardModel leaderbord_model)
    {
        Debug.Log("SetLeaderBoard");
        leaderboard_view.SetLeaderboard(leaderbord_model);
    }

    public void AddLeaderBoardName()
    {
        leaderboard_view.AddName();
    }

    public void ShowMainMenu()
    {
        main_menu_view.SetActive(true);
    }
    public void HideMainMenu()
    {
        main_menu_view.SetActive(false);
    }

    /// <summary>
    /// Show Game Over screen
    /// </summary>
    /// <param name="is_win"> true if player win, false if player lose</param>
    public void ShowGameOver(bool is_win)
    {
        game_over_view.SetActive(true);
        game_over_label.text = (is_win) ? "You Win" : " You Lose";
        next_lvl_button.gameObject.SetActive(is_win);
        replay_button.gameObject.SetActive(!is_win);
    }

    public void HideGameOver()
    {
        game_over_view.SetActive(false);
    }
    /// <summary>
    /// Add score and show adding sccore animation in ui
    /// </summary>
    /// <param name="full_score"> how many score Player earn </param>
    /// <param name="score">how many sccore need to add</param>
    public void AddScore(float full_score, float score)
    {
        float current_score = full_score - score;
        StartCoroutine(IncreaceSccore(current_score, full_score));

    }
    IEnumerator IncreaceSccore(float cucurrent_score, float full_sccore)
    {
        float time = 0;
        float duration = 0.5f;
        while(time< duration)
        {
            cucurrent_score = Mathf.RoundToInt(Mathf.Lerp(cucurrent_score, full_sccore, time / duration));
            score_label.text = cucurrent_score.ToString();
            time += Time.deltaTime;
            yield return null;
        }

        cucurrent_score = full_sccore;
        score_label.text = cucurrent_score.ToString();
    }

    public void PushStartGameAnimation(Action after)
    {
        preLevel_animation.gameObject.SetActive(true);
        preLevel_animation.PreLevelAnimation(() =>
        {
            preLevel_animation.gameObject.SetActive(false);
            after?.Invoke();
        });
    }

    public void SetLevel(int level)
    {
        preLevel_animation.SetLevel(level);
    }
}
