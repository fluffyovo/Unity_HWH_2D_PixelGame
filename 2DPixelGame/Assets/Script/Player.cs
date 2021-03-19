using UnityEngine;

public class Player : MonoBehaviour
{
    //註解

    //欄位語法
    //修飾詞 類型 名稱 (指定 值);
    //私人 private 不顯示 (預設值)
    //公開 public  顯示

    //類型 四大類型
    //整數   int
    //浮點數 float
    //布林值 bool true 是、flase 否
    //字串   string
    [Header ("等級"), Tooltip("這是角色等級")]
    public int level = 1;
    [Header ("移動速度"), Range (1,100)]
    public float speed = 10.5f; //浮點數設定值後一定要加 f/F (大小寫不均)
    [Header ("是否死亡")]
    public bool isDead = false;
    [Header ("角色名稱"), Tooltip ("這是角色名稱")]
    public string cName = "熊咪"; //字串必須加上""

    [Header ("血量"), Range(0,100)]
    public float hp = 100f;
    [Header ("攻擊"), Range(1,100)]
    public float attack = 20f;
    [Header ("經驗值"), Range(0, 100000)]
    public int exp = 0;
    [Header("金幣"), Range(0, 100000)]
    public int coin = 0;
    [Header("虛擬搖桿")]
    public FixedJoystick joystick;
    [Header("變形元件")]
    public Transform tra;
    [Header("動畫元件")]
    public Animator ani;

    // 方法語法 Method - 儲存複雜的程式區塊或演算法
    // 修飾詞 類型 名稱() { 程式區塊或演算法}
    // void 無類型
    
    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        print("移動");
        float h = joystick.Horizontal;
        float v = joystick.Vertical;

        tra.Translate(h * speed * Time.deltaTime, v * speed * Time.deltaTime, 0);
        ani.SetFloat("水平", h);
        ani.SetFloat("垂直", v);
    }

    private void Attack()
    {

    }

    private void Hit() 
    {
    
    }

    private void Dead()
    {

    }

    // 事件 - 特定時間會執行方法
    // 開始事件 : 播放後執行一次
    private void Start()
    {
        //呼叫方法
        //方法名稱();
    }

    // 更新事件 : 大約一秒執行六十次 60FPS
    private void Update()
    {
        Move();
    }
}
