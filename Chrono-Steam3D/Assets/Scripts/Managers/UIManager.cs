using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Dictionary<string, Image> UIElements;
    [SerializeField] Player_Input playerInputs;
    [SerializeField] Player_Controler playerController;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] UIelem = GameObject.FindGameObjectsWithTag(UtilitiesTags.UI_ICON_TAG);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TestSetIcons()
    {
        //if(playerController.IsWeaponSlotNull) UIElements.ContainsKey()
    }

    void EnableUI(string UIName)
    {

    }
}
