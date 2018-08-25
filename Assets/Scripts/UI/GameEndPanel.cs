using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndPanel : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private Text header = null;

    [Space]

    [SerializeField]
    private string victoryHeader = "Victory";

    [SerializeField]
    private string gameOverHeader = "Game Over";

    #endregion

    public void Show(bool victory)
    {
        Time.timeScale = 0.0f;
        header.text = victory ? victoryHeader : gameOverHeader;

        gameObject.SetActive(true);
    }

    public void OnRetryButtonClick()
    {
        Time.timeScale = 1.0f;
        GameLoop.Instance.LoadGameScene();
    }
    
    public void OnQuitButtonClick()
    {
        Time.timeScale = 1.0f;
        GameLoop.Instance.LoadMenuScene();
    }
}
