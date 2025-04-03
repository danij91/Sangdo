public class LobbyLoginState : IState
{
    private LobbySceneController controller;
    private LobbyStateMachine stateMachine;

    public LobbyLoginState(LobbySceneController controller, LobbyStateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        UIManager.Instance.OpenOverlay<LobbyOverlay>();
        UIManager.Instance.OpenScreen<LoginScreen>();
    }

    public void Update() { }
    public void Exit() { }
}