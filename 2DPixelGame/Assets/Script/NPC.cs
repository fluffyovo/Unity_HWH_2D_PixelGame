using UnityEngine;

public class NPC : MonoBehaviour
{
    #region 欄位
    [Header("商店介面")]
    public GameObject objShop;
    /// <summary>
    /// 玩家身上武器的物件，編號與購買武器相同
    /// </summary>
    [Header("武器")]
    public GameObject[] objWeapon;
    /// <summary>
    /// 武器編號
    /// 0 劍　１元
    /// 1 弓　２元
    /// 2 錘　３元
    /// </summary>
    [Header("武器編號")]
    public int indexWeapon;

    /// <summary>
    /// 武器價格，編號與選取武器相同
    /// </summary>
    private int[] priceWeapon = { 1, 2, 3 };
    private float[] attackWeapon = { 10, 20, 30 };
    private Player player;
    #endregion

    private void Start()
    {
        player = GameObject.Find("玩家").GetComponent<Player>();
    }

    #region 方法
    /// <summary>
    /// 開啟商店介面
    /// </summary>
    public void OpenShop()
    {
        objShop.SetActive(true);
    }
    
    /// <summary>
    /// 關閉商店介面
    /// </summary>
    public void CloseShop()
    {
        objShop.SetActive(false);
    }

    /// <summary>
    /// 玩家選了哪個武器
    /// </summary>
    /// <param name="choose">武器編號</param>
    public void ChooseWeapon(int choose)
    {
        indexWeapon = choose;
    }

    /// <summary>
    /// 購買武器
    /// 1.判斷玩家金幣是否足夠
    /// 2.購買後扣除金幣更新介面 
    /// 3.並顯示武器
    /// </summary>
    public void Buy()
    {
        if (player.coin >= priceWeapon[indexWeapon])
        {
            player.coin -= priceWeapon[indexWeapon];
            player.textCoin.text = "金幣:" + player.coin;

            //將目前購買的武器攻擊力給玩家
            player.attackWeapon = attackWeapon[indexWeapon];

            for (int i = 0; i < objWeapon.Length; i++)
            {
                objWeapon[i].SetActive(false);
            }
            objWeapon[indexWeapon].SetActive(true);
        }
    }
    #endregion
}
