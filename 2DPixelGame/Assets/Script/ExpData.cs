using UnityEngine;

// 自訂檔案預設名稱、Project+號目錄下 的 資料夾名稱/子項名稱
[CreateAssetMenu (fileName = "經驗值資料", menuName = "OvO/經驗值資料")]
public class ExpData : ScriptableObject
{
    // 陣列
    // 語法: 在類型後面加上中括號
    // 陣列的作用: 儲存多筆相同類型的資料
    [Header("每個等級經驗值需求，從一等開始")]
    public float[] exp;
}
