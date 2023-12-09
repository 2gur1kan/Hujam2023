using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Canvas : MonoBehaviour
{
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject ControlPanel;
    [SerializeField] private GameObject SoundPanel;

    private bool isPaused;

    private void Awake()
    {
        PausePanelOff();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            PausePanelControl();

            if (isPaused) Time.timeScale = 0f;
            else Time.timeScale = 1f;

        }
    }

    public void PausePanelControl()
    {
        SettingsPanelOff();
        PausePanel.SetActive(!PausePanel.activeSelf);
    }

    public void PausePanelOff()
    {
        SettingsPanelOff();
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SettingsPanelControl()
    {
        SettingsPanel.SetActive(!SettingsPanel.activeSelf);
    }

    public void SettingsPanelOff()
    {
        SettingsPanel.SetActive(false);
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

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
