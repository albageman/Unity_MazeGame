using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
   [SerializeField] private Transform target;         // 따라갈 대상 (Player)
    [SerializeField] private Transform cameraTransform; // 실제 카메라 (자식)
    [SerializeField] private float distance = 4.0f;    
    [SerializeField] private float height = 2.0f;
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private LayerMask collisionMask;  // 벽 감지용
    [SerializeField] private float smoothSpeed = 10f;  // Lerp 속도

    private float currentX = 0f;
    private float currentY = 0f;
    [SerializeField] private float minY = -40f;
    [SerializeField] private float maxY = 80f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // 마우스 입력으로 회전 각도 계산
        currentX += Input.GetAxis("Mouse X") * rotationSpeed;
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        currentY = Mathf.Clamp(currentY, minY, maxY);

        // 회전값 → 위치 방향 계산
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 direction = rotation * new Vector3(0, 0, -distance);
        Vector3 desiredPosition = target.position + Vector3.up * height + direction;

        // 벽 충돌 감지
        RaycastHit hit;
        Vector3 origin = target.position + Vector3.up * height;
        if (Physics.Linecast(origin, desiredPosition, out hit, collisionMask))
        {
            desiredPosition = hit.point; // 벽에 막히면 거기서 멈춤
        }

        // 🚀 부드러운 이동 (Lerp)
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, Time.deltaTime * smoothSpeed);

        // 카메라가 항상 타겟을 바라봄
        cameraTransform.LookAt(origin);
    } 
}
// {
//     [SerializeField] Transform target;

//     [SerializeField] float followSpeed = 10f;
//     [SerializeField] float sensitivity = 100f;
//     [SerializeField] float clampAngle = 70f;
    
//     private float rotX;
//     private float rotY;

//     [SerializeField] Transform realCamera;
//     [SerializeField] Vector3 dirNormalized;
//     [SerializeField] Vector3 finalDir;
//     [SerializeField] float minDistance = 3f;
//     [SerializeField] float maxDistance = 1f;
//     [SerializeField] float finalDistance;
//     [SerializeField] float smoothness = 10f;
        

//     // Start is called before the first frame update
//     void Start()
//     {
//         rotX = transform.localRotation.eulerAngles.x;
//         rotY = transform.localRotation.eulerAngles.y;

//         dirNormalized = realCamera.localPosition.normalized; //찐카메라의 방향
//         finalDistance = realCamera.localPosition.magnitude;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         rotX += Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;
//         rotY += Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;

//         rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
//         Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
//         transform.rotation = rot;
//         Vector3 desiredPosition = transform.position;

//         RaycastHit hit;
//         if(Physics.Linecast(target.position, desiredPosition, out hit))
//         {
//             desiredPosition = hit.point;
//         }

//         realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, desiredPosition, smoothness * Time.deltaTime);

//         realCamera.LookAt(target);
//     }
// }
