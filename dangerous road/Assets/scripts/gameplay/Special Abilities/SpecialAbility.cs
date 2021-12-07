using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public abstract class SpecialAbility
{
    [SerializeField] protected Button button;
    [SerializeField] protected int maxPoints = 3;
    [SerializeField] protected float duration = 5;

    private int _curPoints;
    protected int CurPoints {
        get => _curPoints; 
        set
        {
            _curPoints = value;
            button.interactable = _curPoints >= maxPoints;
        }
    }

    public virtual void Init()
    {
        CurPoints = maxPoints;
        button.onClick.AddListener(ApplyButton);
    }   

    protected abstract IEnumerator Ability(float duration);   

    protected bool CanApplyAbility() => CurPoints >= maxPoints;

    protected void ApplyButton()
    {
        if (CanApplyAbility())
        {
            CarSpawnManager.SpawnedCar.StartCoroutine(Ability(duration));
            CurPoints = 0;
        }            
    } 
}

