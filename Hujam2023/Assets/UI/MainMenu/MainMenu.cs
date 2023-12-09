using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject MenuPanel;

    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject ControlPanel;
    [SerializeField] private GameObject SoundPanel;

    [SerializeField] private GameObject CreditsPanel;


    private void Awake()
    {
        Back();
    }

    public void CreaditsPanelOn()
    {
        MenuPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }

    public void SettingsPanelOn()
    {
        MenuPanel.SetActive(false);
        SettingsPanel.SetActive(true);
        CreditsPanel.SetActive(false);
    }

    public void Back()
    {
        MenuPanel.SetActive(true);
        SettingsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
    }

    public void ControlPanelOn()
    {
        ControlPanel.SetActive(true);
        SoundPanel.SetActive(false);
    }

    public void SoundPanelOn()
    {
        ControlPanel.SetActive(false);
        SoundPanel.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
