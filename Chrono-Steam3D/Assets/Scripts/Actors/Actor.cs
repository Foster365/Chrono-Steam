using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] private ActorStats stats;

    public ActorStats Stats { get => stats;}
}
