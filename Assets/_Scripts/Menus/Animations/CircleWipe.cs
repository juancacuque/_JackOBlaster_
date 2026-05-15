using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CircleWipe : SceneTransition
{
    public Image square;

    public override IEnumerator AnimateTransitionIn()
    {
        square.rectTransform.anchoredPosition = new Vector2(-1000f, 0f);
        var tweener = square.rectTransform.DOAnchorPosX(0f, 1f);
        yield return tweener.WaitForCompletion();
    }

    public override IEnumerator AnimateTransitionOut()
    {
        var tweener = square.rectTransform.DOAnchorPosX(1000f, 1f);
        yield return tweener.WaitForCompletion();
    }
}
