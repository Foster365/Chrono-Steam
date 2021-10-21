using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openDirection;
    // 1 = top door / need a botom door conected
    // 2 = rigth door / need a lefth door conected
    // 3 = botom door / need a top door conected
    // 4 = left door / need a rigth door conected

    [SerializeField]
    private RoomTamplates templates;
    private int random;
    public bool spawned;

    void Start()
    {
        templates = GameObject.FindWithTag("RoomTamplates").GetComponent<RoomTamplates>();
        Invoke("SpawnRoom",0.1f);
    }

    void SpawnRoom()
    {
        if (!spawned)
        {
            if (openDirection == 1)
            {
                random = Random.Range(0, templates.botomDoorRooms.Length);
                Instantiate(templates.botomDoorRooms[random], transform.position, Quaternion.identity);
            }
            else if (openDirection == 2)
            {
                random = Random.Range(0, templates.lefthDoorRooms.Length);
                Instantiate(templates.lefthDoorRooms[random], transform.position, Quaternion.identity);
            }
            else if (openDirection == 3)
            {
                random = Random.Range(0, templates.topDoorRooms.Length);
                Instantiate(templates.topDoorRooms[random], transform.position, Quaternion.identity);
            }
            else if (openDirection == 4)
            {
                random = Random.Range(0, templates.rightDoorRooms.Length);
                Instantiate(templates.rightDoorRooms[random], transform.position, Quaternion.identity);
            }
            else if (openDirection <= 0)
            {
                Debug.Log("non open direction asigned");
            }
            spawned = true;
        }
        else
            Destroy(this.gameObject);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other!=null)
        {
            if (other.CompareTag("SpawnPoint"))
            {
                if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
                {
                    if (templates == null)
                    {
                        templates = GameObject.FindWithTag("RoomTamplates").GetComponent<RoomTamplates>();
                    }
                    random = Random.Range(0, templates.closeDoorRooms.Length);
                    Instantiate(templates.closeDoorRooms[random], transform.position, Quaternion.identity);
                }
                spawned = true;
            }
        }
    }
}
