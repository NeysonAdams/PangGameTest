using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerView : GameObjectView
{
    [SerializeField]private Animator animator;

    public Action<PlayerView> OnShoot;

    public Player Model
    {
        set
        {
            model = value;
            transform.localPosition = value.startPosition;
            OnUpdate += (model) =>
            {
                var p_model = model as Player;
                if (p_model == null)
                    return;
                rigidbody.velocity = new Vector2(p_model.velocity, 0);
                int tag = 0;
                if (p_model.velocity > 0)
                    tag = 1;
                else if(p_model.velocity < 0)
                    tag = 2;
                animator.SetInteger("stat", tag);

                if(p_model.is_shoot)
                {
                    p_model.is_shoot = false;
                    OnShoot?.Invoke(this);
                }

                
            };
        }

        get => model as Player;
    }

}
