using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    private UnityEvent _winlvl;
    private UnityEvent _winRoom;
    private UnityEvent _bossDying;
    private Enemy _bossInstance;
    [SerializeField]
    private GameObject _elevatorDoor;
    [SerializeField]
    private bool BossDead;
    [SerializeField]
    private int RoomsToClear;
    [SerializeField]
    private int NextLVL;

    public static LevelManager Instance => instance;
    public UnityEvent BossDying => _bossDying;
    public UnityEvent Winlvl => _winlvl;
    public UnityEvent WinRoom => _winRoom;

    public GameObject ElevatorDoor { get => _elevatorDoor; set => _elevatorDoor = value; }
    public Enemy BossInstance { get => _bossInstance; set => _bossInstance = value; }

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
        _winRoom = new UnityEvent();
        _bossDying = new UnityEvent();
        _bossDying.AddListener(BossDie);
        _winlvl.AddListener(GameManager.Instance.GameWin);
        _winRoom.AddListener(allRoomsClear);
        GameManager.Instance.LvlManager = this.gameObject;
        GameManager.Instance.LvlToCharge = NextLVL;
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
    private void allRoomsClear()
    {
        if (GameManager.Instance.ClearRooms >= RoomsToClear)
        {
            GameManager.Instance.ClearRooms = 0;
            GameManager.Instance.EventQueue.Add(_winlvl);
        }
        else
        {
            GameManager.Instance.ClearRooms++;
            GameManager.Instance.reloadScene();
        }
    }
    private void BossDie()
    {
        BossDead = true;
    }
}
