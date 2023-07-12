using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorsUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider coinsBar;

    private void Start()
    {
        Player.OnPlayerHealthChanged += Player_OnPlayerHealthChanged;
        Player.OnPlayerCollectCoin += Player_OnPlayerCollectCoin;
    }

    private void Player_OnPlayerCollectCoin(int coinsCount)
    {
        coinsBar.value = coinsCount;
    }

    private void Player_OnPlayerHealthChanged(float health)
    {
        healthBar.value = health;
    }

    private void OnDestroy()
    {
        Player.OnPlayerHealthChanged -= Player_OnPlayerHealthChanged;
        Player.OnPlayerCollectCoin -= Player_OnPlayerCollectCoin;
    }
}
