using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GamePlayMobileUIView : MonoBehaviour
{
    [SerializeField] private Button shoot_button;

    public Action ShootAction;
    public Action RightAction;
    public Action LeftAction;
    public Action PointerUPAcction;

    // Start is called before the first frame update
    void Start()
    {
        shoot_button.onClick.AddListener(() =>
        {
            ShootAction?.Invoke();
        });
    }

    public void RightButtonPointerDown()
    {
        RightAction?.Invoke();
    }

    public void LeftButtonPointerDown()
    {
        LeftAction?.Invoke();
    }

    public void PointerUP ()
    {
        PointerUPAcction?.Invoke();
    }
}
