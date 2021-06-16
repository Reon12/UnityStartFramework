using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotBackground : MonoBehaviour
{
    [SerializeField] private RectTransform _Panel_QuickSlot;

    [SerializeField] private Image _QuickSlotBackground;

    [SerializeField] private TextMeshProUGUI _TMP_CoolTime;


    public Image quickSlotImage => _QuickSlotBackground;

    public TextMeshProUGUI coolTime => _TMP_CoolTime;


    private void Awake()
    {
        
    }
    public void SetText(string text)
    {
        coolTime.text = text;
    }

    public void SetFillamount(float amount)
    {
        quickSlotImage.fillAmount = amount;
    }
}
