
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RobotTimelinePanel : MonoBehaviour
{
    public TextMeshProUGUI MainText;
    public TextMeshProUGUI DateText;
    public Image LeftImage;
    public CanvasGroup canvasGroup;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
}
