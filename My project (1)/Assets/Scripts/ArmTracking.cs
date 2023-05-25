using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;
using TMPro;

public class ArmTracking : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public float adjustmentTime = 3f;
    public float minDistanceX = 0.5f;

    private bool isTracking = false;
    private float leftArmHeight = 0f;
    private float rightArmHeight = 0f;
    private float initialArmHeight = 0f;
    private float maxArmHeight = 0f;
    private Vector3 headPosition;

    private void Start()
    {
        resultText.text = "Se posicione";
    }

    public void StartTracking()
    {
        StartCoroutine(AdjustmentTime());
    }

    private System.Collections.IEnumerator AdjustmentTime()
    {
        isTracking = false;
        leftArmHeight = 0f;
        rightArmHeight = 0f;
        initialArmHeight = GetAverageArmHeight();
        resultText.text = "Se posicione";

        yield return new WaitForSeconds(adjustmentTime);

        maxArmHeight = GetMaxArmHeight();
        isTracking = true;

        UpdateArmDistances();
    }

    private void Update()
    {
        if (isTracking)
        {
            Vector3 leftHandPosition = GetHandPosition(XRNode.LeftHand);
            Vector3 rightHandPosition = GetHandPosition(XRNode.RightHand);

            if (leftHandPosition != Vector3.zero && rightHandPosition != Vector3.zero)
            {
                float currentLeftArmHeight = Mathf.Clamp01((leftHandPosition.y - initialArmHeight) / (maxArmHeight - initialArmHeight));
                float currentRightArmHeight = Mathf.Clamp01((rightHandPosition.y - initialArmHeight) / (maxArmHeight - initialArmHeight));
                leftArmHeight = Mathf.Max(leftArmHeight, currentLeftArmHeight);
                rightArmHeight = Mathf.Max(rightArmHeight, currentRightArmHeight);

                float distanceX = Mathf.Abs(leftHandPosition.x - rightHandPosition.x);

                if (distanceX < minDistanceX && (leftArmHeight * 100) >= 20f && (rightArmHeight * 100) <= 90f)
                {
                    resultText.text = "Afastar os braços do corpo!";
                }
                else
                {
                    resultText.text = "Braço Esquerdo: " + (leftArmHeight * 100).ToString("F0") + "%" +
                                      "\nBraço Direito: " + (rightArmHeight * 100).ToString("F0") + "%";
                }
            }
            else
            {
                Debug.Log("Failed to get hand positions");
            }
        }
    }

    private Vector3 GetHandPosition(XRNode handNode)
    {
        List<XRNodeState> nodeStates = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodeStates);

        foreach (var nodeState in nodeStates)
        {
            if (nodeState.nodeType == handNode)
            {
                Vector3 handPosition;
                if (nodeState.TryGetPosition(out handPosition))
                {
                    return handPosition;
                }
            }
        }

        return Vector3.zero;
    }

    private float GetAverageArmHeight()
    {
        Vector3 leftHandPosition = GetHandPosition(XRNode.LeftHand);
        Vector3 rightHandPosition = GetHandPosition(XRNode.RightHand);

        if (leftHandPosition != Vector3.zero && rightHandPosition != Vector3.zero)
        {
            headPosition = GetHandPosition(XRNode.Head) + new Vector3(0f, 0.5f, 0f);
            return (leftHandPosition.y + rightHandPosition.y) * 0.5f;
        }

        return 0f;
    }

    private float GetMaxArmHeight()
    {
        Vector3 floorPosition = new Vector3(headPosition.x, 0f, headPosition.z);
        return Vector3.Distance(headPosition, floorPosition);
    }

    private void UpdateArmDistances()
    {
        resultText.text = "Braço Esquerdo: " + (leftArmHeight * 100).ToString("F0") + "%" +
                          "\nBraço Direito: " + (rightArmHeight * 100).ToString("F0") + "%";
    }
}
