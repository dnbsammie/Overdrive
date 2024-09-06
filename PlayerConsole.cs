using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class PlayerConsole : MonoBehaviour
{
    [Header("Inputs")]
    public KeyCode lcdUp = KeyCode.W;
    public KeyCode lcdDown = KeyCode.S;
    public KeyCode lcdLeft = KeyCode.A;
    public KeyCode lcdRight = KeyCode.D;
    [Header("Elements")]
    public TMP_Text typeFunction;
    public Slider trcSlider;
    public TMP_Text trcText;
    public Slider ABSSlider;
    public TMP_Text absText;
    public Slider biasSlider;
    public TMP_Text biasText;
    public TMP_Text ptButton1;
    public TMP_Text ptButton2;
    public TMP_Text ptButton3;
    public TMP_Text ptButton4;
    public GameObject[] panels;
    private int currentPanelIndex = 0;
    void Start()
    {
        ShowPanel(currentPanelIndex);
    }
    void Update()
    {
        if (Input.GetKeyDown(lcdLeft))
        {
            ChangePanel(-1);
        }
        else if (Input.GetKeyDown(lcdRight))
        {
            ChangePanel(1);
        }
        if (Input.GetKeyDown(lcdDown))
        {
            DecrementSlider();
        }
        else if (Input.GetKeyDown(lcdUp))
        {
            IncrementSlider();
        }
        UpdateTextValues();
    }
    void ChangePanel(int direction)
    {
        panels[currentPanelIndex].SetActive(false);
        currentPanelIndex = (currentPanelIndex + direction + panels.Length) % panels.Length;
        ShowPanel(currentPanelIndex);
    }
    void ShowPanel(int index)
    {
        panels[index].SetActive(true);
        typeFunction.text = panels[index].name;
    }
    void IncrementSlider()
    {
        if (trcSlider.IsActive() && trcSlider.value + 1 <= trcSlider.maxValue)
        {
            trcSlider.value += 1;
        }
        else if (ABSSlider.IsActive() && ABSSlider.value + 1 <= ABSSlider.maxValue)
        {
            ABSSlider.value += 1;
        }
        else if (biasSlider.IsActive() && biasSlider.value + 1 <= biasSlider.maxValue)
        {
            biasSlider.value += 1;
        }
    }
    void DecrementSlider()
    {
        if (trcSlider.IsActive() && trcSlider.value - 1 >= trcSlider.minValue)
        {
            trcSlider.value -= 1;
        }
        else if (ABSSlider.IsActive() && ABSSlider.value - 1 >= ABSSlider.minValue)
        {
            ABSSlider.value -= 1;
        }
        else if (biasSlider.IsActive() && biasSlider.value - 1 >= biasSlider.minValue)
        {
            biasSlider.value -= 1;
        }
    }
    void UpdateTextValues()
    {
        trcText.text = trcSlider.value.ToString("F0");
        absText.text = ABSSlider.value.ToString("F0");
        biasText.text = biasSlider.value.ToString("F0");
    }
}