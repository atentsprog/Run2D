using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigid;
    public Vector2 jumpForce = new Vector2(0, 1000);
    public float gravityScale = 7;
    void Start()
    {
        //animator = transform.Find("Sprite").GetComponent<Animator>();
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = gravityScale;
        referenceTransform = Camera.main.transform;
        defaultOffsetX = referenceTransform.position.x - transform.position.x;
    }

    public void OnStageEnd()
    {
        animator.Play("Idle");
    }

    public float speed = 20;
    public float midAirVelocity = 10;
    void Update()
    {
        if (RunGameManager.instance.gameState != RunGameManager.GameStateType.Playing)
            return;

        transform.Translate(speed * Time.deltaTime, 0, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.velocity = Vector2.zero;
            rigid.AddForce(jumpForce);
        }

        //자석 효과 끄기/켜기 테스트
        if (Input.GetKeyDown(KeyCode.Alpha1))
            transform.Find("Magnet").gameObject.SetActive(false);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            transform.Find("Magnet").gameObject.SetActive(true);

        float velocity = rigid.velocity.y;
        float absVelocity = Mathf.Abs(velocity);
        //float absVelocity = velocity > 0 ? velocity : -velocity;
        //float absVelocity = velocity;
        //if (absVelocity > 0)
        //    absVelocity = -velocity;

        //string animationName = "";
        string animationName = string.Empty;
        if (IsGround())
        {
            animationName = "Run";
        }
        else
        {
            if (absVelocity < midAirVelocity) 
                animationName = "Jump_MidAir";
            else if (velocity > 0) 
                animationName = "Jump_Up";               //상승. 
            else//하락
            {
                animationName = "Jump_Fall";                
            }
        }
        animator.Play(animationName);



        // X위치가 기준값보다 뒤쳐져 있다면 기준값 위치로 부드럽게 이동시키자.
        float offset = GetInvalidOffsetXFromInitPos();
        if (IsInvalidPosX(offset))
        {
            //if (IsGround() == false) // 점프중에만 이동시키려면 여기를 주석 풀자.
            {
                transform.Translate(offset * restorePosSpeed * Time.deltaTime, 0, 0);
            }
        }
    }


    public Transform referenceTransform;
    public float defaultOffsetX;
    public float restorePosSpeed = 2f;
    public float allowPosX = 0.2f;
    float GetInvalidOffsetXFromInitPos()
    {
        float diff = referenceTransform.position.x - transform.position.x - defaultOffsetX;
        float absDiff = Mathf.Abs(diff);
        return absDiff;
    }
    private bool IsInvalidPosX(float absDiff)
    {
        return absDiff > allowPosX;
    }


    public Transform rayStart;
    public float rayCheckDistance = 0.1f;
    public LayerMask groundLayer;
    private bool IsGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayStart.position, Vector2.down, rayCheckDistance, groundLayer);
        return hit.transform != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(rayStart.position, Vector2.down * rayCheckDistance);
    }
}
