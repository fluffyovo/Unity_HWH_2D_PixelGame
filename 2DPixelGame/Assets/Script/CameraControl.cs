﻿using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("攝影機速度"), Range(0, 50)]
    public float speed = 1.5f;
    [Header("上下邊界")]
    public Vector2 limitY = new Vector2(-5, 5);
    [Header("左右邊界")]
    public Vector2 limitX = new Vector2(-5, 5);


}
