using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeadTracking : MonoBehaviour
{
    [SerializeField] private float interactionDuration = 10f; // Duração da interação em segundos
    [SerializeField] private float playerHeight = 1.5f; // Altura mínima do jogador para considerá-lo em pé

    [SerializeField] private Button startButton; // Referência ao botão de início na cena
    [SerializeField] private TMP_Text percentageText; // Referência ao componente TextMeshPro para exibir a porcentagem de movimento

    private bool isTracking = false;
    private float elapsedTime = 0f;
    private Vector3 cameraStartPosition;
    private float totalDistance = 0f;
    private Transform playerHead;

    private void Update()
    {
        bool isPlayerStanding = IsPlayerStanding();
        startButton.interactable = !isTracking && isPlayerStanding; // Habilita ou desabilita o botão de início

        if (isTracking)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= interactionDuration)
            {
                float percentageMoved = 100f - (totalDistance / interactionDuration) * 100f;
                percentageText.text = $"Porcentagem de movimento: {percentageMoved:F2}%";

                elapsedTime = 7f; // Reinicializa o tempo decorrido para evitar que o cálculo seja executado novamente
                // totalDistance = 0f; // Comentado para não reiniciar a distância total a cada interação
                isTracking = true; // Isso não altera o estado de rastreamento, pois já está definido como true
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

    private bool IsPlayerStanding()
    {
        float playerHeadY = transform.position.y;

        return playerHeadY >= playerHeight; // Retorna true se a posição Y da cabeça do jogador for maior ou igual à altura mínima
    }

    public void OnButtonClick()
    {
        isTracking = true; // Define o estado de rastreamento como true
        cameraStartPosition = Camera.main.transform.position;
        elapsedTime = 0f;
        totalDistance = 0f;
        percentageText.text = "Calculando..."; // Define um texto temporário enquanto a porcentagem está sendo calculada
    }
}
