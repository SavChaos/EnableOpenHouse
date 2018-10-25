using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

public class Book : Singleton<Book>
{
    public List<Page> pages = new List<Page>();
    public List<GameObject> pagesToLoad = new List<GameObject>();
    public Button nextButton;
    public Button previousButton;
    public bool autoTurn = false;
    public GameObject optionsPanel;
    public int startPage = 0;

    private const string PP_RESUME_PATH = "Resume";
    private int currentPage = 0;

    [HideInInspector]
    public UnityEvent onPageChange;

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        currentPage = startPage;
        OpenBook(startPage);
    }

    public bool IsFirstPage(Page _page)
    {
        return pages[0] == _page ? true : false;
    }

    public bool IsLastPage(Page _page)
    {
        return pages[pages.Count - 1] == _page ? true : false;
    }

    public void NextPage()
    {
        if (pagesToLoad.Count > 0)
        {
            GameObject loadedPage = Instantiate(pagesToLoad[0]);
            loadedPage.transform.SetParent(transform, false);
            pages.Insert(pages.Count - 1, loadedPage.GetComponent<Page>());
            pagesToLoad.RemoveAt(0);
        }
        if ((currentPage + 1) < transform.childCount)
        {
            onPageChange.Invoke();
            pages[currentPage].Disable();
            currentPage++;
            PlayerPrefs.SetInt(PP_RESUME_PATH, currentPage);
            PlayerPrefs.Save();
            pages[currentPage].Enable();
        }
    }

    public void PreviousPage() // TODO: is this used?
    {
        if ((currentPage - 1) >= 0)
        {
            onPageChange.Invoke();
            pages[currentPage].Disable();
            currentPage--;
            pages[currentPage].Enable();
        }
    }

    public void NextButton()
    {
        nextButton.gameObject.SetActive(true);
        if (autoTurn)
        {
            NextPage();
        }
    }

    public void SetNextButton(bool isActive)
    {
        nextButton.gameObject.SetActive(isActive);
    }

    public void PreviousButton(bool isActive)
    {
        previousButton.gameObject.SetActive(isActive);
    }

    public void OptionsPanel(bool isActive)
    {
        optionsPanel.gameObject.SetActive(isActive);
    }

    public void OpenBook(int pageIndex)
    {
        pages[pageIndex].Enable();
    }

    public void TurnToPage(int pageIndex)
    {
        if ((pageIndex) != currentPage)
        {
            if (IsLastPage(pages[pageIndex]) == true)
            {
                if (optionsPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Opened"))
                {
                    optionsPanel.GetComponent<OptionsMenuToggle>().SetTrigger("Open");
                }
            }
            //AudioManager.Instance.PageTurn();
            pages[currentPage].Disable();
            pages[pageIndex].Enable();
            currentPage = pageIndex;
        }
    }

    public void AutoTurn(bool autoTurnEnabled) // TODO: is this used?
    {
        autoTurn = autoTurnEnabled;
    }

    public void AutoFlip() // TODO: is this used?
    {
        if (autoTurn)
        {
            NextPage();
        }
    }

    public void Resume() // TODO: is this used?
    {
        int pageResume = PlayerPrefs.GetInt(PP_RESUME_PATH);
        if (pageResume < pages.Count - 1)
        {
            TurnToPage(pageResume);
        }
        else
        {
            PlayerPrefs.SetInt(PP_RESUME_PATH, 0);
            TurnToPage(1);
        }
    }
}
