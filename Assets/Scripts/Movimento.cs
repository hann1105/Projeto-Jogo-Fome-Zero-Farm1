using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimento : MonoBehaviour
{
    public float speed;

    public Animator animator;

    private void Update()
    {
        float horizontal=Input.GetAxisRaw("Horizontal");
        float vertical=Input.GetAxisRaw("Vertical");

        Vector3 direcao=new Vector3(horizontal,vertical);

        AnimateMovement(direcao);

        transform.position += direcao*speed *Time.deltaTime;  
    }

    void AnimateMovement(Vector3 direcao)
    {
        if(animator!= null)
        {
            if(direcao.magnitude>0)
            {
                animator.SetBool("isMoving",true);

                animator.SetFloat("horizontal",direcao.x);
                animator.SetFloat("vertical",direcao.y);
            }
            else
            {
                animator.SetBool("isMoving",false);
            }
        }
    }
}
