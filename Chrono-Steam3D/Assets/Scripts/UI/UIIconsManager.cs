using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIIconsManager : MonoBehaviour
{
    [SerializeField] WeaponIconUI[] weaponsIcons;

    public static UIIconsManager instance;

    private void Awake()
    {

        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        //foreach (WeaponIconUI i in weaponsIcons)
        //{
        //    i.IconImageUI = weaponsIcons.IconImageUI;
        //}
    }

    public void EnableIcon(string name)
    {
        WeaponIconUI i = Array.Find(weaponsIcons, weaponIcon => weaponIcon.IconName == name);
        if (i == null)
        {
            return;
        }
        i.IconImageUI.enabled = true;
    }

}
