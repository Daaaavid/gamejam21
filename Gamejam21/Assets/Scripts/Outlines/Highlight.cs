using System;
using System.Collections;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    public HighlightBus[] IsActivatedBy;
    public GameObject target;
    private void OnEnable()
    {
        foreach (var highlightBus in IsActivatedBy)
        {
            highlightBus.OnActivate.AddListener(ActivateOutline);
            highlightBus.OnDeactivate.AddListener(
                (ht) =>
                {
                    StopAllCoroutines();
                    DeactivateOutline(ht);
                });
        }
    }

    private void ActivateOutline(HighlightTrigger t)
    {
        target.SetActive(true);
        if (t.EnabledDuration > 0) StartCoroutine(WaitForDeactivation(t));
    }

    IEnumerator WaitForDeactivation(HighlightTrigger t)
    {
        yield return new WaitForSeconds(t.EnabledDuration);
        DeactivateOutline(t);
    }

    private void DeactivateOutline(HighlightTrigger t)
    {
        target.SetActive(false);
    }
}