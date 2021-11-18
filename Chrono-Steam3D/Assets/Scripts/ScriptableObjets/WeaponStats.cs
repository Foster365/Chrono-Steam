using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObject/Weapon/WeaponStats", order = 1)]
public class WeaponStats : ScriptableObject
{
    [SerializeField] private int attDamage = 30;
    [SerializeField] private int durability = 100;
    [SerializeField] private int durabilityDecrease = 0;
    [SerializeField] private float coolDown = 0;
    [SerializeField] private float _EspExeCd = 3;
    [SerializeField] private int _espDamage = 0;
    [SerializeField] GameObject weaponIconImage;

    //[SerializeField] private IAttack attStrategy;
    public int AttDamage { get => attDamage;}
    public int Durability { get => durability;}
    public float CoolDown { get => coolDown;}
    public int DurabilityDecrease { get => durabilityDecrease;}
    public float EspExeCd { get => _EspExeCd;}
    public int EspDamage { get => _espDamage;}
}
