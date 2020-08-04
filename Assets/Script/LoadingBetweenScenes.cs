using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingBetweenScenes : MonoBehaviour
{
    public Slider slider;

    AsyncOperation async_operation;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartLoad("LobbyScene"));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator StartLoad(string scenename)
    {
        yield return new WaitForSeconds(1);

        async_operation = SceneManager.LoadSceneAsync(scenename);
        async_operation.allowSceneActivation = false;

        while (!async_operation.isDone)
        {
            slider.value = async_operation.progress;

            if (async_operation.progress >= 0.9f)
            {
                async_operation.allowSceneActivation = true;
            }

            Debug.Log("progess : " + async_operation.progress);
            yield return null;

        }
    }
}
