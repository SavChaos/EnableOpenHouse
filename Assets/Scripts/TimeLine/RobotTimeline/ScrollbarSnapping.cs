using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ScrollbarSnapping : SerializedMonoBehaviour, IEndDragHandler
{
    public RobotTimeline robotTimeline;
    public ScrollRect scrollRect;
    public AudioClip scrollEffect;
    public Transform handle;
    public Transform nodeGroup;

    // There must be as many snapValues as there are targets
    public List<float> snapValues = new List<float>();

    [SerializeField]
    private int currentTargetPanel;
    [SerializeField]
    private float snapDuration = 0.3f;

    private void Awake()
    {
        scrollRect.onValueChanged.AddListener(OnValueChange);
        foreach (RectTransform child in nodeGroup)
        {
            snapValues.Add(child.localPosition.x + child.rect.width/2f);
        }

        // Set first node of timeline as start position of the timeline bar 
        Vector3 localPos = handle.localPosition;
        handle.localPosition = new Vector3(-snapValues[0], localPos.y, localPos.z);

        RectTransform rectTransform = (RectTransform)transform;
        rectTransform.DOAnchorPosY(0, 0.5f).From();
    }

    private void OnDestroy()
    {
        scrollRect.onValueChanged.RemoveAllListeners();
    }

    // Snaps to the currentSnapFloat when Hnadle is released
    public void OnEndDrag(PointerEventData data)
    {
        SnapToNearestElement();
    }

    public void OnValueChange(Vector2 pos)
    {
        // Declare a new Target Panel 
        int newTargetPanel = 0;

        for (int i = 1; i < snapValues.Count; i++)
        {
            float handleX = -handle.transform.localPosition.x;
            float distBetween = snapValues[i] - snapValues[i - 1];
            float inBetweenPoint = snapValues[i - 1] + distBetween / 2f;

            // If the current scroller position is between two time line nodes
            if (handleX >= snapValues[i - 1] && handleX <= snapValues[i])
            {
                // Decide which node we are closer to
                newTargetPanel = (handleX <= inBetweenPoint) ? i - 1 : i;
                break;
            }
            else
            {
                if (handleX < snapValues[0])
                {
                    newTargetPanel = 0;
                }
                else if (handleX > snapValues[snapValues.Count - 1])
                {
                    newTargetPanel = snapValues.Count - 1;
                }
            }
        }

        // If the target panel has changed
        if (newTargetPanel != currentTargetPanel)
        {
            currentTargetPanel = newTargetPanel;

            // Show the correct time line panel
            robotTimeline.SetPanel(currentTargetPanel);
        }
    }

    // Snaps to float provided
    public void SnapToNearestElement()
    {
        handle.transform.DOKill();  // Kill any existing tween
        handle.transform.DOLocalMoveX(-snapValues[currentTargetPanel], snapDuration);
    }


}
