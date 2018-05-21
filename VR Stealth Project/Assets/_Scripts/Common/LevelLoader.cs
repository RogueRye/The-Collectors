using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

    [SerializeField]
    GameObject loadingScreen;
    [SerializeField]
    Slider slider;
    [SerializeField]
    Text progressText;
    

    public void Quit()
    {
        Application.Quit();
    }

	public void LoadLevel(int index)
    {
        StartCoroutine(LoadAsynchronously(index));         
    }

    IEnumerator LoadAsynchronously (int index)
    {


        var operation = SceneManager.LoadSceneAsync(index);
        if(loadingScreen != null)
            loadingScreen.SetActive(true); 

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            if(slider != null)
                slider.value = progress;

            if (progressText != null)
                progressText.text = progress * 100 + "%";

            yield return null;
        }
    }


}
