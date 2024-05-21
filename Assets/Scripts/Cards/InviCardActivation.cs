using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InviCardActivation : MonoBehaviour
{
    [SerializeField] private float inviDuration = 5f; // Duration of the invisibility effect
    [SerializeField] private float cooldownDuration = 10f; // Cooldown duration for invisibility
    [SerializeField] private Image inviCooldownFillImage; // Reference to the fill image for cooldown
    private bool isCooldown = false;
    private bool isEffectActive = false; // Flag to track if the invisibility effect is active
    private float inviStartTime;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Activate invisibility card when E key is pressed and the effect is not already active and not on cooldown
        if (Input.GetKeyDown(KeyCode.E) && CardManager.instance.IsInviCardActivated() && !isEffectActive && !isCooldown)
        {
             print("activating invisibility");
            ActivateInvisibility();
        }
    }

    private void ActivateInvisibility()
    {
        print("activating invisibility");
        // Only activate if not already active and not on cooldown
        if (!isEffectActive && !isCooldown)
        {
            // Set the invisibility effect active
            isEffectActive = true;
            playerController.SetInvisible(true);
            inviStartTime = Time.time;

            // Start the effect duration coroutine
            StartCoroutine(InvisibilityEffect());
        }
    }

    private IEnumerator InvisibilityEffect()
    {
        yield return new WaitForSeconds(inviDuration);

        // Effect duration over, deactivate invisibility
        playerController.SetInvisible(false);
        isEffectActive = false;

        // Start cooldown coroutine
        StartCoroutine(InvisibilityCooldown());
    }

    private IEnumerator InvisibilityCooldown()
    {
        isCooldown = true;

        // Activate cooldown fill image
        if (inviCooldownFillImage != null)
        {
            inviCooldownFillImage.gameObject.SetActive(true);
            float fillAmount = 1f; // Start with full fill
            float fillChangeRate = 1f / cooldownDuration; // Rate at which fill decreases per second

            while (fillAmount > 0f)
            {
                fillAmount -= fillChangeRate * Time.deltaTime;
                inviCooldownFillImage.fillAmount = fillAmount;
                yield return null; // Wait for the next frame
            }

            // Cooldown finished, deactivate cooldown fill image
            inviCooldownFillImage.gameObject.SetActive(false);
        }

        // Cooldown finished, allow activation again
        isCooldown = false;
    }
}