using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WeaponIconUI : MonoBehaviour
{

    [SerializeField]
    string iconName;

    [SerializeField] Image iconImageUI;

    public string IconName { get => iconName; }
    public Image IconImageUI { get => iconImageUI; set => iconImageUI = value; }

}
