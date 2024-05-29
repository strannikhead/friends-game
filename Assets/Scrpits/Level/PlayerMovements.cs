using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (player != null)
        {
            if (player.isGrounded && !player.isLeftBlocked)
                StartWalkingAnimation();
            else
                StopWalkingAnimation();
        }
    }
    
    private void StartWalkingAnimation()
    {
        // review(26.05.2024): Я бы брал текущий layerIndex у gameObject игрока
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            animator.Play("Movement");
    }


    private void StopWalkingAnimation()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            animator.Play("Movement", -1, 0f);
    }
}