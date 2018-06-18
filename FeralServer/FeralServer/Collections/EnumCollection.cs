﻿using System;
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
        EmptyMessage2,
        GameStateMessage,
        GameInputMessage,
        EndTurnMessage,
        GameSettingsMessage
    }

    public enum eSettingType
    {
        TrunCount,
        MaxPlayers,
        RaceID
    }
}