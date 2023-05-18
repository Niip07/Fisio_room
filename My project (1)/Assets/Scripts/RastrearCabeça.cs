using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RastrearCabeça : MonoBehaviour
{
    bool isTracking = false;
    float elapsedTime = 0f;
    float interactionDuration = 10f;
    Vector3 cameraStartPosition;
    float totalDistance = 0f;

    public TMP_Text percentageText;

    void Update()
    {   
        //Condição para iniciar o rastreamento quando o botão for apertado
        if (isTracking)
        {   
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= interactionDuration)
            {
                float percentageMoved = 100 - ((totalDistance / interactionDuration) * 100f);
                
                percentageText.text = "Porcentagem de movimento: " + percentageMoved.ToString("F2") + "%";

                elapsedTime = 7f;
                //totalDistance = 0f;
                isTracking = true;
            }
            else if (elapsedTime >= 1f)
            {
                Vector3 cameraCurrentPosition = Camera.main.transform.position;
                float distanceMoved = Vector3.Distance(cameraStartPosition, cameraCurrentPosition);
                totalDistance += distanceMoved;

                cameraStartPosition = cameraCurrentPosition;
            }
        }
    }

    public void OnButtonClick()
    {
        isTracking = true;
        cameraStartPosition = Camera.main.transform.position;
        elapsedTime = 0f;
        totalDistance = 0f;
        percentageText.text = "Calculando...";
    }
}


