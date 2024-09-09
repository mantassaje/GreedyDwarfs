using UnityEngine;

public class Cargo : MonoBehaviour, IInteractable
{
    private Blink Blink { get; set; }

    private RulesController RulesController { get; set; }

    void Awake()
    {
        RulesController = FindObjectOfType<RulesController>();
        Blink = GetComponent<Blink>();
    }

    public bool Interact(InteractActor actor)
    {
        RulesController.AddGoldToTotal();

        Blink.BlinkOnce();

        return true;
    }
}
