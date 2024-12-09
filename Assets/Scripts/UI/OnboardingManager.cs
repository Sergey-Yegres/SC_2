using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingManager : MonoBehaviour
{
    [SerializeField] private GameObject onboardingGM;
    private bool onboardingState;
    private const string onboarding = "onboarding";
    private void Awake()
    {
        if (PlayerPrefs.HasKey(onboarding))
        {
            onboardingGM.SetActive(false);
        }
        else
        {
            onboardingGM.SetActive(true);
        }
    }
    public void ChangeOnboardingState()
    {
        PlayerPrefs.SetInt(onboarding, 1);
        onboardingGM.SetActive(false);
    }
}
