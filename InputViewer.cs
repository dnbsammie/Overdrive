using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InputViewer : MonoBehaviour
{
    public InputController inputController;
    public Slider throttleSlider;
    public Slider brakeSlider;
    public Slider clutchSlider;
    public Slider handbrakeSlider;
    public Image steeringImage;
    void Update()
    {
        //UI sliders
        throttleSlider.value = Input.GetAxis("RT");
        brakeSlider.value = Input.GetAxis("A");
        clutchSlider.value = Input.GetAxis("LT");
        handbrakeSlider.value = Input.GetAxis("X");
        // Update steering image rotation
        float horizontalInput = Input.GetAxis("Stick 1 X");
        float rotationAngle = horizontalInput * -90f;
        steeringImage.rectTransform.localEulerAngles = new Vector3(0, 0, rotationAngle);
    }
}