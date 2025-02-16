using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoginServer.Game;

namespace FengShengServer
{
    public interface ICharacter
    {
        CharacterType GetCharacterType();

        bool IsVictory(ChairInfo chair);
    }
}
