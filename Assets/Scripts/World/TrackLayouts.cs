using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackLayouts : MonoBehaviour
{
    [Header("Circuits Layouts")]
    public GameObject circuit1;
    public GameObject circuit2;
    public GameObject circuit3;
    public GameObject circuit4;
    public GameObject circuit5;
    public GameObject circuit6;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void ActivateSection(GameObject newSection)
    {
        circuit1.SetActive(false);
        circuit2.SetActive(false);
        circuit3.SetActive(false);
        circuit4.SetActive(false);
        circuit5.SetActive(false);
        circuit6.SetActive(false);
        newSection.SetActive(true);
    }
}