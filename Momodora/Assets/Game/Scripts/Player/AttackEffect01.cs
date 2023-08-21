using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect01 : MonoBehaviour
{
    private Animator attackEffect = default;

    private bool effectOn = false;

    void Awake()
    {
        attackEffect = GetComponent<Animator>();
    }

    void OnEnable()
    {
        effectOn = true;
        attackEffect.SetBool("EffectOn", effectOn);
    }

    void OnDisable()
    {
        effectOn = false;
        attackEffect.SetBool("EffectOn", effectOn);
    }
}
