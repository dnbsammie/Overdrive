using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RearView : MonoBehaviour
{
    public RenderTexture renderTexture;
    public float rvRate = 0.5f;
    void Start()
    {
        StartCoroutine(UpdateRetrovisor());
    }
    void Update()
    {
        if (renderTexture != null)
        {
            RenderTexture temporalTexture = RenderTexture.GetTemporary(renderTexture.width, renderTexture.height);

            Camera.main.targetTexture = temporalTexture;
            Camera.main.Render();

            Camera.main.targetTexture = null;

            Graphics.Blit(temporalTexture, renderTexture);

            RenderTexture.ReleaseTemporary(temporalTexture);
        }
    }
    IEnumerator UpdateRetrovisor()
    {
        while (true)
        {
            yield return new WaitForSeconds(rvRate);
        }
    }
}