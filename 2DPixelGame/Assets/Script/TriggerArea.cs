using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    [Header("要被機關消除的石頭")]
    public GameObject[] stones;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "石頭")
        {
            stones[0].SetActive(false);
            stones[1].SetActive(false);
            
        }
    }
}
