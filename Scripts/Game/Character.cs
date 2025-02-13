
using LoginServer.Game;
using System.Collections.Generic;
using System.Linq;

namespace FengShengServer
{
    public class Character
    {
        /// <summary>
        /// Ω«…´ø®≈‰÷√
        /// </summary>
        public static List<CharacterType> CharacterConfigList = new List<CharacterType>()
        {
            CharacterType.LaoJin, 
            CharacterType.DaMeiNv, 
            CharacterType.EmeiFeng
        };

        public static List<CharacterType> GetNewCharacterList()
        {
            var list = CharacterConfigList.ToList();
            list.FisherYatesShuffle();
            return list;
        }

    }
}
