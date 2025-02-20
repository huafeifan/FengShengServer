
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
        /// �ȴ����ѡ��
        /// </summary>
        IdentityChoose,
        /// <summary>
        /// �ȴ����ѡ��
        /// </summary>
        CharacterChoose,
        /// <summary>
        /// �ȴ�����
        /// </summary>
        DealCards,
        /// <summary>
        /// �ȴ��ֵ�ĳ��
        /// </summary>
        GameTurn,
        /// <summary>
        /// �ȴ��غϿ�ʼ
        /// </summary>
        GameTurnStart,
        /// <summary>
        /// �ȴ��غϲ�������
        /// </summary>
        GameTurnOpertateEnd,
        /// <summary>
        /// �ȴ��غ�����
        /// </summary>
        GameTurnDisCard,
        /// <summary>
        /// �ȴ��غϽ���
        /// </summary>
        GameTurnEnd,
    }
}
