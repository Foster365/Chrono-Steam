using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    private UnityEvent _winlvl;
    private UnityEvent _bossDying;
    [SerializeField]
    private GameObject _elevatorDoor;
    [SerializeField]
    private bool BossDead;


    public static LevelManager Instance => instance;
    public UnityEvent BossDying => _bossDying;
    public UnityEvent Winlvl => _winlvl;

    public GameObject ElevatorDoor { get => _elevatorDoor; set => _elevatorDoor = value; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }
    private void Start()
    {
        _winlvl = new UnityEvent();
        _bossDying = new UnityEvent();
        _bossDying.AddListener(BossDie);
        _winlvl.AddListener(GameManager.Instance.GameWin);
        GameManager.Instance.LvlManager = this.gameObject;
    }
    private void Update()
    {
        if (BossDead)
        {
            //deactivate elevator door
            _elevatorDoor.SetActive(false);
        }
        if (ElevatorDoor==null)
        {
            var elevator = GameObject.FindGameObjectsWithTag("ElevatorDoor");
            foreach (var item in elevator)
            {
                if (item.activeSelf)
                {
                    _elevatorDoor = item;
                }
            }
        }
    }
    private void BossDie()
    {
        BossDead = true;
    }
}
