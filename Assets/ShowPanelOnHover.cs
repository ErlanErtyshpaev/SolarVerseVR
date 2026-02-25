using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;


public class ShowPanelOnHover : MonoBehaviour
{
    [Header("What to show/hide")]
    public GameObject panelRoot;

    [Header("UI")]
    public TMP_InputField questionInput;
    public TMP_Text answerText;
    public Button askButton;

    [Header("OpenAI Online")]
    public OpenAIConfig openAIConfig;
    public string planetName = "Марс";
    [TextArea(2, 6)]
    public string planetFacts = "Ты — Марс. Отвечай от первого лица, строго по фактам, кратко.";

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;
    private bool isHovering = false;
    private bool isShown = false;

    private bool lastTriggerPressed = false;
    private InputDevice rightHand;

    void Awake()
    {
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        if (panelRoot) panelRoot.SetActive(false);

        if (askButton != null)
            askButton.onClick.AddListener(OnAskClicked);
    }

    void OnEnable()
    {
        if (interactable != null)
        {
            interactable.hoverEntered.AddListener(_ => isHovering = true);
            interactable.hoverExited.AddListener(_ => isHovering = false);
        }

        TryGetRightHand();
    }

    void Update()
    {
        if (!rightHand.isValid) TryGetRightHand();

        if (!isHovering || !rightHand.isValid)
        {
            lastTriggerPressed = false;
            return;
        }

        bool triggerPressed = false;
        rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed);

        // toggle panel on trigger press (edge)
        if (triggerPressed && !lastTriggerPressed)
        {
            isShown = !isShown;
            if (panelRoot) panelRoot.SetActive(isShown);
        }

        lastTriggerPressed = triggerPressed;
    }

    private void TryGetRightHand()
    {
        rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    private async void OnAskClicked()
    {
        if (questionInput == null || answerText == null || askButton == null) return;

        string q = questionInput.text;
        if (string.IsNullOrWhiteSpace(q))
        {
            answerText.text = "Задай вопрос 🙂";
            return;
        }

        if (openAIConfig == null || string.IsNullOrWhiteSpace(openAIConfig.apiKey))
        {
            answerText.text = "Нет OpenAIConfig или API key.";
            return;
        }

        askButton.interactable = false;
        answerText.text = "Думаю…";

        string systemPrompt =
            $"Ты планета {planetName}. Отвечай от первого лица, дружелюбно, но строго по фактам. " +
            $"Если не уверен — честно скажи. Коротко.\n\nФакты:\n{planetFacts}";

        string reply = await OpenAIResponsesClient.AskAsync(
            openAIConfig.apiKey,
            openAIConfig.model,
            systemPrompt,
            q.Trim()
        );

        answerText.text = reply;
        askButton.interactable = true;
    }
}
