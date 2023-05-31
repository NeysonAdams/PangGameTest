using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallView : GameObjectView
{
    [SerializeField] List<Color> colors = new List<Color>();
    [SerializeField] Material material;
    [SerializeField] ExplosionView explosion;

    public BallModel Model
    {
        set
        {
            model = value;

            transform.localScale = new Vector3(value.scale, value.scale, value.scale);
            rigidbody.mass = value.weight;

            Material ball_material = new Material(material.shader);
            ball_material.CopyPropertiesFromMaterial(material);
            ball_material.color = colors[value.level];
            gameObject.GetComponent<Image>().material = ball_material;

            transform.localPosition = value.startPosition;

            OnCollision += (ball, collised) =>
            {
                if (collised.name.Equals("Ground"))
                {
                    this.rigidbody.AddForce(new Vector2(0, value.bounceForce), ForceMode2D.Impulse);
                    SoundHolder.Instance.PlaySFx(SoundsSFX.JUMP);

                }
            };
            OnStart += (ball) =>
            {
                if (value.startforce > 0)
                {
                    this.rigidbody.AddForce(new Vector2(0, value.startforce), ForceMode2D.Impulse);
                }
                this.rigidbody.velocity = new Vector2(value.velocity, rigidbody.velocity.y);

            };

            OnDead += (m) =>
            {
                var expl = Instantiate<ExplosionView>(explosion, transform.parent);
                expl.transform.localPosition = transform.localPosition;
            };
        }
        get => model as BallModel;


    }


}
