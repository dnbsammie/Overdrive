using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class SceneController : MonoBehaviour
{
    [Header("Loading Screen Settings")]
    public GameObject loadScreen;
    public Slider loadBar;
    public TextMeshProUGUI textProgress;
    public float currentPercent;
    public AsyncOperation loadAsync;
    private void Update()
    {
        loadBar.value = Mathf.MoveTowards(loadBar.value, currentPercent, 50 * Time.deltaTime);
    }
    public void LoadLevel(string nameLevel)
    {
        loadScreen.SetActive(true);
        StartCoroutine(LoadAsync(nameLevel));
    }
    IEnumerator LoadAsync(string nameLevel)
    {
        textProgress.text = "00&";
        loadAsync = SceneManager.LoadSceneAsync(nameLevel);
        while (!loadAsync.isDone)
        {
            currentPercent = loadAsync.progress * 100 / 0.9f;
            textProgress.text = "." + currentPercent.ToString("00") + "%";
            yield return null;
        }
    }
    //Fast load
    public void FastLoad(string nameLevel)
    {
        SceneManager.LoadScene(nameLevel);
    }
    //Quit
    public void Leave()
    {
        Application.Quit();
    }
}