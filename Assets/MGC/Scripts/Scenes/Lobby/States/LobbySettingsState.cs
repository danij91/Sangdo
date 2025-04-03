public class LobbySettingsState : IState
{
    private LobbySceneController controller;
    private LobbyStateMachine stateMachine;

    public LobbySettingsState(LobbySceneController controller, LobbyStateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        UIManager.Instance.OpenPopup<LobbySettingsPopup>();
    }

    public void Update() { }
    public void Exit() { }
}