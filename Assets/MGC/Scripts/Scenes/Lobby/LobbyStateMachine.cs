using Enums;
using UnityEngine;

public class LobbyStateMachine : MonoBehaviour
{
    private StateMachine<LobbyStateType> stateMachine;
    private LobbySceneController controller;

    public LobbyStateType CurrentState => stateMachine.CurrentStateType;

    public void Initialize(LobbySceneController controller)
    {
        this.controller = controller;
        stateMachine = new StateMachine<LobbyStateType>();

        stateMachine.AddState(LobbyStateType.Login, new LobbyLoginState(controller, this));
        stateMachine.AddState(LobbyStateType.CharacterSelect, new LobbyCharacterSelectState(controller, this));
        stateMachine.AddState(LobbyStateType.Main, new LobbyMainState(controller, this));
        stateMachine.AddState(LobbyStateType.Settings, new LobbySettingsState(controller, this));
    }

    public void ChangeState(LobbyStateType newState)
    {
        stateMachine.ChangeState(newState);
    }

    private void Update()
    {
        stateMachine?.Update();
    }
}