using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    [Header("Game Data")]
    public Image wifiImg;
    [Header("Clock")]
    public TMP_Text hourText;
    public Sprite daySprite;
    public Sprite noonSprite;
    public Sprite nightSprite;
    public GameObject referenceObject;
    void Start()
    {
    }
    void Update()
    {
        TimeCycle();
    }
    void TimeCycle()
    {
        DateTime actualTime = DateTime.Now;
        string formhour = actualTime.ToString("hh:mm tt");
        hourText.text = formhour;
    }
    //USER VAR
    //INICIALÑIZACION
    //GESTION DE EVENTOS
    //ESTADO Y ACTUALIZACIÓN
    //GESTION DE ENTRADA
    //GESTION DE TIEMPO
}