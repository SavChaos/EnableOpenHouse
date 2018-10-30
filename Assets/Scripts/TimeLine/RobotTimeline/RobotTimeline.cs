﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using DG.Tweening;

public class RobotTimeline : SerializedMonoBehaviour
{
    [System.Serializable]
    public class RobotTimePeriod
    {
        public string bodyText;
        public Sprite panelImage;
        public AudioClip textAudio;
    }

    public Dictionary<TextMeshProUGUI, RobotTimePeriod> dic = new Dictionary<TextMeshProUGUI, RobotTimePeriod>();
    public GameObject handle;
    public TextMeshProUGUI panelDateText;
    public RobotTimelinePanel mainTimelineArea;
    [SerializeField] private RobotTimelinePanel startPanel;
    public GameObject defaultTextBox;
    public GameObject timelineTextBox;
    public AudioClip defaultAudioClip;

    private List<TextMeshProUGUI> textMeshes = new List<TextMeshProUGUI>();

    private void Awake()
    {
        for (int i = 0; i < handle.transform.childCount; i++)
        {
            textMeshes.Add(handle.transform.GetChild(i).GetComponent<TextMeshProUGUI>());
        }
    }

    private void Start()
    {
        if (startPanel)
        {
            startPanel.gameObject.SetActive(true);
        }
        else
        {
            mainTimelineArea.gameObject.SetActive(false);
            SetPanel(0);
        }
    }


    public void SetPanel(int i)
    {
        if (i < textMeshes.Count)
        {
            SetPanel(textMeshes[i], i);
        }
    }

    public void SetPanel(TextMeshProUGUI textMesh, int i)
    {
        //Use the default layout if there is no textMesh there
        if (textMesh == null)
        {
            SetLayoutDefault();
            return;
        }

        SetLayoutTimeline();

        mainTimelineArea.DOKill();

        panelDateText.text = textMesh.text;
        mainTimelineArea.DateText.text = textMesh.text;
        mainTimelineArea.canvasGroup.DOFade(0, 0.5f).OnComplete(()=>
        {
            mainTimelineArea.MainText.text = dic[textMesh].bodyText;
            mainTimelineArea.LeftImage.sprite = dic[textMesh].panelImage;
            mainTimelineArea.canvasGroup.DOFade(1, 0.5f);
        });

        //AudioManager.Instance.PlayClip(dic[textMesh].textAudio);

        SetPanelOrientation(i);
    }

    private void SetLayoutDefault()
    {
        // Show default start panel, if there is one
        if (startPanel)
        {
            startPanel.gameObject.SetActive(true);
        }

        mainTimelineArea.gameObject.SetActive(false);
        timelineTextBox.SetActive(false);
        defaultTextBox.SetActive(true);
    }

    private void SetLayoutTimeline()
    {
        mainTimelineArea.gameObject.SetActive(true);
        timelineTextBox.SetActive(true);
        defaultTextBox.SetActive(false);

        if (startPanel)
        {
            startPanel.gameObject.SetActive(false);
        }
        else
        {
            mainTimelineArea.canvasGroup.alpha = 0;
        }
    }

    private void SetPanelOrientation(int i)
    {
        //Sets orientation if Panel is odd or even in list
        if (i % 2 == 0)
        {
           /* mainTimelineArea.LeftImage.rectTransform.anchorMin = new Vector2(1f, 0.5f);
            mainTimelineArea.LeftImage.rectTransform.anchorMax = new Vector2(1f, 0.5f);
            mainTimelineArea.LeftImage.rectTransform.pivot = new Vector2(1f, 0.5f);
            mainTimelineArea.MainText.rectTransform.localPosition = new Vector2(-225f, -6f);*/
        }
        else
        {
          /*  mainTimelineArea.LeftImage.rectTransform.anchorMin = new Vector2(0f, 0.5f);
            mainTimelineArea.LeftImage.rectTransform.anchorMax = new Vector2(0f, 0.5f);
            mainTimelineArea.LeftImage.rectTransform.pivot = new Vector2(0f, 0.5f);
            mainTimelineArea.MainText.rectTransform.localPosition = new Vector2(220f, -6f);*/
        }
    }
}
