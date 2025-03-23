using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkHandler : MonoBehaviour
{
    public static NetworkHandler Instance { get; private set; }

    public string serverUrl = "https://wadahub.manerai.com/api/inventory/status";
    private string bearerToken = "kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SendInventoryEvent(string itemId, string eventType)
    {
        StartCoroutine(SendEventCoroutine(itemId, eventType));
    }

    private IEnumerator SendEventCoroutine(string itemId, string eventType)
    {
        InventoryEventData data = new InventoryEventData { itemID = itemId, eventName = eventType };
        string jsonPayload = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(serverUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + bearerToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Network response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Network error: " + request.error);
        }
    }
}

[System.Serializable]
public class InventoryEventData
{
    public string itemID;
    public string eventName;
}
