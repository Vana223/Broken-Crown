using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    public float smoothSpeed;

    private float targetValue;

    public void SetMaxStamina(int stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;
        targetValue = stamina;
    }

    public void SetStamina(float stamina)
    {
        targetValue = stamina;
    }

    void Update()
    {
        slider.value = Mathf.Lerp(slider.value, targetValue, smoothSpeed * Time.deltaTime);
    }
}
