using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public void TogglePausePanel()
    {
        if(gameObject.activeSelf)
        {
            Time.timeScale = 1.0f;
            Player.Instance.FreezeInput = false;
            gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 0.0f;
            Player.Instance.FreezeInput = true;
            gameObject.SetActive(true);
        }
    }

    public void OnContinueButtonClick()
    {
        Time.timeScale = 1.0f;
        Player.Instance.FreezeInput = false;
        gameObject.SetActive(false);
    }

    public void OnQuitButtonClick()
    {
        Time.timeScale = 1.0f;
        GameLoop.Instance.LoadMenuScene();
    }
}
