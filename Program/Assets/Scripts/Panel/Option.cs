using System;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] Dropdown windowModes;
    [SerializeField] Slider volumeSlider;

    [SerializeField] int preWindowMode;
    [SerializeField] float preVolumeValue;

    public void Awake()
    {
        preWindowMode = windowModes.value;
        preVolumeValue = volumeSlider.value;
    }

    public void Save()
    {
        SetWindowModes(windowModes.value);
        AudioManager.Instance.SetVolume(volumeSlider.value);

        preWindowMode = windowModes.value;
        preVolumeValue = volumeSlider.value;

        Quit();
    }

    public void Cancel()
    {
        windowModes.value = preWindowMode;
        volumeSlider.value = preVolumeValue;

        Quit();
    }

    public void Quit()
    {
        GameObject TitleManager = GameObject.FindGameObjectWithTag("Manager");

        if (TitleManager != null)
        {
            PanelManager.Instance.Open(Panel.Title);
        }
        else
        {
            PanelManager.Instance.Open(Panel.Pause);
        }

        AudioManager.Instance.PlaySE("button");

        gameObject.SetActive(false);
    }

    public void SetWindowModes(int currentWindowMode)
    {
        switch (currentWindowMode)
        {
            case 0: // 창화면
                Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
                break;
            case 1: // 테두리 없는 창
                Screen.SetResolution(1920, 1080, FullScreenMode.MaximizedWindow);
                break;
            case 2: // 전체화면
                Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
                break;
            default:
                break;
        }
    }
}
