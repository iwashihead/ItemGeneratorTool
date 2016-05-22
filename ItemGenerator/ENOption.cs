using System.Collections;
using System.Collections.Generic;

/// <summary>�\�͒l���</summary>
public enum Ability {
    /// <summary>�q�b�g�|�C���g</summary>
    HP,
    /// <summary>�e�N�j�J���|�C���g</summary>
    TP,
    /// <summary>��</summary>
    STR,
    /// <summary>����</summary>
    VIT,
    /// <summary>�m��</summary>
    INT,
    /// <summary>��p</summary>
    DEX,
    /// <summary>�q��</summary>
    AGI,
    /// <summary>�^</summary>
    LUC,

    MAX
}

/// <summary>��퓬�X�L��</summary>
public enum ENSkill { Negotiation, BlackSmith, Alchemy, Cooking, Scout, Gather, MAX }

/// <summary>�������</summary>
public enum ENElement {
    /// <summary>1�Ԗ� �a������</summary>
    Slash,
    /// <summary>2�Ԗ� �Ō�����</summary>
    Strike,
    /// <summary>3�Ԗ� �h�ˑ���</summary>
    Thrust,
    /// <summary>4�Ԗ� ������</summary>
    Fire,
    /// <summary>5�Ԗ� ������</summary>
    Aqua,
    /// <summary>6�Ԗ� ������</summary>
    Thunder,
    /// <summary>7�Ԗ� �ő���</summary>
    Dark,
    /// <summary>8�Ԗ� ������</summary>
    Light,

    MAX
}

/// <summary>
/// �I�v�V���� - �ǉ�����
/// </summary>
public class ENOption
{
    public enum OptionType
    {
        /// <summary>
        /// <para>�X�e�[�^�X�{�[�i�X - �㏸or���~</para>
        /// <para>�e���x1 : �X�e�[�^�X�̎��, enum Ability�Q��</para>
        /// <para>�e���x2 : �㏸or���~����l</para>
        /// </summary>
        STATUS_ADD,
        /// <summary>
        /// <para>�X�e�[�^�X�{�[�i�X(����) - �㏸or���~</para>
        /// <para>�e���x1 : �X�e�[�^�X�̎��, enum Ability�Q��</para>
        /// <para>�e���x2 : �㏸or���~���銄��</para>
        /// <para> ex) �e���x1=3, �e���x2=10 �Ȃ� �uSTR��10���㏸�v �ƂȂ�</para>
        /// </summary>
        STATUS_MULTI,
        /// <summary>
        /// <para>�����ϐ��{�[�i�X(����) - �㏸or���~</para>
        /// <para>�e���x1 : �����̎��, enum ENElement�Q��</para>
        /// <para>�e���x2 : �㏸or���~���銄��</para>
        /// <para> ex) �e���x1=5, �e���x2=10 �Ȃ� �u���ϐ���10���㏸�v �ƂȂ�</para>
        /// </summary>
        ELEMENT_MULTI,
        /// <summary>
        /// <para>�o�b�h�X�e�[�^�X�ϐ��{�[�i�X(����) - �㏸or���~</para>
        /// <para>�e���x1 : �o�b�h�X�e�[�^�X�̎��, enum ENElement�Q��</para>
        /// <para>�e���x2 : �㏸or���~���銄��</para>
        /// <para> ex) �e���x1=5, �e���x2=10 �Ȃ� �u���ϐ���10���㏸�v �ƂȂ�</para>
        /// </summary>
        STATE_MULTI,
        /// <summary>�X�L���K��, �e���x1=�X�L��ID</summary>
        GAIN_SKILL,
        /// <summary>�h���b�v�A�C�e���{�[�i�X, �e���x1=�㏸%(20�Ȃ�20%�A�b�v)</summary>
        DROP_BONUS,
        /// <summary>�����̃h���b�v�{�[�i�X, �e���x1=�㏸%(20�Ȃ�20%�A�b�v)</summary>
        MONEY_BONUS,

        MAX
    }

    /// <summary>�I�v�V�����̎��</summary>
    public OptionType type;
    /// <summary>�e���l�P</summary>
    public int value1;
    /// <summary>�e���l�Q</summary>
    public int value2;
}

/// <summary>
/// �񋓌^���Q�[�����̕�����ɕϊ�����g�����\�b�h�p�N���X
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// �I�v�V�����̃Q�[�����\����������擾����
    /// ��jHP+100, �_���[�W32%�㏸ ...��
    /// </summary>
    public static string GetDesc(this ENOption op)
    {
        switch (op.type)
        {
            case ENOption.OptionType.STATUS_ADD:
                return string.Format(((Ability)op.value1).DispName() + "+{0}", op.value2);
            default:
                return string.Empty;
        }
    }
    /// <summary>�Q�[������������擾���܂�</summary>
    public static string DispName(this Ability abl)
    {
        switch (abl)
        {
            case Ability.AGI:
                return "�q��";
            case Ability.INT:
                return "�m��";
            case Ability.HP:
                return "���C�t";
            case Ability.LUC:
                return "�^";
            case Ability.TP:
                return "�}�i";
            case Ability.STR:
                return "��";
            case Ability.DEX:
                return "��p";
            case Ability.VIT:
                return "����";
            default:
                return string.Empty;
        }
    }
    /// <summary>�Q�[������������擾���܂�</summary>
    public static string DispName(this MasterItem.Group val)
    {
        switch (val)
        {
            case MasterItem.Group.Book:
                return "�{�E����";
            case MasterItem.Group.Food:
                return "����";
            case MasterItem.Group.FoodSrc:
                return "�H��";
            case MasterItem.Group.Herb:
                return "�n�[�u";
            case MasterItem.Group.Key:
                return "��";
            case MasterItem.Group.Lacryma:
                return "������";
            case MasterItem.Group.Misc:
                return "���̑�";
            case MasterItem.Group.Potion:
                return "��i";
            case MasterItem.Group.Seed:
                return "��";
            case MasterItem.Group.Tool:
                return "����";
            case MasterItem.Group.Torch:
                return "�����܂�";
            default:
                return string.Empty;
        }
    }
    /// <summary>�Q�[������������擾���܂�</summary>
    public static string DispName(this MasterWeapon.Group val)
    {
        switch (val)
        {
            case MasterWeapon.Group.Axe:
                return "��";
            case MasterWeapon.Group.Bow:
                return "�|��";
            case MasterWeapon.Group.Hammer:
                return "�݊�";
            case MasterWeapon.Group.Spear:
                return "��";
            case MasterWeapon.Group.Staff:
                return "��";
            case MasterWeapon.Group.Sword:
                return "��";
            default:
                return string.Empty;
        }
    }
    /// <summary>�Q�[������������擾���܂�</summary>
    public static string DispName(this MasterArmor.Group val)
    {
        switch (val)
        {
            case MasterArmor.Group.Body:
                return "�Z";
            case MasterArmor.Group.Hand:
                return "����";
            case MasterArmor.Group.Head:
                return "�X�q";
            case MasterArmor.Group.Leg:
                return "�C";
            case MasterArmor.Group.Shield:
                return "��";
            default:
                return string.Empty;
        }
    }
    /// <summary>�Q�[������������擾���܂�</summary>
    public static string DispName(this MasterAcce.Group val)
    {
        switch (val)
        {
            case MasterAcce.Group.Orb:
                return "�I�[�u";
            case MasterAcce.Group.Pendant:
                return "�����";
            case MasterAcce.Group.Ring:
                return "�w��";
            default:
                return string.Empty;
        }
    }
    /// <summary>�Q�[������������擾���܂�</summary>
    public static string DispName(this MasterMaterial.Group val)
    {
        switch (val)
        {
            case MasterMaterial.Group.Bone:
                return "���E��";
            case MasterMaterial.Group.Cotton:
                return "��";
            case MasterMaterial.Group.Leather:
                return "�v";
            case MasterMaterial.Group.Metal:
                return "����";
            case MasterMaterial.Group.Stone:
                return "�΍�";
            case MasterMaterial.Group.Wood:
                return "�؍�";
            default:
                return string.Empty;
        }
    }
    /// <summary>�Q�[������������擾���܂�</summary>
    public static string DispName(this ENElement val)
    {
        switch (val)
        {
            case ENElement.Aqua:
                return "��";
            case ENElement.Dark:
                return "��";
            case ENElement.Fire:
                return "��";
            case ENElement.Light:
                return "��";
            case ENElement.Slash:
                return "�ؒf";
            case ENElement.Strike:
                return "�Ō�";
            case ENElement.Thrust:
                return "�h��";
            case ENElement.Thunder:
                return "��";
            default:
                return string.Empty;
        }
    }
    /// <summary>�Q�[������������擾���܂�</summary>
    public static string DispName(this MasterItem.EffectType val)
    {
        switch (val)
        {
            case MasterItem.EffectType.AddOption:
                return "�A�C�e���ɃI�v�V�����������_���ɒǉ�����";
            case MasterItem.EffectType.AddState:
                return "��Ԉُ��t������";
            case MasterItem.EffectType.CreateMaterial:
                return "�}�e���A���������_���ɐ�������";
            case MasterItem.EffectType.HealL:
                return "���C�t���񕜂���";
            case MasterItem.EffectType.HealLM:
                return "���C�t�ƃ}�i���񕜂���";
            case MasterItem.EffectType.HealM:
                return "�}�i���񕜂���";
            case MasterItem.EffectType.HealState:
                return "��Ԉُ���񕜂���";
            case MasterItem.EffectType.MixOption:
                return "2�̃A�C�e���������_����������";
            case MasterItem.EffectType.None:
                return "�Ȃ�";
            case MasterItem.EffectType.Regeneration:
                return "���C�t�̎�����";
            case MasterItem.EffectType.Revive:
                return "�퓬�s�\����̉�";
            case MasterItem.EffectType.SubOption:
                return "�A�C�e���̃I�v�V����������";
            case MasterItem.EffectType.Torch:
                return "���͂��Ƃ炷";
            case MasterItem.EffectType.Skill:
                return "�퓬���Ɏw��X�L�������s����";
            default:
                return string.Empty;
        }
    }
    /// <summary>val1��val2�̐ݒ������\������</summary>
    public static string DispDescription(this MasterItem.EffectType val)
    {
        switch (val)
        {
            case MasterItem.EffectType.AddOption:
                return "�I�v�V���������_���ǉ�" + 
                    "\nval1=�Œ�I�v�V���������N \nval2=�ō��I�v�V���������N";
            case MasterItem.EffectType.AddState:
                return "��Ԉُ��t������" +
                    "\nval1=�X�e�[�gID\nval2=�t���^�[����";
            case MasterItem.EffectType.CreateMaterial:
                return "�}�e���A���������_���ɐ�������" +
                    "\nval1=�Œ�}�e���A�������N\nval2=�ō��}�e���A�������N";
            case MasterItem.EffectType.HealL:
                return "���C�t���񕜂���" +
                    "\nval1=�񕜗�\nval2=1:�͈́i �����S��, ����ȊO:�P�� �j";
            case MasterItem.EffectType.HealLM:
                return "���C�t�ƃ}�i���񕜂���" +
                    "\nval1=�񕜗�\nval2=1:�͈́i �����S��, ����ȊO:�P�� �j";
            case MasterItem.EffectType.HealM:
                return "�}�i���񕜂���" +
                    "\nval1=�񕜗�\nval2=1:�͈́i �����S��, ����ȊO:�P�� �j";
            case MasterItem.EffectType.HealState:
                return "��Ԉُ���񕜂���" +
                    "\nval1=�X�e�[�gID\nval2=";
            case MasterItem.EffectType.MixOption:
                return "2�̃A�C�e���������_����������" +
                    "\nval1=\nval2=";
            case MasterItem.EffectType.None:
                return "�Ȃ�";
            case MasterItem.EffectType.Regeneration:
                return "���C�t�̎�����" +
                    "\nval1=�񕜗�(����)\nval2=�����^�[����";
            case MasterItem.EffectType.Revive:
                return "�퓬�s�\����̉�" +
                    "\nval1=��������HP�񕜗��i���j\nval2=�������i���j";
            case MasterItem.EffectType.SubOption:
                return "�A�C�e���̃I�v�V����������" +
                    "\nval1=�������i���j\nval2=";
            case MasterItem.EffectType.Torch:
                return "���͂��Ƃ炷" +
                    "\nval1=���邳�iMax100�j\nval2=�����^�[����";
            case MasterItem.EffectType.Skill:
                return "�퓬���Ɏw��X�L�������s" +
                    "\nval1=�X�L��ID\nval2=1:�ړ����̎g�p�𐧌�, ����ȊO:�Ή��X�L���̎d�l�ɏ�����";
            default:
                return string.Empty;
        }
    }
    /// <summary>�Q�[������������擾���܂�</summary>
    public static string DispName(this ENOption.OptionType val)
    {
        return val.ToString();
        switch (val)
        {
            default:
                return string.Empty;
        }
    }
    /*
                 for (i = 0; i < (int)MasterItem.Group.MAX; i++)
                comboBoxItemType.Items.Add(((MasterItem.Group)i).ToString());
            for (i = 0; i < (int)MasterItem.EffectType.MAX; i++)
                comboBoxItemEffecr.Items.Add(((MasterItem.EffectType)i).ToString());
            for (i = 0; i < (int)MasterWeapon.Group.MAX; i++)
                comboBoxWeaponType.Items.Add(((MasterWeapon.Group)i).ToString());
            for (i = 0; i < (int)ENElement.MAX; i++)
                comboBoxWeaponElement.Items.Add(((ENElement)i).ToString());
            for (i = 0; i < (int)MasterArmor.Group.MAX; i++)
                comboBoxArmorType.Items.Add(((MasterArmor.Group)i).ToString());
            for (i = 0; i < (int)ENOption.OptionType.MAX; i++)
                comboBoxWeaponOptionType.Items.Add(((ENOption.OptionType)i).ToString());
            for (i = 0; i < (int)MasterAcce.Group.MAX; i++)
                comboBoxAcceType.Items.Add(((MasterAcce.Group)i).ToString());
            for (i = 0; i < (int)MasterMaterial.Group.MAX; i++)
                comboBoxMaterialType.Items.Add(((MasterMaterial.Group)i).ToString());
     */
}