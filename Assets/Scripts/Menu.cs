using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public GameObject Menus;
    public GameObject Settings;
    public GameObject Loading;

    public Object NewGameScene;

    public Slider volumeSlider;
    public Slider progressSlider;

    public Dropdown resolutionDropdown;

    public AudioSource selectSound;

    private int[,] resolutions = {
        {1920, 1080},
        {1680, 1050},
        {1366, 768}
    };

    void Start()
    {
        Menus.SetActive(true);
        Settings.SetActive(false);
        Loading.SetActive(false);
        SetResolution();
        SetVolume();
    }
    public void SetResolution()
    {
        int dropdownOption = resolutionDropdown.value;
        Screen.SetResolution(resolutions[dropdownOption, 0], resolutions[dropdownOption, 1], false);
        Debug.Log("Screen size: " + resolutions[dropdownOption, 0] + "x" + resolutions[dropdownOption, 1]);
    }

    public void ShowSettings(bool value)
    {
        Menus.SetActive(!value);
        Settings.SetActive(value);
    }

    public void Quit()
    {
        Debug.Log("Quit application!");
        Application.Quit();
    }

    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Debug.Log("Volume: " + volumeSlider.value * 100);
    }

    public void Play()
    {
        StartCoroutine(LoadAsynchronously(NewGameScene.name));
    }

    IEnumerator LoadAsynchronously(string index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        Loading.SetActive(true);
        Menus.SetActive(false);


        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            progressSlider.value = progress;
            Debug.Log("Slider progress: " + progress);
            yield return null;
        }
    }

    public void PlaySelectSound()
    {
        selectSound.Play();
    }
}
