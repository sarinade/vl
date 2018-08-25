using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loadout01", menuName = "VL/Gameplay/New Loadout", order = 1)]
public class LoadoutParams : ScriptableObject
{
    public int StartingWeaponIndex = 0;
     
    public WeaponParams[] Weapons = null;

    public WeaponParams GetWeapon(int index)
    {
        if (index < 0 || index >= Weapons.Length)
        {
            Debug.LogFormat("Weapon with index {0} is not present in loadout.", index);
            return null;
        }

        return Weapons[index];
    }

    public int GetWeaponCount()
    {
        return Weapons.Length;
    }
}
