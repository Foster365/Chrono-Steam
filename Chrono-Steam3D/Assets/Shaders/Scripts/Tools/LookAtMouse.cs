using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    public GameObject target;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    public void Update()
    {
        LookMouse();
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 destination = new Vector3(mousePos.x, mousePos.y, 0);
        // target.transform.position = destination;
    }
    void LookMouse()
    {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }
}
