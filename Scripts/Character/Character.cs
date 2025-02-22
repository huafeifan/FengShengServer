using LoginServer.Game;
using System.Collections.Generic;
using System.Linq;

namespace FengShengServer
{
    public class Character
    {
        /// <summary>
        /// ��ɫ������
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

        public static ICharacter GetCharacter(CharacterType character)
        {
            switch (character)
            {
                case CharacterType.LaoJin:
                    return new LaoJin();
                case CharacterType.DaMeiNv:
                    return new DaMeiNv();
                case CharacterType.EmeiFeng:
                    return new EmeiFeng();
            }
            return null;
        }

    }
}
