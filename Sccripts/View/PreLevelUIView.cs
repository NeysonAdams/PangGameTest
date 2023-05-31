using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PreLevelUIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI level_label;
    [SerializeField] private TextMeshProUGUI animated_label;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private CanvasGroup animated_group;
    [SerializeField] private Animator animator;
    float delta;

    // Start is called before the first frame update
    void Start()
    {
        group.alpha = 0;
        animated_group.alpha = 0;
        delta = 0.002f;
    }

    public void SetLevel(int level)
    {
        if (level > 0)
            level_label.text = "Level " + level.ToString();
        else
            level_label.text = "Versus Mode";
    }

    public void PreLevelAnimation(Action after)
    {
        StartCoroutine(PreevelSubroutine(after));
    }

    IEnumerator PreevelSubroutine(Action after)
    {
        float time = 0;
        float duration = .5f;
        int step = 4;

        while(time < 0.5)
        {
            time += delta;
            group.alpha = Mathf.Lerp(0, 1, time / duration);
            
            yield return null;
        }
        group.alpha = 1;

        while (step > 0)
        {
            time = 0;
            animated_label.text = (step == 1) ? "GO" : (step - 1).ToString();
            animated_group.alpha = 0;
            animated_label.transform.localScale = new Vector3(2, 2, 2);

            while (time < 0.5)
            {
                time += delta;
                animated_group.alpha = Mathf.Lerp(0, 1, time / duration);
                animated_label.transform.localScale = Vector3.Lerp(animated_label.transform.localScale, Vector3.one, time / duration);
                yield return null;
            }

            step--;
            time = 0;
            while (time < 0.5)
            {
                time += delta;
                yield return null;
            }
        }

        after?.Invoke();
    }

}
