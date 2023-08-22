using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect03 : MonoBehaviour
{
    private Animator attackEffect = default;

    public SpriteRenderer effectRenderer;

    private bool effectOn = false;

    void Awake()
    {
        attackEffect = GetComponent<Animator>();
        effectRenderer = GetComponent<SpriteRenderer>();
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
