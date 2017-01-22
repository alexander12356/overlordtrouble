using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct EnemyOption
{
    public string id;
    public JourneyEnemy prefab;
    public int chance;
}

public class RoomEnemyGenerator : MonoBehaviour
{
    private List<Transform> m_EnemySpawnTransform = new List<Transform>();
    private Dictionary<string, JourneyEnemy> m_EnemyList = new Dictionary<string, JourneyEnemy>();

    [SerializeField]
    private List<EnemyOption> m_EnemyOptionList =  null;

	public void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            m_EnemySpawnTransform.Add(transform.GetChild(i));
        }
    }

    public void Clear()
    {
        foreach (JourneyEnemy l_JourneyEnemy in m_EnemyList.Values)
        {
            Destroy(l_JourneyEnemy.gameObject);
        }
        m_EnemyList.Clear();
    }

    public void Generate()
    {
        Clear();
        GenerateEnemies();
    }

    private void GenerateEnemies()
    {
        for (int i = 0; i < m_EnemySpawnTransform.Count; i++)
        {
            JourneyEnemy l_EnemyPrefab = GetEnemy();

            JourneyEnemy l_NewEnemy = Instantiate(l_EnemyPrefab);
            l_NewEnemy.myTransform.SetParent(m_EnemySpawnTransform[i]);
            l_NewEnemy.myTransform.position = m_EnemySpawnTransform[i].position;
            l_NewEnemy.actorId += "_" + i;

            JourneyActorUnityEvent l_OnDieEvent = new JourneyActorUnityEvent();
            l_OnDieEvent.AddListener(OnDestroyEvent);

            l_NewEnemy.onDieEvent = l_OnDieEvent;

            m_EnemyList.Add(l_NewEnemy.actorId, l_NewEnemy);
        }
    }

    private JourneyEnemy GetEnemy()
    {
        int l_Chance = Random.Range(0, 100);
        int l_EnemyChance = 0;

        for (int i = 0; i < m_EnemyOptionList.Count; i++)
        {
            l_EnemyChance += m_EnemyOptionList[i].chance;
            if (l_Chance < l_EnemyChance)
            {
                return m_EnemyOptionList[i].prefab;
            }
        }

        return null;
    }

    private void OnDestroyEvent(JourneyActor p_JourneyActor)
    {
        m_EnemyList.Remove(p_JourneyActor.actorId);
    }
}
