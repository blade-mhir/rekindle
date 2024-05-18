using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    private bool dashCardActivated = false;
    private bool inviCardActivated = false;
    private bool laserCardActivated = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsDashCardActivated() => dashCardActivated;
    public bool IsInviCardActivated() => inviCardActivated;
    public bool IsLaserCardActivated() => laserCardActivated;

    public void ActivateDashCard()
    {
        DeactivateAllCards();
        dashCardActivated = true;
    }

    public void ActivateInviCard()
    {
        DeactivateAllCards();
        inviCardActivated = true;
    }

    public void ActivateLaserCard()
    {
        DeactivateAllCards();
        laserCardActivated = true;
    }

    public void DeactivateAllCards()
    {
        dashCardActivated = false;
        inviCardActivated = false;
        laserCardActivated = false;
    }
    
    public void ResetCardState()
    {
        DeactivateAllCards();
    }
}
