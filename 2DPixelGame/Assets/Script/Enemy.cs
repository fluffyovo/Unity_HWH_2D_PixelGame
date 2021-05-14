using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region 欄位
    [Header("追蹤範圍"), Range(0, 50)]
    public float rangeTrack = 3;
    [Header("攻擊範圍"), Range(0, 50)]
    public float rangeAttack = 1;
    [Header("移動速度"), Range(0, 50)]
    public float speed = 2;
    [Header("攻擊特效")]
    public ParticleSystem psAttack;
    [Header("攻擊冷卻時間"), Range(0, 50)]
    public float cdAttack = 3;
    [Header("攻擊力"), Range(0, 100)]
    public float attack = 10;
    [Header("血量"), Range(0, 1000)]
    public float hp = 200f;
    [Header("血條系統")]
    public HpManager hpManager;
    [Header("提供經驗值"), Range(0, 100000)]
    public float exp = 30;

    private float hpMax;
    private bool isDead = false;

    private Transform player;           // 存取玩家位置資料
    private Player _player;             // 存取玩家C#腳本
    /// <summary>
    /// 計時器
    /// </summary>
    private float timer;
    #endregion

    #region 事件
    private void Start()
    {
        hpMax = hp;      //取得血量最大值
        
        // 玩家位置 = 抓取場景的玩家物件 的 位置
        player = GameObject.Find("玩家").transform;
        _player = player.GetComponent<Player>();
    }

    // 繪製圖示事件 : 在Unity內顯示輔助開發
    private void OnDrawGizmos()
    {
        // 圖形的顏色
        Gizmos.color = new Color(0, 0, 1, 0.3f);
        // 繪製圓形範圍(中心點 ，半徑)
        Gizmos.DrawSphere(transform.position, rangeTrack);

        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, rangeAttack);
    }

    private void Update()
    {
        Track();
    }
    #endregion

    #region 方法
    /// <summary>
    /// 追蹤玩家
    /// </summary>
    private void Track()
    {
        if (isDead) return;           //如果死亡 就跳出

        // 兩者間的距離 = 三維向量 的 距離(Ａ點，Ｂ點)
        float dis = Vector3.Distance(transform.position, player.position);
        
        // 如果 距離 <= 攻擊範圍 就 攻擊
        // 否則 距離 <= 追蹤範圍 就 繼續追蹤
        if (dis <= rangeAttack)
        {
            Attack();
        }
        else if (dis <= rangeTrack)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 攻擊事件
    /// </summary>
    private void Attack()
    {
        timer += Time.deltaTime;    // 累加時間

        // 如果 計時器 >= 冷卻時間 就 攻擊
        if (timer >= cdAttack)
        {
            timer = 0;              // 計時器 歸零
            psAttack.Play();        // 播放 攻擊特效

            // 2D 碰撞 = 2D 物理.覆蓋圓形範圍 (中心點，半徑，圖層)
            Collider2D hit = Physics2D.OverlapCircle(transform.position, rangeAttack, 1<<9);
            // 碰到的物件 取得元件<玩家>().受傷(攻擊力)
            hit.GetComponent<Player>().Hit(attack);
        }
    }

    /// <summary>
    /// 受傷
    /// </summary>
    /// <param name="Pdamage"></param>
    public void Hit(float Pdamage)
    {
        if (isDead) return;
        hp -= Pdamage;                                    // 扣除傷害值
        hpManager.UpdateHpBar(hp, hpMax);                 // 更新血條
        StartCoroutine(hpManager.ShowDamage(Pdamage));    // 啟動協同程序(顯示傷害值())

        if (hp <= 0) Dead();                              // 如果血量 <= 0 就死亡
    }

    /// <summary>
    /// 死亡
    /// </summary>
    private void Dead()
    {
        if (isDead) return;
        hp = 0;
        isDead = true;
        Destroy(gameObject, 1.5f);
        _player.Exp(exp);
    }
    #endregion
}
