using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderBoardLine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI id_label;
    [SerializeField] private TextMeshProUGUI player_name_label;
    [SerializeField] private TextMeshProUGUI score_label;

    public void Set(string id, string name, string score)
    {
        id_label.text = id;
        player_name_label.text = name;
        score_label.text = score;
    }
}
