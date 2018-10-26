using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class RobotTimelinePanel : MonoBehaviour
{
    [Required]
    public TextMeshProUGUI MainText;
    [Required]
    public TextMeshProUGUI DateText;
    public Image LeftImage;
    public CanvasGroup canvasGroup;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
}
