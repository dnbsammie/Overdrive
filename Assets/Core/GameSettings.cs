using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameSettings : MonoBehaviour
{
    [Header("Option Sections")]
    public GameObject audioSection;
    public GameObject controlSection;
    public GameObject prefSection;
    public GameObject videoSection;
    [Header("Audio Settings")]
    public Slider masterSLD;
    public Image audio1;
    public Image audio2;
    public Image audio3;
    public Image audio4;
    public float masterSLDValue;
    public enum AudioMode
    {
        Mono,
        Stereo,
        Surround
    }
    public AudioMode audioMode = AudioMode.Stereo;
    public TMP_Text buttonText;
    public Image mode1;
    public Image mode2;
    public Image mode3;
    public AudioSource audioSource;
    void Start()
    {
        //Master Volume
        masterSLD.value = PlayerPrefs.GetFloat("volumenAudio", 0.5f);
        AudioListener.volume=masterSLD.value;
        CheckMute();
        SetAudioMode();
    }
    void Update()
    {
        
    }
    public void ActivateSection(GameObject newSection)
    {
        audioSection.SetActive(false);
        controlSection.SetActive(false);
        prefSection.SetActive(false);
        videoSection.SetActive(false);

        newSection.SetActive(true);
    }
    public void ChangeSlider(float percent)
    {
        masterSLDValue = percent;
        PlayerPrefs.SetFloat("volumenAudio",masterSLDValue);
        AudioListener.volume = masterSLD.value;
        CheckMute();
    }
    public void CheckMute()
    {
        if(masterSLDValue==0)
        {
            audio1.enabled = true;
            audio2.enabled = false;
            audio3.enabled = false;
            audio4.enabled = false;
        }
        else if (masterSLDValue<0.3)
        {
            audio1.enabled = false;
            audio2.enabled = true;
            audio3.enabled = false;
            audio4.enabled = false;
        }
        else if (masterSLDValue<0.5)
        {
            audio1.enabled = false;
            audio2.enabled = false;
            audio3.enabled = true;
            audio4.enabled = false;
        }
        else if (masterSLDValue>0.7)
        {
            audio1.enabled = false;
            audio2.enabled = false;
            audio3.enabled = false;
            audio4.enabled = true;
        }
    }
    private void SetAudioMode()
    {
        switch (audioMode)
        {
            case AudioMode.Mono:
                audioSource.spatialBlend = 0f;
                buttonText.text = "Mono";
                mode1.enabled = true;
                mode2.enabled = false;
                mode3.enabled = false;
                break;
            case AudioMode.Stereo:
                audioSource.spatialBlend = 0.5f;
                buttonText.text = "Stereo";
                mode1.enabled = false;
                mode2.enabled = true;
                mode3.enabled = false;
                break;
            case AudioMode.Surround:
                audioSource.spatialBlend = 1f;
                buttonText.text = "Surround";
                mode1.enabled = false;
                mode2.enabled = false;
                mode3.enabled = true;
                break;
        }
    }
    public void ToggleAudioMode()
    {
        audioMode = (AudioMode)(((int)audioMode + 1) % System.Enum.GetValues(typeof(AudioMode)).Length);
        SetAudioMode();
    }
}