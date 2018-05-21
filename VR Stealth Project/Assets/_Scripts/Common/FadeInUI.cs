using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeInUI : MonoBehaviour {

    [SerializeField]
    bool doesChangeScene;
    [SerializeField]
    int sceneIndex;

    public float fadeRate;
    private CanvasGroup canvasGroup;
    IEnumerator FadeOutMethod;
    // Use this for initialization
    void OnEnable()
    {
        this.canvasGroup = this.GetComponent<CanvasGroup>();
        if (this.canvasGroup == null)
        {
            Debug.LogError("Error: No image on " + this.name);
        }

        FadeInCall();
    }

    // Update is called once per frame
    void Update()
    {
        //Color curColor = this.image.color;
        //float alphaDiff = Mathf.Abs(curColor.a - this.targetAlpha);
        //if (alphaDiff > 0.0001f)
        //{
        //    curColor.a = Mathf.Lerp(curColor.a, targetAlpha, this.FadeRate * Time.deltaTime);
        //    this.image.color = curColor;
        //}

    }

    public void FadeOutCall(bool doesChangeScene = false, int sceneIndex = 0)
    {
        StartCoroutine(FadeOut(doesChangeScene, sceneIndex));
    }

    public void FadeInCall()
    {
        StartCoroutine(FadeIn());
    }


    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(.5f);

        float startAlpha = 1;

        float rate = 1 / fadeRate;

        float progress = 0;

        while (progress < 1f)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;

   
    }

    IEnumerator FadeOut(bool doesChangeScene, int sceneIndex)
    {
        yield return new WaitForSeconds(.5f);

        float startAlpha = 0;

        float rate = 1 / fadeRate;

        float progress = 0;

        while (progress < 1f)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        if (doesChangeScene)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            FadeInCall();

        }


    }
}
