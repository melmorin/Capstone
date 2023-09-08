using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerController playerCont; 
    [SerializeField] private Image fillImage;
    private Slider slider; 
    private float fillValue; 

    // Start is called before the first frame update
    private void Start()
    {
        slider = GetComponent<Slider>();
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        fillValue = playerCont.currentHealth / playerCont.maxHealth; 

        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false; 
        }
        if (slider.value > slider.minValue && !fillImage.enabled)
        {
            fillImage.enabled = true; 
        }

        slider.value = fillValue; 
    }
}
