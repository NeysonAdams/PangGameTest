using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LeaderBoardUIView : MonoBehaviour
{
    [SerializeField] List<LeaderBoardLine> vlines = new List<LeaderBoardLine>();
    [SerializeField] TMP_InputField name_label;

    public Action<string> SetNameAction;

    // Start is called before the first frame update
    void Start()
    {
        name_label.onValueChanged.AddListener((value) =>
        {
            SetNameAction?.Invoke(value);
        });

    }

    public void SetLeaderboard(LeaderBoardModel model)
    {
        Debug.Log(model.lines.Count);
        for (int i = 0; i < model.lines.Count; i++)
        {
            vlines[i].gameObject.SetActive(true);
            vlines[i].Set((i+1).ToString(), model.lines[i].name, model.lines[i].score.ToString());
        }

    }

    public void AddName()
    {
        name_label.gameObject.SetActive(true);
    }
}
