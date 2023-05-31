using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionView : GameObjectView
{
    protected override void Start()
    {
        base.Start();
        StartCoroutine("WaytBeforeDestroy");
    }

    private IEnumerator WaytBeforeDestroy()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
