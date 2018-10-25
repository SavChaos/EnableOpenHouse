using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Page : MonoBehaviour
{
    public bool narrated = false;
    public TextMeshProUGUI[] tM;
    public AudioClip[] audioClips;
    public float clipDelay;
    public bool hasTextChanged;
    public bool narrating = false;

    private float timeSinceStart = 0;
    private bool visited = false;

    private void OnEnable()
    {
        // Subscribe to event fired when text object has been regenerated.
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
    }

    private void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
        narrating = false;
    }

    public void Load()
    {
        //StartCoroutine(LoadAudio());
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        visited = true;
        Book.Instance.nextButton.gameObject.SetActive(false);

        timeSinceStart = 0;
        gameObject.SetActive(true);

        Load();

        if (Book.Instance.IsFirstPage(this))
        {
            Book.Instance.PreviousButton(false);
            Book.Instance.OptionsPanel(true);
        }
        else
        {
            Book.Instance.NextButton();
            Book.Instance.PreviousButton(true);
            Book.Instance.OptionsPanel(false);
        }

        if (Book.Instance.IsLastPage(this))
        {
            Book.Instance.SetNextButton(false);
            Book.Instance.OptionsPanel(true);
        }
    }

    /*private IEnumerator LoadAudio()
    {
        if (narrated)
        {
            //Playing only the first clip for now
            for (int i = 0; i < audioClips.Length; i++)
            {
                AudioManager.Instance.PlayClip(audioClips[i]);

                if (i == 0)
                {
                    AudioManager.Instance.PlayPageTurn();
                }

                yield return new WaitForSeconds(audioClips[i].length + clipDelay);
            }
        }
    }*/

    private IEnumerator FadeTextIn() // Not used currently
    {
        if (tM.Length > 0)
        {
            narrating = true;
            TMP_Text textComponent = tM[0].GetComponent<TMP_Text>();
            textComponent.ForceMeshUpdate();

            TMP_TextInfo textInfo = textComponent.textInfo;

            int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
            int visibleCount = 0;

            while (visibleCount > totalVisibleCharacters)
            {
                if (hasTextChanged)
                {
                    totalVisibleCharacters = textInfo.characterCount; // Update visible character count.
                    hasTextChanged = false;
                }
                textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?
                visibleCount += 1;
                yield return null;
            }
        }
        narrating = false;
    }

    /// <summary>
    /// Method revealing the text one word at a time.
    /// </summary>
    private IEnumerator FadeWordIn() // Not used currently
    {
        if (tM.Length > 0)
        {
            narrating = true;

            for (int i = 0; i < tM.Length; i++)
            {
                TMP_Text textComponent = tM[i].GetComponent<TMP_Text>();
                textComponent.ForceMeshUpdate();

                int totalWordCount = textComponent.textInfo.wordCount;
                int totalVisibleCharacters = textComponent.textInfo.characterCount; // Get # of Visible Character in text object
                int counter = 0;
                int currentWord = 0;
                int visibleCount = 0;

                while (visibleCount < totalVisibleCharacters)
                {
                    currentWord = counter % (totalWordCount + 1);

                    // Get last character index for the current word.
                    if (currentWord == 0) // Display no words.
                    {
                        visibleCount = 0;
                    }
                    else if (currentWord < totalWordCount)
                    { // Display all other words with the exception of the last one.
                        visibleCount = textComponent.textInfo.wordInfo[currentWord - 1].lastCharacterIndex + 1;

                        for (int u = textComponent.textInfo.wordInfo[currentWord - 1].firstCharacterIndex; u < visibleCount; u++)
                        {
                            textComponent.textInfo.characterInfo[u].color.a = 0;
                        }
                    }
                    else if (currentWord == totalWordCount) // Display last word and all remaining characters.
                    {
                        visibleCount = totalVisibleCharacters;
                    }

                    textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?
                    TMP_CharacterInfo charInfo = textComponent.textInfo.characterInfo[1];
                    counter += 1;

                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    // Event received when the text object has changed.
    private void ON_TEXT_CHANGED(Object obj)
    {
        hasTextChanged = true;
    }
}
