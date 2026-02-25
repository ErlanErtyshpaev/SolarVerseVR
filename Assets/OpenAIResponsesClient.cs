using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class OpenAIResponsesClient
{
    private const string ENDPOINT = "https://api.openai.com/v1/responses";

    public static async Task<string> AskAsync(
        string apiKey,
        string model,
        string instructions,
        string input
    )
    {
        string json = $@"
{{
  ""model"": ""{Escape(model)}"",
  ""instructions"": ""{Escape(instructions)}"",
  ""input"": ""{Escape(input)}""
}}";

        using UnityWebRequest req = new UnityWebRequest(ENDPOINT, "POST");
        req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();

        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Authorization", $"Bearer {apiKey}");

        var op = req.SendWebRequest();
        while (!op.isDone)
            await Task.Yield();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"HTTP ERROR: {req.responseCode}\n{req.error}\n{req.downloadHandler.text}");
            return $"Ошибка ИИ ({req.responseCode})";
        }

        return ExtractOutputText(req.downloadHandler.text);
    }

    [System.Serializable] private class ResponseRoot { public Output[] output; }
    [System.Serializable] private class Output { public Content[] content; }
    [System.Serializable] private class Content { public string type; public string text; }

    private static string ExtractOutputText(string json)
    {
        try
        {
            var root = JsonUtility.FromJson<ResponseRoot>(json);
            if (root?.output != null && root.output.Length > 0 && root.output[0].content != null)
            {
                foreach (var c in root.output[0].content)
                {
                    if (c != null && (c.type == "output_text" || c.type == "text") && !string.IsNullOrEmpty(c.text))
                        return c.text;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

        Debug.LogWarning("Не удалось извлечь текст. Raw:\n" + json);
        return "ИИ ответил, но текст не найден.";
    }

    private static string Escape(string s)
    {
        if (s == null) return "";
        return s.Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", "\\n")
                .Replace("\r", "");
    }
}
