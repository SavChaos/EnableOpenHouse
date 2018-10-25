using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuToggle : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetTrigger(string trigger)
    {
        //AudioManager.Instance.PlayOptions();
        animator.SetTrigger(trigger);
    }
}
