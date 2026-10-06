using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;

    // 1스테이지에 출현하는 적들 종류
    public List<Enemy> stage1_Emergence_Enemy;
    public Vector3 WatsonVector3;
    public Vector3 offset1;
    public Vector3 offset2;
    public Vector3 offset3;
    [SerializeField] private GameObject John_Watson;
    public static bool WatsonAlive = true;
    [SerializeField] private TurnManager _TurnManager;
    [SerializeField] private GameObject Sherlock_Holmes;
    [SerializeField] private EnemyAttakManager _EnemyAttakManager;
    

    // list의 몇 번째 몹을 소환할지
    private int list1;

    void Start()
    {
        Sherlock_Holmes = GameObject.FindWithTag("Holmes");  
        
        list1 = Random.Range(0, stage1_Emergence_Enemy.Count);
        GameObject InstantiateEnemy1 = Instantiate(enemy, offset1, Quaternion.identity);
        EnemyDataLoad EnemyData1 = InstantiateEnemy1.GetComponentInChildren<EnemyDataLoad>();
        EnemyData1.Load(stage1_Emergence_Enemy[list1]);
        _TurnManager.Input_Enemt_Turn(InstantiateEnemy1);
        
        list1 = Random.Range(0, stage1_Emergence_Enemy.Count);
        GameObject InstantiateEnemy2 = Instantiate(enemy, offset2, Quaternion.identity);
        EnemyDataLoad EnemyData2 = InstantiateEnemy2.GetComponentInChildren<EnemyDataLoad>();
        EnemyData2.Load(stage1_Emergence_Enemy[list1]);
        _TurnManager.Input_Enemt_Turn(InstantiateEnemy2);
        
        list1 = Random.Range(0, stage1_Emergence_Enemy.Count);
        GameObject InstantiateEnemy3 = Instantiate(enemy, offset3, Quaternion.identity);
        EnemyDataLoad EnemyData3 = InstantiateEnemy3.GetComponentInChildren<EnemyDataLoad>();
        EnemyData3.Load(stage1_Emergence_Enemy[list1]);
        _TurnManager.Input_Enemt_Turn(InstantiateEnemy3);

        if (WatsonAlive)
        {
            GameObject Watson = Instantiate(John_Watson, WatsonVector3, Quaternion.identity);
            _EnemyAttakManager.WatsonObjectInput(Watson);
            _TurnManager.Turn_order.Insert(0, Watson);
        }
        
        _TurnManager.TurnStart(Sherlock_Holmes);
        _EnemyAttakManager.HolmesObjectInput(Sherlock_Holmes);
        _TurnManager.Turn_order.Insert(0, Sherlock_Holmes);
    }
}