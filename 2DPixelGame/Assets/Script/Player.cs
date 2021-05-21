﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    #region 欄位
    [Header ("等級"), Tooltip("這是角色等級")]
    public int level = 1;
    [Header ("等級文字")]
    public Text lvText;
    [Header ("移動速度"), Range (1, 100)]
    public float speed = 10.5f; //浮點數設定值後一定要加 f/F (大小寫不均)
    [Header ("角色名稱"), Tooltip ("這是角色名稱")]
    public string cName = "熊咪"; //字串必須加上""
    [Header ("血量"), Range(0, 1000)]
    public float hp = 200f;
    [Header ("攻擊"), Range(1, 100)]
    public float attack = 20f;
    [Header ("經驗值"), Range(0, 100000)]
    public float exp = 0;
    [Header("金幣"), Range(0, 100000)]
    public int coin = 0;
    [Header("虛擬搖桿")]
    public FixedJoystick joystick;
    [Header("變形元件")]
    public Transform tra;
    [Header("動畫元件")]
    public Animator ani;
    [Header("偵測攻擊範圍")]
    public float rangeAttack = 1.2f;
    [Header("音效來源")]
    public AudioSource aud;
    [Header("攻擊音效")]
    public AudioClip soundAttack;
    [Header("血條系統")]
    public HpManager hpManager;
    [Header("吃金幣音效")]
    public AudioClip SoundEat;
    [Header("吃金幣數量")]
    public Text textCoin;
    [Header("經驗值吧條")]
    public Image imgExp;
    [Header("經驗值資料")]
    public ExpData expData;

    public float attackWeapon;
    private float hpMax;
    private bool isDead = false;
    /// <summary>
    /// 需要多少經驗值才能升等，一等設定為100
    /// </summary>
    private float expNeed = 100;

    #endregion

    #region 方法
    // 方法語法 Method - 儲存複雜的程式區塊或演算法
    // 修飾詞 類型 名稱() { 程式區塊或演算法}
    // void 無類型

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        if (isDead) return;           //如果死亡 就跳出

        float h = joystick.Horizontal;
        float v = joystick.Vertical;

        tra.Translate(h * speed * Time.deltaTime, v * speed * Time.deltaTime, 0);
        ani.SetFloat("水平", h);
        ani.SetFloat("垂直", v);
    }

    // 要被按鈕呼叫必須設公開
    public void Attack()
    {
        if (isDead) return;           //如果死亡 就跳出

        // 音效來源.播放一次(音效片段，音量）
        aud.PlayOneShot(soundAttack,1.2f);

        // 2D物理 圓形碰撞 (中心點，半徑，方向，距離，圖層編號－寫法為"１＜＜指定圖層編號"）
        RaycastHit2D hit =  Physics2D.CircleCast(transform.position, rangeAttack, -transform.up, 0, 1 << 8);
 
        /*
        // 如果 碰到的物件 標籤 為 道具 就刪除(碰到的碰撞器的遊戲物件)
        if (hit &&　hit.collider.tag == "道具") Destroy(hit.collider.gameObject);
        print("碰到的物件:" + hit.collider.name);
        */

        // 如果 碰到的物件存在 並且 標籤 為 道具 就 取得道具腳本並呼叫掉落道具方法
        if (hit && hit.collider.tag == "道具") hit.collider.GetComponent<item>().DropProp();
        // 如果 打到的標籤是 敵人 就 受傷
        if (hit && hit.collider.tag == "敵人") hit.collider.GetComponent<Enemy>().Hit(attack + attackWeapon);
        // 如果 打到的標籤是 NPC 就 開啟商店
        if (hit && hit.collider.tag == "NPC") hit.collider.GetComponent<NPC>().OpenShop();

    }

    // 要被其他腳本呼叫也要設公開
    /// <summary>
    /// 受傷
    /// </summary>
    /// <param name="Pdamage"></param>
    public void Hit(float Pdamage) 
    {
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
        hp = 0;
        isDead = true;
        Invoke("Replay", 2);          // 延遲呼叫("方法名稱"，延遲時間)
    }

    /// <summary>
    /// 重新開始
    /// </summary>
    private void Replay()
    {
        SceneManager.LoadScene("遊戲場景");
    }

    /// <summary>
    /// 經驗值控制
    /// </summary>
    /// <param name="getExp">得到的經驗值</param>
    public void Exp(float getExp)
    {
        // 取得目前等級需要的經驗需求
        // 要取得的資料為 等級 減1
        expNeed = expData.exp[level - 1];

        exp += getExp;
        print("經驗值:" + exp);
        imgExp.fillAmount = exp / expNeed;

        // 升級
        // 迴圈 while
        // 語法:
        // while（布林值）｛布林值　為　true 時持續執行｝
        // if（布林值）｛布林值　為　true 時執行一次｝
        while (exp >= expNeed)                       // 如果經驗值 >= 經驗需求
        {
            level++;                             // 等級提升
            lvText.text = "Lv. " + level;         // 介面更新
            exp -= expNeed;                       // 補回溢出的經驗
            imgExp.fillAmount = exp / expNeed;    // 介面更新
            expNeed = expData.exp[level - 1];
            LevelUp();                            // 呼叫升級方法
        }
    }

    /// <summary>
    /// 升級後的數據更新
    /// </summary>
    private void LevelUp()
    {
        // 攻擊力每一等提升 10，從 預設attack(20) 開始
        attack = 20 + (level - 1) * 10;
        // 血量上限每一等提升 50，從 預設hp(200) 開始
        hpMax = 200 + (level - 1) * 50;

        hp = hpMax;                         // 恢復血量全滿
        hpManager.UpdateHpBar(hp, hpMax);   // 更新血條
    }
    #endregion

    #region 事件
    // 事件 - 特定時間會執行方法

    // 開始事件 : 播放後執行一次
    private void Start()
    {
        coin = 10;
        textCoin.text = "金幣：" + 10;

        hpMax = hp;

        for (int i = 0; i < 99; i++)
        {
            expData.exp[i] = (i + 1 ) * 100;
            //print(i);
        }
    }

    // 更新事件 : 大約一秒執行六十次 60FPS
    private void Update()
    {
        Move();
    }

    // 觸發事件 - 進入：兩個物件必須有一個勾選Is Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "金條")
        {
            coin++;
            aud.PlayOneShot(SoundEat);
            Destroy(collision.gameObject);
            textCoin.text = "金幣：" + coin;

        }
    }

    // 事件 : 繪製圖示
    private void OnDrawGizmos()
    {
        //指定圖示顏色 (紅，綠，藍，透明）
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        //繪製圖示 球體(中心點，半徑)
        Gizmos.DrawSphere(transform.position, rangeAttack);
    }
 
    #endregion
}
