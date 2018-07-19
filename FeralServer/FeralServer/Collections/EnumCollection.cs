using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralServerProject.Collections
{
    public enum eMessageTypes
    {
        ConnnectMessage,
        DisconnectMessage,
        ReadyMessage,
        StartGameMessage,
        StopGameMessage,
        ChatMessage,
        HeartbeatMessage,
        GameStateMessage,
        GameInputMessage,
        EndTurnMessage,
        GameSettingsMessage,
        ClientInformationMessage,
        PlayerRenameMessage,
        RoomCreationMessage,
        RoomJoinMessage,
        RoomListUpdateMessage,
        RoomInformationMessage,
        MapSendMessage,
        RoomLobbyMessage
    }

    public enum eSettingType
    {
        TrunCount,
        MaxPlayers,
        RaceID
    }

    public enum eReadyState
    {
        Ready,
        NotReady
    }
}
