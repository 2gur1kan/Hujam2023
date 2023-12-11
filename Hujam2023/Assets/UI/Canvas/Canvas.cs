using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Canvas : MonoBehaviour
{   
    [Header("Pause")]
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject ControlPanel;
    [SerializeField] private GameObject SoundPanel;

    [Header("")]
    [SerializeField] private GameObject PlayerIcons;
    [SerializeField] private GameObject DeadScreen;

    private Health player;
    private bool isPaused;

    private void Awake()
    {
        PausePanelOff();
        PlayerIcons.SetActive(true);
        DeadScreen.SetActive(false);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    void Update()
    {
        if (player.CurrentHealth < 1) Dead();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            PausePanelControl();

            if (isPaused) Time.timeScale = 0f;
            else Time.timeScale = 1f;

        }
    }

    public void setAudioVolume(Slider Value)
    {
        AudioListener.volume = Value.value;
    }

    public void Dead()
    {
        PlayerIcons.SetActive(false);
        DeadScreen.SetActive(true);

        Time.timeScale = 0f;
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
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
