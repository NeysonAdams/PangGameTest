using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;

public class ScoreTabView : GameObjectView
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private CanvasGroup group;
    float delta;

    public void Set(float score)
    {
        label.text = "+"+score.ToString();
        group.alpha = 0;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(PushScoreTabAnimation());
        delta = Time.deltaTime;
    }

    private IEnumerator PushScoreTabAnimation()
    {
        float time = 0;
        float duration = 1;
        Vector3 distance = new Vector3(0,25,0);
        Vector3 startPosition = transform.localPosition - distance;
        Vector3 middlePosition = transform.localPosition;
        Vector3 endPosition = transform.localPosition + distance;

        while (time < 1)
        {
            transform.localPosition = Vector3.Lerp(startPosition, middlePosition, time / duration);
            group.alpha = Mathf.Lerp(0,1, time/duration);
            time += delta;
            yield return null;
        }

        transform.localPosition = middlePosition;
        group.alpha = 1;
        time = 0;

        while (time < 1)
        {
            transform.localPosition = Vector3.Lerp(middlePosition, endPosition, time / duration);
            group.alpha = Mathf.Lerp(1, 0, time / duration);
            time += delta;
            yield return null;
        }

        Destroy(gameObject);

    }
}
