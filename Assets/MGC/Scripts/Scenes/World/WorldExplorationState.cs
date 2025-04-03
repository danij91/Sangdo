using UnityEngine;

public class WorldExplorationState : IState
{
    private WorldSceneController controller;

    public WorldExplorationState(WorldSceneController controller)
    {
        this.controller = controller;
    }

    public void Enter()
    {
        Debug.Log("[WorldState] Exploration 시작");
    }

    public void Update()
    {
    }

    public void Exit()
    {
        Debug.Log("[WorldState] Exploration 종료");
    }
}