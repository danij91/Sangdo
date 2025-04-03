public class LobbyMainState : IState
{
    private LobbySceneController controller;
    private LobbyStateMachine stateMachine;

    public LobbyMainState(LobbySceneController controller, LobbyStateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        UIManager.Instance.OpenScreen<LobbyMainScreen>();
        UIManager.Instance.OpenOverlay<LobbyOverlay>();
    }

    public void Update() { }
    public void Exit()
    {
        UIManager.Instance.CloseAllPopups();
    }
}