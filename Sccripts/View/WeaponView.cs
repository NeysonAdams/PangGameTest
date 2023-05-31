using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponView : GameObjectView
{
    public WeaponModel Model
    {
        set
        {
            model = value;
            OnStart += (weapon) =>
            {
                rigidbody.velocity = new Vector2(0, value.strike_power);
            };

            OnCollision += (weapon, coliized) =>
            {
                if (coliized.name.Equals("Cilling"))
                {
                    Destroy(weapon.gameObject);
                }

            };
        }

        get => model as WeaponModel;
    }
}
