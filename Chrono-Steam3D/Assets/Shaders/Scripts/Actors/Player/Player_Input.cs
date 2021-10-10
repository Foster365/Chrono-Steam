using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    public float xMovement()
    {
        float xMovement = Input.GetAxis("Horizontal");
        return xMovement;
    }
    public float yMovement()
    {
        float yMovement = Input.GetAxis("Vertical");
        return yMovement;
    }
    public bool Action1()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }
        return false;
    }
    public bool Action2()
    {
        if (Input.GetMouseButtonDown(1))
        {
            return true;
        }
        return false;
    }
    public bool Action02()
    {
        if (Input.GetMouseButtonUp(1))
        {
            return true;
        }
        return false;
    }
    public bool Action3()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            return true;
        }
        return false;
    }
    public bool Action4()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        return false;
    }
    public bool Action5()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            return true;
        }
        return false;
    }
}
