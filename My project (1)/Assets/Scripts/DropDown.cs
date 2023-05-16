using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropDown : MonoBehaviour
{

    public TextMeshProUGUI output;
    
    // Start is called before the first frame update
    public void HandleInputData(int val)
    {
        if(val == 0)
        {
            Debug.Log("Sucesso 1");
        }
        if(val == 1)
        {
            Debug.Log("Sucesso 2");
        }
        if(val == 2)
        {
            Debug.Log("Sucesso 3");
        }
    }
}
