using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Weapon/AreaStats")]
public class AreaStats : ScriptableObject
{
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _maxAmplitude;

    public float MaxDistance => _maxDistance;
    public float MaxAmplitude  => _maxAmplitude;
}
