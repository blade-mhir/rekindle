using System.Collections;
using UnityEngine;

public class InviCardActivation : MonoBehaviour
{
    [SerializeField] private float inviDuration = 5f; // Duration of the invisibility effect
    [SerializeField] private float cooldownDuration = 10f; // Cooldown duration for invisibility
    private bool isCooldown = false;
    private float inviStartTime;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Activate invisibility card only if it's not already active and not on cooldown
        if (Input.GetKeyDown(KeyCode.E) && !playerController.IsInvisible() && !isCooldown)
        {
            ActivateInvisibility();
        }
    }

    private void ActivateInvisibility()
    {
        if (!CardManager.instance.IsDashCardActivated() && !CardManager.instance.IsLaserCardActivated())
        {
            CardManager.instance.ActivateInviCard();
            playerController.SetInvisible(true);
            inviStartTime = Time.time;
            StartCoroutine(InvisibilityCooldown());
        }
    }

    private IEnumerator InvisibilityCooldown()
    {
        yield return new WaitForSeconds(inviDuration);
        playerController.SetInvisible(false);
        yield return new WaitForSeconds(cooldownDuration);
        isCooldown = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!CardManager.instance.IsDashCardActivated() && !CardManager.instance.IsLaserCardActivated() && collision.gameObject.CompareTag("InviCard"))
        {
            CardManager.instance.ActivateInviCard();
            ActivateInvisibility();
            Destroy(collision.gameObject); // Destroy the power-up on collision
        }
    }
}
