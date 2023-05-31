using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class GameObjectView : MonoBehaviour
{
    [SerializeField]private Collider2D collider;
    [SerializeField] protected Rigidbody2D rigidbody;
    protected string name;
    protected Models model;

    public Action<Models> OnUpdate; 
    public Action<GameObjectView> OnStart;
    public Action<GameObjectView> OnDead; 
    public Action<GameObjectView, GameObject> OnCollision;
    public Action<GameObjectView> OnAwake;

    public Collider2D Collider => collider;

    protected virtual void Awake()
    {
        name = gameObject.name;
        OnAwake?.Invoke(this);
    }

    protected virtual void Start()
    {
        OnStart?.Invoke(this);
    }

    public void Update()
    {
        OnUpdate?.Invoke(model);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject view = collision.gameObject;
        OnCollision?.Invoke(this, view);
    }

    public void RemoveAllActions()
    {
        OnUpdate = null;
        OnDead = null;
        OnStart = null;
        OnCollision = null;
    }

    private void OnDestroy()
    {
        OnDead?.Invoke(this);
        RemoveAllActions();
    }
}
