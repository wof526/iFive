using UnityEngine;

public static class JoystickUtils
{

    public static Vector3 TouchPosition(this Canvas _Canvas,int touchID)
    {
        // _Canvas는 터치 위치를 계산할 canvas, touchID는 모바일에서의 터치 처리
        Vector3 Return = Vector3.zero;

        if (_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {   // 화면 좌표계 그대로 사용
#if UNITY_ANDROID && !UNITY_EDITOR  // 안드로이드에서는 터치 위치, 다른 플랫폼에서는 마우스 위치 가져오기
            Return = Input.GetTouch(touchID).position;
#else
            Return = Input.mousePosition;
#endif
        }
        else if (_Canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {   // 터치, 마우스 위치를 로컬 좌표계로 변환
            Vector2 tempVector = Vector2.zero;
#if UNITY_ANDROID && !UNITY_EDITOR
           Vector3 pos = Input.GetTouch(touchID).position;
#else
            Vector3 pos = Input.mousePosition;
#endif
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_Canvas.transform as RectTransform, pos, _Canvas.worldCamera, out tempVector);
            // 변환된 로컬 좌표를 월드 좌표로 변환하여 저장
            Return = _Canvas.transform.TransformPoint(tempVector);
        }

        return Return;
    }
}