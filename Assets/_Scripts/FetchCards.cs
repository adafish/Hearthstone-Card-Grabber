using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Retrieves cards matching card request format using 
/// Hearthstone API.
/// </summary>
public class FetchCards : MonoBehaviour
{
    public delegate void OnCardsFetched(int numCards);
    public static event OnCardsFetched CardsFetched;

    [SerializeField]
    private CardRequestFormat cardRequest;

    private Card[] cards;

    private IEnumerator Start()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(cardRequest.Url))
        {
            request.SetRequestHeader(cardRequest.HostHeaderName, cardRequest.HostHeaderValue);
            request.SetRequestHeader(cardRequest.ApiKeyName, cardRequest.ApiKeyValue);

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                byte[] result = request.downloadHandler.data;
                string cardJSON = System.Text.Encoding.Default.GetString(result);
                cardJSON = JsonHelper.FixJson(cardJSON);
                cards = JsonHelper.FromJson<Card>(cardJSON);
                if (CardsFetched != null)
                    CardsFetched(GetNumberOfCards());
            }    
        }
    }

    public Card GetCard(int cardIndex)
    {
        if (cards == null)
        {
            Debug.Log("Error: Cards not found");
            return null;
        }

        if (cardIndex >= cards.Length)
        {
            Debug.Log(string.Format("Error: Card at {0} is not found", cardIndex));
            return null;
        }

        return cards[cardIndex];
    }

    public int GetNumberOfCards()
    {
        if (cards == null)
        {
            Debug.Log("Error: Cards not found");
            return -1; //error flag
        }

        return cards.Length;
    }
}
