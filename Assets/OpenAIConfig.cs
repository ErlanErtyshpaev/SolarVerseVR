using UnityEngine;

[CreateAssetMenu(menuName = "Config/OpenAI Config", fileName = "OpenAIConfig")]
public class OpenAIConfig : ScriptableObject
{
    [TextArea(2, 5)]
    public string apiKey;

    public string model = "gpt-4.1-mini";
}
