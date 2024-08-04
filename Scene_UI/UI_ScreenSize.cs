using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ScreenSize : MonoBehaviour
{
    private Vector2 previousScreenSize;
    public Vector3 initialPosition;
    public Vector3 initialScale;

    void Start()
    {
        previousScreenSize = new Vector2(Screen.width, Screen.height);
        initialPosition = transform.position;
        initialScale = transform.localScale;

        UI_ScaleUpdate();
    }

    void Update()
    {
        UI_ScaleUpdate();
    }

    void UpdateUI()
    {
        if (Screen.width != previousScreenSize.x || Screen.height != previousScreenSize.y)
        {
            previousScreenSize.x = Screen.width;
            previousScreenSize.y = Screen.height;

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            float currentRatio = screenWidth / screenHeight;

            float newSizeX = initialScale.x * currentRatio;
            float newSizeY = initialScale.y * currentRatio;
            float sizeZ = 1.0f;

            transform.localScale = new Vector3(newSizeX, newSizeY, sizeZ);
        }

    }

    void UI_ScaleUpdate()
    {
        if (Screen.width != previousScreenSize.x || Screen.height != previousScreenSize.y)
        {
            // 변경된 화면 크기를 저장
            previousScreenSize.x = Screen.width;
            previousScreenSize.y = Screen.height;


            // 현재 화면의 가로, 세로 비율 계산 (예: 1920x1080 화면)
            float screenWidth = 1920f;
            float screenHeight = 1080f;
            float currentAspect = screenWidth / screenHeight;

            // 원하는 비율 (예: 16:9)
            float targetAspect = 16f / 9f;

            // 현재 비율과 원하는 비율의 차이 계산
            float ratio = currentAspect / targetAspect;

            // 스케일 값 조정
            Vector3 newScale = initialScale;
            newScale.x *= ratio; // 가로 비율 조정

            // 포지션 값 조정
            Vector3 newPosition = initialPosition;
            newPosition.x *= ratio; // 가로 비율 조정

            // Quad의 스케일과 포지션 업데이트
            transform.localScale = newScale;
            transform.position = newPosition;
        }
    }

}

