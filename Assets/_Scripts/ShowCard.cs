using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>
/// Displays cards fetched by card fetcher.
/// </summary>
public class ShowCard : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private RawImage cardImage;
    [SerializeField]
    private TextMeshProUGUI cardNameDisplay;
    [SerializeField]
    private TextMeshProUGUI cardTextDisplay;
    [SerializeField]
    private TextMeshProUGUI cardInPlayTextDisplay;
    [SerializeField]
    private TextMeshProUGUI cardFlavorTextDisplay;
    [SerializeField]
    private TextMeshProUGUI cardNumberDisplay;
    [SerializeField]
    private Button[] progressButtons;

    [Header("References")]
    [SerializeField]
    private Texture blankCardImage;
    [SerializeField]
    private FetchCards cardFetcher;

    private int currentCard = -1;
    private int numberOfCards = -1;
    private string cardNumberDisplayFormat = "{0} / {1}";
    private Coroutine showCardRoutine;

    private void OnEnable()
    {
        for (int i = 0; i < progressButtons.Length; i++)
        {
            progressButtons[i].interactable = false;
        }

        FetchCards.CardsFetched += InitializeCardDisplay;
    }

    private void OnDisable()
    {
        FetchCards.CardsFetched -= InitializeCardDisplay;
    }

    public void InitializeCardDisplay(int numberCards)
    {
        if (numberCards != -1)
        {
            for (int i = 0; i < progressButtons.Length; i++)
            {
                progressButtons[i].interactable = true;
            }

            currentCard = 0;
            numberCards = numberOfCards;
            DisplayNextCard(0);
        }
    }

    private IEnumerator DisplayCard(int cardIndex)
    {
        Card selectedCard = cardFetcher.GetCard(cardIndex);
        //cards may have normal image, gold image, or no image
        string imgUrl = (string.IsNullOrWhiteSpace(selectedCard.img) ? selectedCard.imgGold : selectedCard.img);

        //cards without images will have empty img fields
        if (!string.IsNullOrWhiteSpace(imgUrl))
        {
            UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(imgUrl);

            yield return imageRequest.SendWebRequest();

            if (imageRequest.isNetworkError || imageRequest.isHttpError)
            {
                Debug.Log("Error: " + imageRequest.error);
                cardImage.texture = blankCardImage;
            }
            else
            {
                cardImage.texture = DownloadHandlerTexture.GetContent(imageRequest);
                cardImage.texture.filterMode = FilterMode.Point;
            }

        }
        else
        {
            cardImage.texture = blankCardImage;
        }

        cardNameDisplay.text = selectedCard.name;
        cardTextDisplay.text = selectedCard.text;
        cardInPlayTextDisplay.text = selectedCard.inPlayText;
        cardFlavorTextDisplay.text = selectedCard.flavor;
        ShowCardNumber();
    }

    public void DisplayNextCard(int direction)
    {
        int numCards = cardFetcher.GetNumberOfCards();
        currentCard = ((currentCard + direction) % numCards + numCards) % numCards;
        if (showCardRoutine != null)
        {
            StopCoroutine(showCardRoutine);
        }

        showCardRoutine = StartCoroutine(DisplayCard(currentCard));
    }

    public void ShowCardNumber()
    {
        cardNumberDisplay.text = string.Format(cardNumberDisplayFormat, currentCard + 1, cardFetcher.GetNumberOfCards());
    }
}
