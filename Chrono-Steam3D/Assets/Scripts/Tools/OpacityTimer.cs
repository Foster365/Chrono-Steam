using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpacityTimer : MonoBehaviour
{
    [SerializeField] private float _time;
    [SerializeField] private SpriteRenderer [] _images;
    [SerializeField] private TextMeshProUGUI [] _texts;
    private float _currentTime;
    private void Awake()
    {
        _currentTime = _time;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_currentTime <= 0)
        {
            if (_images.Length !=0)
            {
                foreach (var item in _images)
                {
                    if (item.color.a < 255)
                    {
                        item.color += new Color(0, 0, 0, 0.1f);
                    }
                    else
                        return;
                }

            }
            if(_texts.Length !=0)
            foreach (var item in _texts)
            {
                if (item.color.a < 255)
                {
                    item.color += new Color(0, 0, 0, 0.1f);
                }
                else
                    return;
            }
        }
        else
        {
            if (_images.Length != 0)
            foreach (var item in _images)
            {
                if (item.color.a > 0)
                {
                    item.color -= new Color(0, 0, 0, 0.1f);
                }
                else
                    return;
            }

            if (_texts.Length != 0)
            foreach (var item in _texts)
            {
                if (item.color.a > 0)
                {
                    item.color -= new Color(0, 0, 0, 0.1f);
                }
                else
                    return;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _currentTime -= Time.deltaTime;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _currentTime = _time;
        }
    }
}
