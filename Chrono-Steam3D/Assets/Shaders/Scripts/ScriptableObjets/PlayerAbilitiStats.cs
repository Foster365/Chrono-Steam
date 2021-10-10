using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Player/PlayerAbilitiStats")]
public class PlayerAbilitiStats : ScriptableObject
{
    [SerializeField]
    private float _punchDistance;
    [SerializeField]
    private float _punchDuration;
    [SerializeField]
    private float _punchCoolDown;
    [SerializeField]
    private float _punchDamage;
    [SerializeField]
    private float _punchArea;

    public float PunchDistance => _punchDistance;
    public float PunchDuration => _punchDuration;
    public float PunchCoolDown => _punchCoolDown;
    public float PunchDamage => _punchDamage;
    public float PunchArea => _punchArea;
}
