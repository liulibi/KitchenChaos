using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private const string Is_Walking = "IsWalking";
    private Animator animator;
    [SerializeField] Player player;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(Is_Walking, player.IsWalking());    
    }
}
