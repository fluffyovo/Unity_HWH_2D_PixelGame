using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("攝影機速度"), Range(0, 50)]
    public float speed = 1.5f;
    [Header("上下邊界")]
    public Vector2 limitY = new Vector2(-5, 5);
    [Header("左右邊界")]
    public Vector2 limitX = new Vector2(-5, 5);
    [Header("玩家座標")]
    public Transform player;

    private void Update()
    {
        Track();
    }


    private void Track()
    {
        Vector3 poscam = transform.position;   // 取得 攝影機座標
        Vector3 pospla = player.position;      // 取得 玩家座標

        poscam = Vector3.Lerp(poscam, pospla, 0.5f * speed * Time.deltaTime);
        poscam.z = -10;


        poscam.x = Mathf.Clamp(poscam.x, limitX.x, limitX.y);
        poscam.y = Mathf.Clamp(poscam.y, limitY.x, limitY.y);

        transform.position = poscam;           // 更新攝影機座標
    }
}
