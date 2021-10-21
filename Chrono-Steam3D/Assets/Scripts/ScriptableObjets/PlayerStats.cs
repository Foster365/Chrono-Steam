using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Player/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _dashDistance;
    [SerializeField]
    private float _dashDuration;
    [SerializeField]
    private float _dashCoolDown;
    [SerializeField]
    private float _maxLife;
    [SerializeField]
    private PlayerAbilitiStats _abilitiStats;
    [SerializeField]
    private Transform _spawnPoint;
    [SerializeField]
    private GameObject _weapon;

    public float Speed => _speed;
    public float DashDistance => _dashDistance;
    public float DashDuration => _dashDuration;
    public float DashCoolDown => _dashCoolDown;
    public float MaxLife => _maxLife;
    public Transform SpawnPoint1 => _spawnPoint;
    public GameObject Weapon { get => _weapon; set => _weapon = value; }
    public PlayerAbilitiStats AbilitiStats => _abilitiStats;
}
