using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Renderer))]
public class BallColorUpdater : MonoBehaviour
{
    [Header("API Settings")]
    [Tooltip("Python服务地址，例如：http://localhost:5000")]
    public string apiUrl = "http://localhost:5000/color";
    
    [Tooltip("颜色更新间隔（秒）")]
    [Range(0.5f, 2f)] public float updateInterval = 1f;

    private Renderer targetRenderer;
    private Coroutine updateCoroutine;

    void Awake()
    {
        targetRenderer = GetComponent<Renderer>();
        targetRenderer.material = new Material(Shader.Find("Standard"));
    }

    void OnEnable() => updateCoroutine = StartCoroutine(ColorUpdateLoop());
    void OnDisable() => StopCoroutine(updateCoroutine);

    private IEnumerator ColorUpdateLoop()
    {
        while (true)
        {
            yield return FetchColorFromServer();
            yield return new WaitForSeconds(updateInterval);
        }
    }

    private IEnumerator FetchColorFromServer()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"更新失败: {request.error}");
                yield break;
            }

            if (TryParseColorResponse(request.downloadHandler.text, out Color newColor))
            {
                targetRenderer.material.color = newColor;
            }
        }
    }

    private bool TryParseColorResponse(string json, out Color color)
    {
        color = Color.white;
        
        try
        {
            var response = JsonUtility.FromJson<ColorResponse>(json);
            return HexToColor(response.color, out color);
        }
        catch
        {
            return false;
        }
    }

    private bool HexToColor(string hex, out Color color)
    {
        hex = hex.TrimStart('#');
        color = Color.white;

        if (hex.Length != 6) return false;

        try
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            color = new Color32(r, g, b, 255);
            return true;
        }
        catch
        {
            return false;
        }
    }

    [System.Serializable]
    private class ColorResponse
    {
        public string color;
    }
}