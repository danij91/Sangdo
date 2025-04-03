public class LobbyCharacterSelectState : IState
{
    private LobbySceneController controller;
    private LobbyStateMachine stateMachine;

    public LobbyCharacterSelectState(LobbySceneController controller, LobbyStateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        UIManager.Instance.OpenScreen<CharacterSelectScreen>();
    }

    public void Update() { }
    public void Exit() { }
}