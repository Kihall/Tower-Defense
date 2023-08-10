using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private int startingGoldAmount;

    private int currentGoldAmount;

    private void Awake()
    {
        Instance = this;

        currentGoldAmount = startingGoldAmount;

        UpdateGoldText();
    }

    public void UpdateGoldText()
    {
        goldText.text = " " + currentGoldAmount;
    }

    public void AddGold(int goldAmount)
    {
        currentGoldAmount += goldAmount;
        UpdateGoldText();
    }

    public bool CanAfford(int goldAmount)
    {
        if (currentGoldAmount >= goldAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SpendGold(int goldAmount)
    {
        currentGoldAmount -= goldAmount;
        UpdateGoldText();
    }
}
