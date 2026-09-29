using UnityEngine;

public class EnemyDataLoad : MonoBehaviour
{
    private Sprite EnemySprite1;
    private SpriteRenderer _SpriteRenderer;
    private HPscripts _HPscripts;

    public void Load (Enemy EnemyData)
    {
        _SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _SpriteRenderer.sprite = EnemyData.EnemySprite;
        _HPscripts = gameObject.GetComponent<HPscripts>();
        _HPscripts.MaxHP = EnemyData.EnemyHP;
        _HPscripts.HP = EnemyData.EnemyHP;
    }
}