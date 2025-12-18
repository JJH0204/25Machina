using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class MobileDragCamera : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public float sensitivity = 0.15f;   // 드래그 감도
    public bool useRightHalfOnly = true; // true면 화면 오른쪽 절반만 카메라 입력

    private CinemachinePOV _pov;

    private void Awake()
    {
        if (vcam == null)
            vcam = GetComponent<CinemachineVirtualCamera>();

        if (vcam != null)
            _pov = vcam.GetCinemachineComponent<CinemachinePOV>();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        // PC / 에디터에서는 이 스크립트는 안 쓰고, 기존 마우스/패드 입력만 사용
#if UNITY_EDITOR || UNITY_STANDALONE
        return;
#endif
        if (_pov == null)
            return;

        // 활성 손가락들 중에서 "카메라용"으로 쓸 손가락 하나 찾기
        foreach (var finger in Touch.activeFingers)
        {
            var touch = finger.currentTouch;

            // 움직인 상태가 아니면 무시
            if (touch.phase != UnityEngine.InputSystem.TouchPhase.Moved)
                continue;

            // 화면 오른쪽 절반만 카메라 입력으로 사용하고 싶다면
            if (useRightHalfOnly && touch.screenPosition.x < Screen.width * 0.5f)
                continue;

            // 드래그 델타 → 카메라 회전
            Vector2 delta = touch.delta * sensitivity;

            _pov.m_HorizontalAxis.Value += delta.x;
            _pov.m_VerticalAxis.Value -= delta.y; // 위로 드래그하면 카메라가 위로 가게 부호 반전

            // 카메라에 쓸 손가락은 하나만 사용
            break;
        }
    }
}
