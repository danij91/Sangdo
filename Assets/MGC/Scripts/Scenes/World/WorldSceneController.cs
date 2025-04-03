// ✅ WorldSceneController.cs
using UnityEngine;
using Enums;

public class WorldSceneController : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mapPrefab;

    private GameObject player;
    private GameObject map;

    private StateMachine<WorldStateType> stateMachine;

    public void Initialize()
    {
        // 1. 맵 생성
        map = Instantiate(mapPrefab);

        // 2. 플레이어 생성
        player = Instantiate(playerPrefab);
        player.transform.position = Vector3.zero; // 시작 위치 (향후 SpawnPoint 등으로 분리 가능)

        // 3. 상태머신 초기화
        stateMachine = new StateMachine<WorldStateType>();
        stateMachine.AddState(WorldStateType.Exploration, new WorldExplorationState(this));
        stateMachine.ChangeState(WorldStateType.Exploration);
    }

    private void Update()
    {
        stateMachine?.Update();
    }

    public GameObject GetPlayer() => player;
    public GameObject GetMap() => map;
}
