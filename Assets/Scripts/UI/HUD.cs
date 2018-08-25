using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : Singleton<HUD>
{
    #region Inspector

    [SerializeField]
    private Text weaponNameText = null;

    [SerializeField]
    private GameEndPanel gameEndPanel = null;

    [SerializeField]
    private PausePanel pausePanel = null;

    #endregion

    public void SetWeaponNameLabel(string name)
    {
        weaponNameText.text = name;
    }

    public void ShowGameEndPanel(bool victory)
    {
        gameEndPanel.Show(victory);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.TogglePausePanel();
        }
    }
}
