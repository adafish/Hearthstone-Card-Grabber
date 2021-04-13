using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardRequest", menuName = "ScriptableObjects/CardRequestFormatObject", order = 1)]
public class CardRequestFormat : ScriptableObject
{
    [SerializeField]
    private string url = "https://omgvamp-hearthstone-v1.p.rapidapi.com/cards/";
    public string Url { get { return url; } }
    [SerializeField]
    private string hostHeaderName = "APIHOST";
    public string HostHeaderName { get { return hostHeaderName; } }
    [SerializeField]
    private string hostHeaderValue = "";
    public string HostHeaderValue { get { return hostHeaderValue; } }
    [SerializeField]
    private string apiKeyName = "APIKEY";
    public string ApiKeyName { get { return apiKeyName; } }
    [SerializeField]
    private string apiKeyValue = "";
    public string ApiKeyValue { get { return apiKeyValue; } }
}
