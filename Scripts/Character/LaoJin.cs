using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoginServer.Game;

namespace FengShengServer
{
    public class LaoJin : ICharacter
    {
        private CharacterType mCharacterType = CharacterType.LaoJin;

        public CharacterType GetCharacterType()
        {
            return mCharacterType;
        }

        public bool IsVictory(ChairInfo chair)
        {
            int redCount = chair.GetInformationCount(Card_ColorType.Red);
            int blueCount = chair.GetInformationCount(Card_ColorType.Blue);
            int redBlueCount = chair.GetInformationCount(Card_ColorType.RedBlue);

            return (redCount + redBlueCount) >= 2 &&
                (blueCount + redBlueCount) >= 2 &&
                (redCount + blueCount + redBlueCount) >= 4;
        }
    }

}
