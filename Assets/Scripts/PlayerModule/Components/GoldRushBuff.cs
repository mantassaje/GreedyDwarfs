using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GoldRushBuff : MonoBehaviour
{
    public float GoldRushSpeedAdd = 15;
    public bool IsGoldRushActive = false;
    public float GoldRushSeconds = 8f;

    private float _originalSpeed;

    private Moving _moving;
    private Inventory _inventory;

    private void Awake()
    {
        _moving = GetComponent<Moving>();
        _inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        _originalSpeed = _moving.MaxSpeed;
    }

    private void Update()
    {
        if (IsGoldRushActive)
        {
            _moving.MaxSpeed = _originalSpeed + GoldRushSpeedAdd;

            if (!_inventory.HasGold)
            {
                StopGoldRush();
            }
        }
        else
        {
            _moving.MaxSpeed = _originalSpeed;
        }
    }

    public void StartGoldRush()
    {
        IsGoldRushActive = true;
        CancelInvoke();
        Invoke(nameof(StopGoldRush), GoldRushSeconds);
    }

    public void StopGoldRush()
    {
        IsGoldRushActive = false;
    }
}
