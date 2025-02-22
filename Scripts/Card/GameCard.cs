using System.Collections.Generic;
using System.Linq;
using LoginServer.Game;

namespace FengShengServer
{
    public class GameCard
    {
        #region card config
        /// <summary>
        /// 1�����ݷ�ʽ���ı�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ������ı�   ���� ��һ����   Ǳ�� �ع� ��������/��һ����
        /// </summary>
        private static readonly CardType GongKaiWenBen1 = new CardType()
        {
            CardName = "card_gongkaiwenben1",
            Transmit = Card_TransmitType.WenBen,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.GongKaiWenBen,
            Gongkaiwenben = Card_GongKaiWenBenType.BlueAdd,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ı�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�����
        /// 5���鱨���ƣ������ı�   Ǳ�� ��һ����   Ǳ�� �ع� ��һ����/��һ����
        /// </summary>
        private static readonly CardType GongKaiWenBen2 = new CardType()
        {
            CardName = "card_gongkaiwenben2",
            Transmit = Card_TransmitType.WenBen,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Lock,
            Xiaoguo = Card_XiaoGuoType.GongKaiWenBen,
            Gongkaiwenben = Card_GongKaiWenBenType.RedSub,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ı�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ������ı�   �ع� ��һ����   Ǳ�� ���� ��������/��һ����
        /// </summary>
        private static readonly CardType GongKaiWenBen3 = new CardType()
        {
            CardName = "card_gongkaiwenben3",
            Transmit = Card_TransmitType.WenBen,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.GongKaiWenBen,
            Gongkaiwenben = Card_GongKaiWenBenType.GreenAdd,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ı�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ������ı�   Ǳ�� ��һ����   �ع� ���� ��������/��һ����
        /// </summary>
        private static readonly CardType GongKaiWenBen4 = new CardType()
        {
            CardName = "card_gongkaiwenben4",
            Transmit = Card_TransmitType.WenBen,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.GongKaiWenBen,
            Gongkaiwenben = Card_GongKaiWenBenType.RedAdd,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ı�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�����
        /// 5���鱨���ƣ������ı�   ���� ��һ����   �ع� Ǳ�� ��һ����/��һ����
        /// </summary>
        private static readonly CardType GongKaiWenBen5 = new CardType()
        {
            CardName = "card_gongkaiwenben5",
            Transmit = Card_TransmitType.WenBen,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Lock,
            Xiaoguo = Card_XiaoGuoType.GongKaiWenBen,
            Gongkaiwenben = Card_GongKaiWenBenType.BlueSub,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ������´�   ���� ���鱨   ���� ���鱨   ���� ���鱨
        /// </summary>
        private static readonly CardType MiMiXiaDa1 = new CardType()
        {
            CardName = "card_mimixiada1",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.MiMiXiaDa,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.BlueGrayRed,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ������´�   ���� ���鱨   ���� ���鱨   ���� ���鱨
        /// </summary>
        private static readonly CardType MiMiXiaDa2 = new CardType()
        {
            CardName = "card_mimixiada2",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.MiMiXiaDa,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.BlueGrayRed,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ������´�   ���� ���鱨   ���� ���鱨   ���� ���鱨
        /// </summary>
        private static readonly CardType MiMiXiaDa3 = new CardType()
        {
            CardName = "card_mimixiada3",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.MiMiXiaDa,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.BlueGrayRed,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ������´�   ���� ���鱨   ���� ���鱨   ���� ���鱨
        /// </summary>
        private static readonly CardType MiMiXiaDa4 = new CardType()
        {
            CardName = "card_mimixiada4",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.MiMiXiaDa,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.RedBlueGray,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ������´�   ���� ���鱨   ���� ���鱨   ���� ���鱨
        /// </summary>
        private static readonly CardType MiMiXiaDa5 = new CardType()
        {
            CardName = "card_mimixiada5",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.MiMiXiaDa,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.RedBlueGray,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ������´�   ���� ���鱨   ���� ���鱨   ���� ���鱨
        /// </summary>
        private static readonly CardType MiMiXiaDa6 = new CardType()
        {
            CardName = "card_mimixiada6",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.MiMiXiaDa,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.GrayRedBule,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ������´�   ���� ���鱨   ���� ���鱨   ���� ���鱨
        /// </summary>
        private static readonly CardType MiMiXiaDa7 = new CardType()
        {
            CardName = "card_mimixiada7",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.MiMiXiaDa,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.GrayRedBule,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ������´�   ���� ���鱨   ���� ���鱨   ���� ���鱨
        /// </summary>
        private static readonly CardType MiMiXiaDa8 = new CardType()
        {
            CardName = "card_mimixiada8",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.MiMiXiaDa,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.GrayRedBule,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ������´�   ���� ���鱨   ���� ���鱨   ���� ���鱨
        /// </summary>
        private static readonly CardType MiMiXiaDa9 = new CardType()
        {
            CardName = "card_mimixiada9",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.MiMiXiaDa,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.RedBlueGray,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ���̽   �ع� ����һ����/˵-���Ǻ���  Ǳ�� ����һ����/˵-�����Ե�   ���� ����һ����/˵-���Ǽ��
        /// </summary>
        private static readonly CardType ShiTan1 = new CardType()
        {
            CardName = "card_shitan1",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShiTan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.GuessHaoRen,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ���̽   �ع� ����һ����/˵-�����Ե�  Ǳ�� ����һ����/˵-���Ǽ��   ���� ����һ����/˵-���Ǻ���
        /// </summary>
        private static readonly CardType ShiTan2 = new CardType()
        {
            CardName = "card_shitan2",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShiTan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.GuessWoDi,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ���̽   �ع� ����һ����/˵-���Ǽ��  Ǳ�� ����һ����/˵-���Ǻ���   ���� ����һ����/˵-�����Ե�
        /// </summary>
        private static readonly CardType ShiTan3 = new CardType()
        {
            CardName = "card_shitan3",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShiTan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.GuessJianDie,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ���̽   ���� ��һ����  Ǳ�� �ع� ��һ����
        /// </summary>
        private static readonly CardType ShiTan4 = new CardType()
        {
            CardName = "card_shitan4",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShiTan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.BlueAdd,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ���̽   ���� ��һ����  Ǳ�� �ع� ��һ����
        /// </summary>
        private static readonly CardType ShiTan5 = new CardType()
        {
            CardName = "card_shitan5",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShiTan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.BlueAdd,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ���̽   �ع� ��һ����  Ǳ�� ���� ��һ����
        /// </summary>
        private static readonly CardType ShiTan6 = new CardType()
        {
            CardName = "card_shitan6",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShiTan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.GreenAdd,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ���̽   �ع� ��һ����  Ǳ�� ���� ��һ����
        /// </summary>
        private static readonly CardType ShiTan7 = new CardType()
        {
            CardName = "card_shitan7",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShiTan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.GreenAdd,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ���̽   Ǳ�� ��һ����  ���� �ع� ��һ����
        /// </summary>
        private static readonly CardType ShiTan8 = new CardType()
        {
            CardName = "card_shitan8",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShiTan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.RedAdd,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ���̽   Ǳ�� ��һ����  ���� �ع� ��һ����
        /// </summary>
        private static readonly CardType ShiTan9 = new CardType()
        {
            CardName = "card_shitan9",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShiTan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.RedAdd,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ı�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType DiaoBao1 = new CardType()
        {
            CardName = "card_diaobao1",
            Transmit = Card_TransmitType.WenBen,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Lock,
            Xiaoguo = Card_XiaoGuoType.DiaoBao,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ı�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType DiaoBao2 = new CardType()
        {
            CardName = "card_diaobao2",
            Transmit = Card_TransmitType.WenBen,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Lock,
            Xiaoguo = Card_XiaoGuoType.DiaoBao,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ı�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType DiaoBao3 = new CardType()
        {
            CardName = "card_diaobao3",
            Transmit = Card_TransmitType.WenBen,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Lock,
            Xiaoguo = Card_XiaoGuoType.DiaoBao,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ��ػ�
        /// </summary>
        private static readonly CardType JieHuo1 = new CardType()
        {
            CardName = "card_jiehuo1",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.JieHuo,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ��ػ�
        /// </summary>
        private static readonly CardType JieHuo2 = new CardType()
        {
            CardName = "card_jiehuo2",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.JieHuo,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ��ػ�
        /// </summary>
        private static readonly CardType JieHuo3 = new CardType()
        {
            CardName = "card_jiehuo3",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.JieHuo,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ��ػ�
        /// </summary>
        private static readonly CardType JieHuo4 = new CardType()
        {
            CardName = "card_jiehuo4",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.JieHuo,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ��ػ�
        /// </summary>
        private static readonly CardType JieHuo5 = new CardType()
        {
            CardName = "card_jiehuo5",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.JieHuo,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ��ػ�
        /// </summary>
        private static readonly CardType JieHuo6 = new CardType()
        {
            CardName = "card_jiehuo6",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.JieHuo,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ����
        /// </summary>
        private static readonly CardType LiJian1 = new CardType()
        {
            CardName = "card_lijian1",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.LiJian,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ����
        /// </summary>
        private static readonly CardType LiJian2 = new CardType()
        {
            CardName = "card_lijian2",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.LiJian,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ����
        /// </summary>
        private static readonly CardType LiJian3 = new CardType()
        {
            CardName = "card_lijian3",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.LiJian,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType PoYi1 = new CardType()
        {
            CardName = "card_poyi1",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.PoYi,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType PoYi2 = new CardType()
        {
            CardName = "card_poyi2",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.PoYi,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType PoYi3 = new CardType()
        {
            CardName = "card_poyi3",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.PoYi,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType PoYi4 = new CardType()
        {
            CardName = "card_poyi4",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.PoYi,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType PoYi5 = new CardType()
        {
            CardName = "card_poyi5",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.PoYi,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType PoYi6 = new CardType()
        {
            CardName = "card_poyi6",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.PoYi,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�������ɽ
        /// </summary>
        private static readonly CardType DiaoHuLiShan1 = new CardType()
        {
            CardName = "card_diaohulishan1",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.DiaoHuLiShan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�������ɽ
        /// </summary>
        private static readonly CardType DiaoHuLiShan2 = new CardType()
        {
            CardName = "card_diaohulishan2",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.DiaoHuLiShan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�������ɽ
        /// </summary>
        private static readonly CardType DiaoHuLiShan3 = new CardType()
        {
            CardName = "card_diaohulishan3",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.DiaoHuLiShan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�������ɽ
        /// </summary>
        private static readonly CardType DiaoHuLiShan4 = new CardType()
        {
            CardName = "card_diaohulishan4",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.DiaoHuLiShan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�������ɽ
        /// </summary>
        private static readonly CardType DiaoHuLiShan5 = new CardType()
        {
            CardName = "card_diaohulishan5",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.DiaoHuLiShan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�������ɽ
        /// </summary>
        private static readonly CardType DiaoHuLiShan6 = new CardType()
        {
            CardName = "card_diaohulishan6",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.DiaoHuLiShan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ���Ԯ
        /// </summary>
        private static readonly CardType ZengYuan1 = new CardType()
        {
            CardName = "card_zengyuan1",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ZengYuan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ���Ԯ
        /// </summary>
        private static readonly CardType ZengYuan2 = new CardType()
        {
            CardName = "card_zengyuan2",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ZengYuan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ���Ԯ
        /// </summary>
        private static readonly CardType ZengYuan3 = new CardType()
        {
            CardName = "card_zengyuan3",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ZengYuan,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ��ջ�
        /// </summary>
        private static readonly CardType ShaoHui1 = new CardType()
        {
            CardName = "card_shaohui1",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShaoHui,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ��ջ�
        /// </summary>
        private static readonly CardType ShaoHui2 = new CardType()
        {
            CardName = "card_shaohui2",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShaoHui,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ��ջ�
        /// </summary>
        private static readonly CardType ShaoHui3 = new CardType()
        {
            CardName = "card_shaohui3",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShaoHui,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType SuoDing1 = new CardType()
        {
            CardName = "card_suoding1",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.SuoDing,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType SuoDing2 = new CardType()
        {
            CardName = "card_suoding2",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.SuoDing,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType SuoDing3 = new CardType()
        {
            CardName = "card_suoding3",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Lock,
            Xiaoguo = Card_XiaoGuoType.SuoDing,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType SuoDing4 = new CardType()
        {
            CardName = "card_suoding4",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Lock,
            Xiaoguo = Card_XiaoGuoType.SuoDing,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType SuoDing5 = new CardType()
        {
            CardName = "card_suoding5",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.SuoDing,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType SuoDing6 = new CardType()
        {
            CardName = "card_suoding6",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.SuoDing,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType SuoDing7 = new CardType()
        {
            CardName = "card_suoding7",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.SuoDing,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType SuoDing8 = new CardType()
        {
            CardName = "card_suoding8",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.SuoDing,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�����
        /// 5���鱨���ƣ�����
        /// </summary>
        private static readonly CardType SuoDing9 = new CardType()
        {
            CardName = "card_suoding9",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Lock,
            Xiaoguo = Card_XiaoGuoType.SuoDing,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ����ѡ
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�����
        /// 5���鱨���ƣ�Σ���鱨
        /// </summary>
        private static readonly CardType WeiXianQinBao1 = new CardType()
        {
            CardName = "card_weixianqinbao1",
            Transmit = Card_TransmitType.Choose,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Lock,
            Xiaoguo = Card_XiaoGuoType.WeiXianQinBao,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ����ѡ
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�����
        /// 5���鱨���ƣ�Σ���鱨
        /// </summary>
        private static readonly CardType WeiXianQinBao2 = new CardType()
        {
            CardName = "card_weixianqinbao2",
            Transmit = Card_TransmitType.Choose,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Lock,
            Xiaoguo = Card_XiaoGuoType.WeiXianQinBao,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�ʶ��
        /// </summary>
        private static readonly CardType ShiPo1 = new CardType()
        {
            CardName = "card_shipo1",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShiPo,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�ʶ��
        /// </summary>
        private static readonly CardType ShiPo2 = new CardType()
        {
            CardName = "card_shipo2",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShiPo,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�ʶ��
        /// </summary>
        private static readonly CardType ShiPo3 = new CardType()
        {
            CardName = "card_shipo3",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShiPo,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�ʶ��
        /// </summary>
        private static readonly CardType ShiPo4 = new CardType()
        {
            CardName = "card_shipo4",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShiPo,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ���ѡ����
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�ʶ��
        /// </summary>
        private static readonly CardType ShiPo5 = new CardType()
        {
            CardName = "card_shipo5",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Choose,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShiPo,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�ʶ��
        /// </summary>
        private static readonly CardType ShiPo6 = new CardType()
        {
            CardName = "card_shipo6",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ShiPo,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�ת��
        /// </summary>
        private static readonly CardType ZhuanYi1 = new CardType()
        {
            CardName = "card_zhuanyi1",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Gray,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ZhuanYi,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�ת��
        /// </summary>
        private static readonly CardType ZhuanYi2 = new CardType()
        {
            CardName = "card_zhuanyi2",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Blue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ZhuanYi,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ���ܵ�
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ����ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ�ת��
        /// </summary>
        private static readonly CardType ZhuanYi3 = new CardType()
        {
            CardName = "card_zhuanyi3",
            Transmit = Card_TransmitType.MiDian,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.Red,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.ZhuanYi,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };

        /// <summary>
        /// 1�����ݷ�ʽ��ֱ��
        /// 2�����ݷ�����ʱ�뷽��
        /// 3���鱨��ɫ������ɫ
        /// 4���鱨��ǣ�δ����
        /// 5���鱨���ƣ������ļ�
        /// </summary>
        private static readonly CardType JiMiWenJian1 = new CardType()
        {
            CardName = "card_jimiwenjian1",
            Transmit = Card_TransmitType.ZhiDa,
            Direction = Card_DirectionType.Ni,
            Color = Card_ColorType.RedBlue,
            Lock = Card_LockType.Unlock,
            Xiaoguo = Card_XiaoGuoType.JiMiWenJian,
            Gongkaiwenben = Card_GongKaiWenBenType.None,
            Mimixiada = Card_MiMiXiaDaType.None,
            Shitan = Card_ShiTanType.None,
        };
        #endregion

        public static List<CardType> CardConfigList = new List<CardType>()
        {
            //GongKaiWenBen1, GongKaiWenBen2, GongKaiWenBen3, GongKaiWenBen4, GongKaiWenBen5,
            //MiMiXiaDa1, MiMiXiaDa2, MiMiXiaDa3, MiMiXiaDa4, MiMiXiaDa5, MiMiXiaDa6, MiMiXiaDa7, MiMiXiaDa8, MiMiXiaDa9,
            //ShiTan1, ShiTan2, ShiTan3, ShiTan4, ShiTan5, ShiTan6, ShiTan7, ShiTan8, ShiTan9,
            //DiaoBao1, DiaoBao1, DiaoBao2, DiaoBao2, DiaoBao3,
            //JieHuo1, JieHuo2, JieHuo3, JieHuo4, JieHuo5, JieHuo6,
            //LiJian1, LiJian2, LiJian3, LiJian3, LiJian3,
            //PoYi1, PoYi2, PoYi3, PoYi4, PoYi5, PoYi6,
            //DiaoHuLiShan1, DiaoHuLiShan2, DiaoHuLiShan3, DiaoHuLiShan4, DiaoHuLiShan5, DiaoHuLiShan6,
            //ZengYuan1, ZengYuan2, ZengYuan3,
            //ShaoHui1, ShaoHui1, ShaoHui1, ShaoHui2, ShaoHui3,
            //SuoDing1, SuoDing2, SuoDing3, SuoDing4, SuoDing5, SuoDing6, SuoDing7, SuoDing8, SuoDing9,
            //WeiXianQinBao1, WeiXianQinBao1, WeiXianQinBao1, WeiXianQinBao2, WeiXianQinBao2,
            //ShiPo1, ShiPo2, ShiPo3, ShiPo4, ShiPo5, ShiPo6,
            //ZhuanYi1, ZhuanYi1, ZhuanYi1, ZhuanYi2, ZhuanYi3,
            //JiMiWenJian1, JiMiWenJian1, JiMiWenJian1
            DiaoBao1, DiaoBao1, DiaoBao2, DiaoBao2, DiaoBao3,
            DiaoBao1, DiaoBao1, DiaoBao2, DiaoBao2, DiaoBao3,
            DiaoBao1, DiaoBao1, DiaoBao2, DiaoBao2, DiaoBao3,
            DiaoBao1, DiaoBao1, DiaoBao2, DiaoBao2, DiaoBao3,
            ShiPo1, ShiPo2, ShiPo3, ShiPo4, ShiPo5, ShiPo6,
            ShiPo1, ShiPo2, ShiPo3, ShiPo4, ShiPo5, ShiPo6,
            ShiPo1, ShiPo2, ShiPo3, ShiPo4, ShiPo5, ShiPo6,
            ShiPo1, ShiPo2, ShiPo3, ShiPo4, ShiPo5, ShiPo6,
        };

        public static List<CardType> GetNewCardList()
        {
            var list = CardConfigList.ToList();
            list.FisherYatesShuffle();
            return list;
        }

        public static ICard GetCardXiaoGuo(Card_XiaoGuoType card)
        {
            switch (card)
            {
                case Card_XiaoGuoType.DiaoBao:
                    return new DiaoBao();
                case Card_XiaoGuoType.ShiPo:
                    return new ShiPo();
                case Card_XiaoGuoType.PoYi:
                    return new PoYi();
                case Card_XiaoGuoType.ShaoHui:
                    return new ShaoHui();
                case Card_XiaoGuoType.ZengYuan:
                    return new ZengYuan();
            }
            return null;
        }

    }
}
