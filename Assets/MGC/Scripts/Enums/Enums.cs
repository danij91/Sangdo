namespace Enums
{
    public enum SceneType
    {
        Lobby , World, Battle
    }
    
    // ✅ LobbyStateType.cs
    public enum LobbyStateType
    {
        None,
        Login,
        CharacterSelect,
        Main,
        Settings
    }

    public enum WorldStateType
    {
        Exploration,
        Encounter,
        Dialogue,
        Town
    }
}