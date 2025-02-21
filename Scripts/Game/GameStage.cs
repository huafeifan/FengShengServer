
using LoginServer.Game;
using System.Collections.Generic;

namespace FengShengServer
{
    public enum GameStage
    {
        None,
        /// <summary>
        /// ��Ϸ��ʼ
        /// </summary>
        GameStart,
        /// <summary>
        /// ���ѡ��
        /// </summary>
        IdentityChoose,
        /// <summary>
        /// ���ѡ��
        /// </summary>
        CharacterChoose,
        /// <summary>
        /// ����
        /// </summary>
        DealCards,
        /// <summary>
        /// �ֵ�ĳ��
        /// </summary>
        GameTurn,
        /// <summary>
        /// �غϿ�ʼ
        /// </summary>
        GameTurnStart,
        /// <summary>
        /// �غϲ�������
        /// </summary>
        GameTurnOpertateEnd,
        /// <summary>
        /// �غ�����
        /// </summary>
        GameTurnDisCard,
        /// <summary>
        /// �غϽ���
        /// </summary>
        GameTurnEnd,
    }
}
