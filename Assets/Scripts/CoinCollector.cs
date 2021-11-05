using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private Text _counterText;
    [SerializeField] private int _coinAmount;

    private void Start()
    {
        _coinAmount = 0;
    }

    private void Update()
    {
        _counterText.text = "Coins : " + _coinAmount;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            _coinAmount += 1;
        }
    }
}
