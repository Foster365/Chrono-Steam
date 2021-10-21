using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torret : Enemy
{
    private float rotTime;
    private float shootCd;
    public GameObject cannon;
    public GameObject bullet;
    [SerializeField] int rotSpeed;
    [SerializeField] float rotTimeMax;
    [SerializeField] float shootCdMax;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        Rotation();
        Shoot();
    }

    private void Rotation()
    {
        transform.Rotate(0, rotSpeed * -1, 0);
        rotTime += Time.deltaTime;

        if (rotTime >= rotTimeMax)
        {
            rotSpeed *= -1;
            rotTime = 0;
        }
    }

    private void Shoot()
    {
        shootCd += Time.deltaTime;
        Quaternion cRot = cannon.transform.rotation;

        if (shootCd >= shootCdMax)
        {
            Instantiate(bullet, cannon.transform.position, cannon.transform.rotation);
            shootCd = 0;
        }
    }
}