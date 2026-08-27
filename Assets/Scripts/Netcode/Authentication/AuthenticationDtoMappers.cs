using System;
using Profiles.Models;
using UnityEngine;

namespace Netcode.Authentication
{
    public static class AuthenticationDtoMappers
    {
        public static PlayerProfile FromDto(this PlayerProfileResponseDto dto)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));
            
            // Check if displayname is formated wrong
            if (string.IsNullOrWhiteSpace(dto.displayName))
                throw new FormatException(nameof(dto.displayName));
            
            // Check if playerId is formated wrong and get it
            if (!Guid.TryParse(dto.playerId, out Guid guid))
                throw new FormatException(nameof(dto.playerId));
            
            // Check if playerColor is formated wrong and get it
            if (!ColorUtility.TryParseHtmlString(dto.playerColor, out Color color))
                throw new FormatException(nameof(dto.playerColor));

            // Save playerId and color32
            PlayerId playerId = new(guid);
            Color32 color32 = color;
            PlayerColor playerColor = new(color32.r, color32.g, color32.b);
            return new PlayerProfile(playerId, dto.displayName, playerColor);
        }
    }
}