
using LoginServer.Game;
using System.Collections.Generic;

namespace FengShengServer
{
    public enum GameStage
    {
        None,
        /// <summary>
        /// �ȴ���Ϸ��ʼ
        /// </summary>
        WaitGameStart,
        /// <summary>
        /// �ȴ����ѡ��
        /// </summary>
        WaitIdentityChoose,
        /// <summary>
        /// �ȴ����ѡ��
        /// </summary>
        WaitCharacterChoose,
        /// <summary>
        /// �ȴ�����
        /// </summary>
        WaitDealCards,
        /// <summary>
        /// �ȴ��ֵ�ĳ��
        /// </summary>
        WaitGameTurn,
        /// <summary>
        /// �ȴ��غϿ�ʼ
        /// </summary>
        WaitGameTurnStart,
        /// <summary>
        /// �ȴ��غϲ�������
        /// </summary>
        WaitGameTurnOpertateEnd,
        /// <summary>
        /// �ȴ��غ�����
        /// </summary>
        WaitGameTurnDisCard,
        /// <summary>
        /// �ȴ��غϽ���
        /// </summary>
        WaitGameTurnEnd,
    }
}
