using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    public int maxStamina;
    public float currentStamina;
    public float regenRate;
    public float regenDelay;

    public StaminaBar staminaBar;

    private float lastStaminaUseTime;

    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
        staminaBar.SetStamina(Mathf.RoundToInt(currentStamina));
    }

    void Update()
    {
        if (Time.time - lastStaminaUseTime >= regenDelay && currentStamina < maxStamina)
        {
            currentStamina += regenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
            staminaBar.SetStamina(Mathf.RoundToInt(currentStamina));
        }
    }

    public bool UseStamina(int amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            staminaBar.SetStamina(Mathf.RoundToInt(currentStamina));
            lastStaminaUseTime = Time.time;
            return true;
        }
        return false;
    }

    public bool IsOutOfStamina()
    {
        return currentStamina <= 0;
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    public void RestoreStamina(int amount)
    {
        currentStamina = Mathf.Min(currentStamina + amount, maxStamina);
        staminaBar.SetStamina(Mathf.RoundToInt(currentStamina));
    }
}
