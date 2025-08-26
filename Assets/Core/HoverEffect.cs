using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;
    private TMP_Text buttonText;
    private Color normalColor;
    public Color hoverColor;
    private Vector3 originalScale;
    public float hoverScaleFactor = 1.025f;
    public float fadeDuration = 0.1f;
    void Start()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
        buttonImage = GetComponent<Image>();
        normalColor = buttonText.color;
        originalScale = transform.localScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(ApplyHover(hoverColor, originalScale * hoverScaleFactor));
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(ApplyHover(normalColor, originalScale));
    }
    private IEnumerator ApplyHover(Color targetColor, Vector3 targetScale)
    {
        float elapsedTime = 0f;
        Color startColor = buttonText.color;
        Vector3 startScale = buttonImage.transform.localScale;
        while (elapsedTime < fadeDuration)
        {
            buttonText.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            buttonImage.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / fadeDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        buttonText.color = targetColor;
        buttonImage.transform.localScale = targetScale;
    }
}