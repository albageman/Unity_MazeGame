using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    float vAxis;
    float hAxis;
    bool runAxis;
    Vector3 movVec = new Vector3();
    Animator anim = new Animator();

    void Awake()
    {
        anim = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        vAxis = Input.GetAxisRaw("Vertical");
        hAxis = Input.GetAxisRaw("Horizontal");
        runAxis = Input.GetButton("Run");
        movVec = new Vector3(hAxis, 0 , vAxis).normalized;  // 대각선방향 속도문제 제어
        transform.position += movVec * speed * Time.deltaTime * (runAxis ? 1f : 0.3f); // 프레임과 상관없이 움직임 제어(Time.deltaTime)
        transform.LookAt(transform.position + movVec);

        anim.SetBool("IsWalk", movVec != Vector3.zero);
        anim.SetBool("IsRun", runAxis);
    }
}
