using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private SceneParams sceneParams = null;

    #endregion

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(sceneParams.GameSceneIndex);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
