using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    Bank bank;
    TextMeshProUGUI goldAmount;
    // Start is called before the first frame update
    void Start()
    {
        bank = FindObjectOfType<Bank>();
        goldAmount = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayGoldAmountText();
    }

    void DisplayGoldAmountText()
    {
        int currentGold = bank.CurrentBalance;
        goldAmount.text = "Gold: " + currentGold.ToString();
    }
}
