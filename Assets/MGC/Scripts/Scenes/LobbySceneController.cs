using System.Collections;
using UnityEngine;

public class LobbySceneController : BaseSceneController
{
    public override void Initialize()
    {
        Debug.Log("[LobbyScene] Initialized");
        StartCoroutine(LobbyFlow());
    }

    private IEnumerator LobbyFlow()
    {
        UIManager.Instance.OpenOverlay<LobbyOverlay>();
        UIManager.Instance.OpenScreen<LoadingScreen>();
        yield return new WaitForSeconds(1f);

        UIManager.Instance.OpenScreen<LoginScreen>();
        yield return new WaitUntil(() => LoginManager.Instance.IsLoggedIn);

        UIManager.Instance.OpenScreen<CharacterSelectScreen>();
        yield return new WaitUntil(() => LoginManager.Instance.HasSelectedCharacter);

        UIManager.Instance.OpenScreen<LobbyMainScreen>();
    }
}