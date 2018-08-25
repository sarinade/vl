using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : Singleton<HUD>
{
    #region Inspector

    [SerializeField]
    private Text weaponNameText = null;

    #endregion

    public void SetWeaponNameLabel(string name)
    {
        weaponNameText.text = name;
    }
}
