using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollbarSnapping : SerializedMonoBehaviour, IEndDragHandler
{
    public RobotTimeline robotTimeline;
    public Scrollbar scrollbar;
    public AudioClip scrollEffect;
    //There must be as many snapValues as there are targets
    public List<float> snapValues = new List<float>();

    private float currentSnapValue;
    private int currentTargetPanel;

    private void OnEnable()
    {
        currentSnapValue = snapValues[0];
        SnapTo(currentSnapValue);
    }

    //Snaps to the currentSnapFloat when Hnadle is released
    public void OnEndDrag(PointerEventData data)
    {
        SnapTo(currentSnapValue);
    }

    public void OnValueChange()
    {
        //declare a new Target Panel 
        int newTargetPanel = 0;

        //Reset the currentSnapFloat to the first SnapValue
        currentSnapValue = snapValues[0];

        for (int i = 1; i < snapValues.Count; i++)
        {
            //If the difference of the current snapValue is greater than the one it is being compared to:
            if (Mathf.Abs(scrollbar.value - currentSnapValue) > Mathf.Abs(scrollbar.value - snapValues[i]))
            {
                //That snapValue becomes the current snapValue
                currentSnapValue = snapValues[i];

                //Id of snapValue stored to acess corresponding targetPanel
                newTargetPanel = i;
            }
        }

        //If the target panel has changed
        if (newTargetPanel != currentTargetPanel)
        {
            //Play Sound Effect
            //AudioManager.Instance.PlaySoundEffect(scrollEffect);
        }

        //Set the new targetPanel as the currentTargetPanel
        currentTargetPanel = newTargetPanel;

        //Send the timeline the currentTargetPanel
        robotTimeline.SetPanel(currentTargetPanel);
    }

    //Snaps to float provided
    public void SnapTo(float f)
    {
        scrollbar.value = f;
    }

    //Snaps to snapValue at the Tranform's sibling index
    public void SnapTo(Transform t)
    {
        scrollbar.value = snapValues[t.GetSiblingIndex()];
    }

}
