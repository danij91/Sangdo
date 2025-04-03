using Enums;
using UnityEngine;

public class LobbySceneController : MonoBehaviour
{
    [SerializeField] private LobbyStateType startState = LobbyStateType.Login;
    private LobbyStateMachine lobbyFSM;

    private void Start()
    {
        lobbyFSM = gameObject.AddComponent<LobbyStateMachine>();
        lobbyFSM.Initialize(this);
        lobbyFSM.ChangeState(startState);
    }

    public void OnLoginSuccess()
    {
        lobbyFSM.ChangeState(LobbyStateType.CharacterSelect);
    }

    public void OnCharacterSelected()
    {
        lobbyFSM.ChangeState(LobbyStateType.Main);
    }

    public void OpenSettings()
    {
        lobbyFSM.ChangeState(LobbyStateType.Settings);
    }

    public void ReturnToMain()
    {
        lobbyFSM.ChangeState(LobbyStateType.Main);
    }
}