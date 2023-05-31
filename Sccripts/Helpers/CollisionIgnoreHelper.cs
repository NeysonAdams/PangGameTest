using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Helper Class containcce method wihitch can ignore collisiion between objects whitch we do not need to collise. For exaple balls with other balls.
/// </summary>

public class CollisionIgnoreHelper
{
    /// <summary>
    /// Ignore collision between to View
    /// </summary>
    /// <param name="view1"></param>
    /// <param name="view2"></param>
    public void IgnoreCollision(GameObjectView view1, GameObjectView view2)
    {
        Physics2D.IgnoreCollision(view1.Collider, view2.Collider, true);
    }

    /// <summary>
    /// Ignore collision betweeb viiew ang list of views
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="view1"></param>
    /// <param name="views"></param>
    public void IgnoreCollision<T>(GameObjectView view1, List<T> views)
    {
        for (int i = 0; i < views.Count; i++)
        {
            var v = views[i] as GameObjectView;
            if (v.Collider != null)
                IgnoreCollision(v, view1);
        }
    }

    /// <summary>
    /// Ignore collision betweeb viiew ang list of Gamobjects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="view1"></param>
    /// <param name="views"></param>
    public void IgnoreCollision(GameObjectView view1, List<GameObject> views)
    {
        for (int i = 0; i < views.Count; i++)
        {
            var v = views[i].GetComponent<Collider2D>();
            if (v != null)
                Physics2D.IgnoreCollision(view1.Collider, v, true);
        }
    }

}
