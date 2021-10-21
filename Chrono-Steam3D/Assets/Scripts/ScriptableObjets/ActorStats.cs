using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ActorStats")]
public class ActorStats : ScriptableObject
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float meleeDamage = 40f;
    [SerializeField] private List<int> lifeRange = new List<int>(3);

    public float Speed { get => speed; set => speed = value; }
    public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float MeleeDamage { get => meleeDamage; set => meleeDamage = value; }
    public List<int> LifeRange { get => lifeRange; }
}
