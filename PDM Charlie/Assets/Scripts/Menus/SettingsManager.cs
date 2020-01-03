using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    string saveFilePath;

    public float audioMainInit;
    public Slider sliderAudioMain;

    public float audioSFXInit;
    public Slider sliderAudioSFX;

    public float audioBGMInit;
    public Slider sliderAudioBGM;

    public AudioMixer audioMixer;

    public float volumeButtonAmount = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/save.dat";
        audioMixer.GetFloat("Master", out audioMainInit);
        audioMixer.GetFloat("Environment", out audioSFXInit);
        audioMixer.GetFloat("BackgroundMusic", out audioBGMInit);
        LoadSettingsFromFile();
    }

    public void UpdateVolume(Slider slider)
    {
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>();

        if (slider == sliderAudioMain)
        {
            gc.settings.audioMain = sliderAudioMain.value;
            audioMixer.SetFloat("Master", Mathf.Log10(gc.settings.audioMain) * 10);
            //Debug.Log($"Updated main volume to {sliderAudioMain.value}!");
        }
        else if (slider == sliderAudioSFX)
        {
            gc.settings.audioSFX = sliderAudioSFX.value;
            audioMixer.SetFloat("Environment", Mathf.Log10(gc.settings.audioSFX) * 10);
            //Debug.Log($"Updated sfx volume to {sliderAudioMain.value}!");
        }
        else if (slider == sliderAudioBGM)
        {
            gc.settings.audioBGM = sliderAudioBGM.value;
            audioMixer.SetFloat("BackgroundMusic", Mathf.Log10(gc.settings.audioBGM) * 10);
            //Debug.Log($"Updated bgm volume to {sliderAudioMain.value}!");
        }
    }

    public void RaiseVolume(Slider slider)
    {
        slider.value += volumeButtonAmount;
        UpdateVolume(slider);
    }

    public void LowerVolume(Slider slider)
    {
        slider.value -= volumeButtonAmount;
        UpdateVolume(slider);
    }

    public void LoadSettingsFromFile()
    {
        FileStream file;
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>();
        Settings settings = new Settings();

        if (File.Exists(saveFilePath))
        {
            file = File.OpenRead(saveFilePath);

            BinaryFormatter bf = new BinaryFormatter();
            settings = (Settings)bf.Deserialize(file);

            file.Close();
        }
        else
        {
            //Debug.LogError("File not found");
        }


        gc.settings = settings;

        sliderAudioMain.value = settings.audioMain;
        audioMixer.SetFloat("MainMenu", Mathf.Log10(settings.audioMain)*10);
        sliderAudioSFX.value = settings.audioSFX;
        audioMixer.SetFloat("Environment", Mathf.Log10(settings.audioSFX) * 10);
        sliderAudioBGM.value = settings.audioBGM;
        audioMixer.SetFloat("BackgroundMusic", Mathf.Log10(settings.audioBGM) * 10);

        //Debug.Log("Loaded audio settings from file!");
        return;
    }

    public void SaveSettingsToFile()
    {
        try
        {
            FileStream file;

            if (File.Exists(saveFilePath)) file = File.OpenWrite(saveFilePath);
            else file = File.Create(saveFilePath);

            GameController gc = GameObject.Find("GameController").GetComponent<GameController>();

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, gc.settings);
            file.Close();
        }
        catch (Exception e)
        {
            //Debug.LogError($"{e.Message}");
        }
    }
}
