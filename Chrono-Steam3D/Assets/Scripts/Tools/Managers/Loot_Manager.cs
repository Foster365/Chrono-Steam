using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot_Manager : MonoBehaviour
{
    private Dictionary<GameObject, int> _weaponDrops = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, int> _currentDrops = new Dictionary<GameObject, int>();
    [SerializeField] List<GameObject> drops;
    [SerializeField] List<int> rates;

    public Dictionary<GameObject, int> CurrentDrops => _currentDrops;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < drops.Count; i++)
        {
            _weaponDrops.Add(drops[i], rates[i]);
        }
        for (int i = 0; i < 2; i++)
        {
            _currentDrops.Add(drops[i], rates[i]);
        }
        GameManager.Instance.LootManager = this;
    }

    public void AddWeaponToLoot()
    {
        if (_currentDrops.Count<_weaponDrops.Count)
        {
            _currentDrops.Add(drops[_currentDrops.Count + 1], rates[_currentDrops.Count + 1]);
        }
    }
}
