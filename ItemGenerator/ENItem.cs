using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>�A�C�e�����</summary>
public enum ItemType
{ Item, Weapon, Armor, Acce, Material,        MAX }

/// <summary>
/// �}�X�^�[�A�C�e���n�̊��N���X
/// </summary>
public class ItemBase : IComparable
{
    /// <summary>�f�[�^�̋�؂�p����</summary>
    protected const char SPLIT = ',';
    /// <summary>�I�v�V�����p �l��؂蕶��</summary>
    protected const char SPLIT_OP = '|';
    /// <summary>�I�v�V�����p �C���f�b�N�X��؂蕶��</summary>
    protected const char SPLIT_OP_END = ';';

    // ���f�[�^
    /// <summary>index:0 �h�������^�C�v���ƂɊ���U��ꂽID</summary>
    public int id;
    /// <summary>index:1 ���O</summary>
    public string name;
    /// <summary>index:2 ����</summary>
    public string desc;
    /// <summary>index:3 �A�C�e���̑啔��</summary>
    public ItemType type;
    /// <summary>index:4 �A�C�R���ԍ�</summary>
    public int icon_id;
    /// <summary>index:5 �l�i</summary>
    public int cost;
    /// <summary>index:6 �����N�A���A�x</summary>
    public int rank;
    /// <summary>index:7 �d��</summary>
    public float weight;

    // �R���X�g���N�^
    public ItemBase()
    {
    }

    public virtual string Serialize()
    {
        string s;
        s = id.ToString() + SPLIT +
            name + SPLIT +
            desc + SPLIT +
            (int)type + SPLIT +
            icon_id.ToString() + SPLIT +
            cost.ToString() + SPLIT +
            rank.ToString() + SPLIT +
            weight.ToString("f1");

        return s;
    }
    public virtual int Deserialize(string src)
    {
        string[] a = src.Split(SPLIT);
        if (a.Length < 8)
        {
            Console.WriteLine("ItemBase Deserialize Error!");
            return -1;
        }

        id = Convert.ToInt32(a[0]);
        name = a[1];
        desc = a[2];
        type = (ItemType)Convert.ToInt32(a[3]);
        icon_id = Convert.ToInt32(a[4]);
        cost = Convert.ToInt32(a[5]);
        rank = Convert.ToInt32(a[6]);
        weight = (float)Convert.ToDouble(a[7]);

        return 0;
    }

    public int CompareTo(object obj)
    {
        return id - ((ItemBase)obj).id;
    }

    public override string ToString()
    {
        return string.Format("{0:D4}  :  " + name, id);
    }
}

/// <summary>
/// ����}�X�^�[�N���X
/// </summary>
public class MasterItem : ItemBase
{
    /// <summary>�����</summary>
    public enum Group
    {
        Seed,
    	Herb,
        Potion,
        Tool,
        Book,
        FoodSrc,
        Food,
        Key,
        Lacryma,
        Torch,
        Misc,

        MAX
    }
    /// <summary>index:8 ����</summary>
    public Group group;
    /// <summary>index:9 �X�^�b�N�\���ǂ���</summary>
    public bool stockEnable;
    /// <summary>index:10 �ő�X�^�b�N���AstackEnable��true�̏ꍇ�̂ݗL��</summary>
    public int stockMax;

    /// <summary>
    /// �A�C�e���̎g�p���� �񋓌^
    /// </summary>
    public enum EffectType
    {
        /// <summary>�g�p�s��</summary>
        None,
        /// <summary>���C�t�� val1=�񕜗�, val2=�񕜁�</summary>
        HealL,
        /// <summary>�}�i�� val1=�񕜗�, val2=�񕜁�</summary>
        HealM,
        /// <summary>���C�t�}�i�� val1=���C�t�񕜁�, val2=�}�i�񕜁�</summary>
        HealLM,
        /// <summary>��Ԉُ��(���S�ȊO) val1=���C�t�񕜗�, val2=���C�t�񕜁�</summary>
        HealState,
        /// <summary>��Ԉُ�t��</summary>
        AddState,
        /// <summary>���S����̕���</summary>
        Revive,
        /// <summary>���W�F�l(������) val1=�^�[�����񕜗�, val2=�񕜃^�[����</summary>
        Regeneration,
        /// <summary>�I�v�V��������</summary>
        MixOption,
        /// <summary>�I�v�V���������_���ǉ�</summary>
        AddOption,
        /// <summary>�I�v�V����������</summary>
        SubOption,
        /// <summary>�����܂�</summary>
        Torch,
        /// <summary>�}�e���A�������_������</summary>
        CreateMaterial,
        /// <summary>�퓬���Ɏw��X�L�������s</summary>
        Skill,

        MAX
    }

    /// <summary>index:11 �g�p����</summary>
    public EffectType effectType;
    /// <summary>index:12 �g�p���ʗp�ϐ��P</summary>
    public int val1;
    /// <summary>index:13 �g�p���ʗp�ϐ��P</summary>
    public int val2;

    // �A�C�e���g�p�f���Q�[�g

    public MasterItem()
    {
    }

    public override string Serialize()
    {
        string s;
        s = base.Serialize();
        s += SPLIT + 
            ((int)group).ToString() + SPLIT +
            ((stockEnable) ? "1" : "0") + SPLIT +
            stockMax.ToString() + SPLIT +
            ((int)effectType).ToString() + SPLIT +
            val1.ToString() + SPLIT +
            val2.ToString();

        return s;
    }
    public override int Deserialize(string src)
    {
//        base.Deserialize(src);
        string[] a = src.Split(SPLIT);
        if (a.Length < 13)
        {
            Console.WriteLine(a.Length + "MasterItem Deserialize Error!");
            return - 1;
        }
        // �f�[�^�쐬
        id = Convert.ToInt32(a[0]);
        name = a[1];
        desc = a[2];
        type = (ItemType)Convert.ToInt32(a[3]);
        icon_id = Convert.ToInt32(a[4]);
        cost = Convert.ToInt32(a[5]);
        rank = Convert.ToInt32(a[6]);
        weight = (float)Convert.ToDouble(a[7]);
        group = (Group)Convert.ToInt32(a[8]);
        stockEnable = (a[9] == "1") ? true : false;
        stockMax = Convert.ToInt32(a[10]);
        effectType = (EffectType)(Convert.ToInt32(a[11]));
        val1 = Convert.ToInt32(a[12]);
        val2 = Convert.ToInt32(a[13]);

        return 0;
    }
}


/// <summary>
/// ����}�X�^�[�N���X
/// </summary>
public class MasterWeapon : ItemBase
{
    /// <summary>���핪��</summary>
    public enum Group
    {
        Sword,
        Axe,
        Spear,
        Bow,
        Staff,
        Hammer,
        MAX
    }
    /// <summary>index:8 ����</summary>
    public Group group;
    /// <summary>index:9 ����</summary>
    public ENElement element;
    /// <summary>index:10 �����U����</summary>
    public int atk;
    /// <summary>index:11 ���@�U����</summary>
    public int matk;
    /// <summary>index:12 �����h���</summary>
    public int def;
    /// <summary>index:13 ���@�h���</summary>
    public int mdef;
    /// <summary>index:14 ����</summary>
    public int hit;
    /// <summary>index:15 �N���e�B�J��</summary>
    public int crt;
    /// <summary>index:16 ���</summary>
    public int eva;
    /// <summary>index:17 ���莝���t���O</summary>
    public bool isTwoHand;

    /// <summary>index:18 �I�v�V���� - ���j�[�N�p</summary>
    public List<ENOption> options;

    public MasterWeapon()
    {
        type = ItemType.Weapon;
    }

    public override string Serialize()
    {
        string s;
        s = base.Serialize();
        s += SPLIT +
            ((int)group).ToString() + SPLIT +
            (int)element + SPLIT +
            atk + SPLIT +
            matk + SPLIT +
            def + SPLIT +
            mdef + SPLIT +
            hit + SPLIT +
            crt + SPLIT +
            eva + SPLIT +
            ((isTwoHand) ? "1" : "0") + SPLIT;

        // �I�v�V����
        if (options != null && options.Count > 0)
        {
            for (int i = 0; i < options.Count; i++)
            {
                s += "" + (int)options[i].type + SPLIT_OP +
                    options[i].value1 + SPLIT_OP +
                    options[i].value2;
                if (i != options.Count - 1)
                    s += SPLIT_OP_END;
            }
        }

        return s;
    }
    public override int Deserialize(string src)
    {
        //        base.Deserialize(src);
        string[] a = src.Split(SPLIT);
        if (a.Length < 18)
        {
            Console.WriteLine(a.Length + "MasterWeapon Deserialize Error!");
            return -1;
        }
        // �f�[�^�쐬
        id = Convert.ToInt32(a[0]);
        name = a[1];
        desc = a[2];
        type = (ItemType)Convert.ToInt32(a[3]);
        icon_id = Convert.ToInt32(a[4]);
        cost = Convert.ToInt32(a[5]);
        rank = Convert.ToInt32(a[6]);
        weight = (float)Convert.ToDouble(a[7]);
        group = (Group)Convert.ToInt32(a[8]);
        element = (ENElement)Convert.ToInt32(a[9]);
        atk = Convert.ToInt32(a[10]);
        matk = Convert.ToInt32(a[11]);
        def = Convert.ToInt32(a[12]);
        mdef = Convert.ToInt32(a[13]);
        hit = Convert.ToInt32(a[14]);
        crt = Convert.ToInt32(a[15]);
        eva = Convert.ToInt32(a[16]);
        isTwoHand = (a[17] == "1") ? true : false;

        // �I�v�V����
        if (a.Length > 18)
        {
            a[18] = a[18].Replace("\n", "");
            a[18] = a[18].Replace("\r", "");
            string[] op = a[18].Split(new char[]{ SPLIT_OP_END }, StringSplitOptions.RemoveEmptyEntries);
            if (op.Length > 0)
            {
                options = new List<ENOption>();
                for (int i = 0; i < op.Length; i++)
                {
                    string[] str = op[i].Split(SPLIT_OP);
                    if (str.Length != 3)
                    {
                        Console.WriteLine("����I�v�V������O�I");
                        continue;
                    }
                    ENOption option = new ENOption();
                    option.type = (ENOption.OptionType)Convert.ToInt32(str[0]);
                    option.value1 = Convert.ToInt32(str[1]);
                    option.value2 = Convert.ToInt32(str[2]);

                    // �I�v�V�����ǉ�
                    options.Add(option);
                }
            }
        }

        return 0;
    }
}

/// <summary>
/// �}�X�^�[�h��N���X
/// </summary>
public class MasterArmor : ItemBase
{
    public enum Group
    {
        Body,
        Hand,
        Head,
        Leg,
        Shield,
        MAX
    }
    /// <summary>index:8 �h���</summary>
    public Group group;
    /// <summary>index:9 �����h���</summary>
    public int def;
    /// <summary>index:10 ���@�h���</summary>
    public int mdef;
    /// <summary>index:11 ���</summary>
    public int eva;

    public MasterArmor()
    {
        type = ItemType.Armor;
    }

    public override string Serialize()
    {
        string s;
        s = base.Serialize();
        s += SPLIT + 
            ((int)group).ToString() + SPLIT +
            def + SPLIT +
            mdef + SPLIT +
            eva;

        return s;
    }
    public override int Deserialize(string src)
    {
        //        base.Deserialize(src);
        string[] a = src.Split(SPLIT);
        if (a.Length < 11)
        {
            Console.WriteLine(a.Length + "MasterWeapon Deserialize Error!");
            return -1;
        }
        // �f�[�^�쐬
        id = Convert.ToInt32(a[0]);
        name = a[1];
        desc = a[2];
        type = (ItemType)Convert.ToInt32(a[3]);
        icon_id = Convert.ToInt32(a[4]);
        cost = Convert.ToInt32(a[5]);
        rank = Convert.ToInt32(a[6]);
        weight = (float)Convert.ToDouble(a[7]);
        group = (Group)Convert.ToInt32(a[8]);
        def = Convert.ToInt32(a[9]);
        mdef = Convert.ToInt32(a[10]);
        eva = Convert.ToInt32(a[11]);

        return 0;
    }
}

/// <summary>
/// �}�X�^�[�A�N�Z�T���N���X
/// </summary>
public class MasterAcce : ItemBase
{
    public enum Group
    {
        Ring,
        Pendant,
        Orb,
        MAX
    }
    /// <summary>index:8 �A�N�Z�T������</summary>
    public Group group;
    /// <summary>index:9 �����U����</summary>
    public int atk;
    /// <summary>index:10 ���@�U����</summary>
    public int matk;
    /// <summary>index:11 �����h���</summary>
    public int def;
    /// <summary>index:12 ���@�h���</summary>
    public int mdef;
    /// <summary>index:13 ����</summary>
    public int hit;
    /// <summary>index:14 �N���e�B�J��</summary>
    public int crt;
    /// <summary>index:15 ���</summary>
    public int eva;
    /// <summary>index:16~ �\�͒l�㏸</summary>
    public int[] ability = new int[(int)Ability.MAX];

    public MasterAcce()
    {
        type = ItemType.Acce;
    }

    public override string Serialize()
    {
        string s;
        s = base.Serialize();
        s += SPLIT +
            ((int)group).ToString() + SPLIT +
            atk + SPLIT +
            matk + SPLIT +
            def + SPLIT +
            mdef + SPLIT +
            hit + SPLIT +
            crt + SPLIT +
            eva + SPLIT +
            ability[(int)Ability.HP] + SPLIT +
            ability[(int)Ability.TP] + SPLIT +
            ability[(int)Ability.STR] + SPLIT +
            ability[(int)Ability.VIT] + SPLIT +
            ability[(int)Ability.INT] + SPLIT +
            ability[(int)Ability.DEX] + SPLIT +
            ability[(int)Ability.AGI] + SPLIT +
            ability[(int)Ability.LUC];
        // index : 23

        return s;
    }
    public override int Deserialize(string src)
    {
        //        base.Deserialize(src);
        string[] a = src.Split(SPLIT);
        if (a.Length < 23)
        {
            Console.WriteLine(a.Length + "MasterAcce Deserialize Error!");
            return -1;
        }
        // �f�[�^�쐬
        id = Convert.ToInt32(a[0]);
        name = a[1];
        desc = a[2];
        type = (ItemType)Convert.ToInt32(a[3]);
        icon_id = Convert.ToInt32(a[4]);
        cost = Convert.ToInt32(a[5]);
        rank = Convert.ToInt32(a[6]);
        weight = (float)Convert.ToDouble(a[7]);
        group = (Group)Convert.ToInt32(a[8]);
        atk = Convert.ToInt32(a[9]);
        matk = Convert.ToInt32(a[10]);
        def = Convert.ToInt32(a[11]);
        mdef = Convert.ToInt32(a[12]);
        hit = Convert.ToInt32(a[13]);
        crt = Convert.ToInt32(a[14]);
        eva = Convert.ToInt32(a[15]);
        ability[(int)Ability.HP] = Convert.ToInt32(a[16]);
        ability[(int)Ability.TP] = Convert.ToInt32(a[17]);
        ability[(int)Ability.STR] = Convert.ToInt32(a[18]);
        ability[(int)Ability.VIT] = Convert.ToInt32(a[19]);
        ability[(int)Ability.INT] = Convert.ToInt32(a[20]);
        ability[(int)Ability.DEX] = Convert.ToInt32(a[21]);
        ability[(int)Ability.AGI] = Convert.ToInt32(a[22]);
        ability[(int)Ability.LUC] = Convert.ToInt32(a[23]);

        return 0;
    }
}

/// <summary>
/// �}�X�^�[�}�e���A���N���X
/// </summary>
public class MasterMaterial : ItemBase
{
    public enum Group
    {
        Metal,
        Stone,
        Wood,
        Bone,
        Leather,
        Cotton,

        MAX
    }
    /// <summary>index:8 �}�e���A������</summary>
    public Group group;
    /// <summary>index:9 �����U����</summary>
    public int atk;
    /// <summary>index:10 ���@�U����</summary>
    public int matk;
    /// <summary>index:11 �����h��e����</summary>
    public int def;
    /// <summary>index:12 ���@�h��e����</summary>
    public int mdef;
    /// <summary>index:13 �����e����</summary>
    public int hit;
    /// <summary>index:14 �N���e�B�J���e����</summary>
    public int crt;
    /// <summary>index:15 ����e����</summary>
    public int eva;
    /// <summary>index:16~23 �\�͒l</summary>
    public int[] ability = new int[(int)Ability.MAX];
    /// <summary>index:24~ ������R</summary>
    public int[] regist = new int[(int)ENElement.MAX];
    /// <summary>index:32 �ړ��� �A�C�e�����̑O�ɕ\������܂�</summary>
    public string epithet;

    public MasterMaterial()
    {
        type = ItemType.Material;
    }

    public override string Serialize()
    {
        string s;
        s = base.Serialize();
        s += SPLIT +
            ((int)group).ToString() + SPLIT +
            atk + SPLIT +
            matk + SPLIT +
            def + SPLIT +
            mdef + SPLIT +
            hit + SPLIT +
            crt + SPLIT +
            eva + SPLIT +
            ability[(int)Ability.HP] + SPLIT +
            ability[(int)Ability.TP] + SPLIT +
            ability[(int)Ability.STR] + SPLIT +
            ability[(int)Ability.VIT] + SPLIT +
            ability[(int)Ability.INT] + SPLIT +
            ability[(int)Ability.DEX] + SPLIT +
            ability[(int)Ability.AGI] + SPLIT +
            ability[(int)Ability.LUC] + SPLIT +
            regist[(int)ENElement.Slash] + SPLIT +
            regist[(int)ENElement.Strike] + SPLIT +
            regist[(int)ENElement.Thrust] + SPLIT +
            regist[(int)ENElement.Fire] + SPLIT +
            regist[(int)ENElement.Aqua] + SPLIT +
            regist[(int)ENElement.Thunder] + SPLIT +
            regist[(int)ENElement.Dark] + SPLIT +
            regist[(int)ENElement.Light] + SPLIT +
            epithet;
        // index : 32

        return s;
    }
    public override int Deserialize(string src)
    {
        //        base.Deserialize(src);
        string[] a = src.Split(SPLIT);
        if (a.Length < 32)
        {
            Console.WriteLine(a.Length + "MasterAcce Deserialize Error!");
            return -1;
        }
        // �f�[�^�쐬
        id = Convert.ToInt32(a[0]);
        name = a[1];
        desc = a[2];
        type = (ItemType)Convert.ToInt32(a[3]);
        icon_id = Convert.ToInt32(a[4]);
        cost = Convert.ToInt32(a[5]);
        rank = Convert.ToInt32(a[6]);
        weight = (float)Convert.ToDouble(a[7]);
        group = (Group)Convert.ToInt32(a[8]);
        atk = Convert.ToInt32(a[9]);
        matk = Convert.ToInt32(a[10]);
        def = Convert.ToInt32(a[11]);
        mdef = Convert.ToInt32(a[12]);
        hit = Convert.ToInt32(a[13]);
        crt = Convert.ToInt32(a[14]);
        eva = Convert.ToInt32(a[15]);
        ability[(int)Ability.HP] = Convert.ToInt32(a[16]);
        ability[(int)Ability.TP] = Convert.ToInt32(a[17]);
        ability[(int)Ability.STR] = Convert.ToInt32(a[18]);
        ability[(int)Ability.VIT] = Convert.ToInt32(a[19]);
        ability[(int)Ability.INT] = Convert.ToInt32(a[20]);
        ability[(int)Ability.DEX] = Convert.ToInt32(a[21]);
        ability[(int)Ability.AGI] = Convert.ToInt32(a[22]);
        ability[(int)Ability.LUC] = Convert.ToInt32(a[23]);
        regist[(int)ENElement.Slash] = Convert.ToInt32(a[24]);
        regist[(int)ENElement.Strike] = Convert.ToInt32(a[25]);
        regist[(int)ENElement.Thrust] = Convert.ToInt32(a[26]);
        regist[(int)ENElement.Fire] = Convert.ToInt32(a[27]);
        regist[(int)ENElement.Aqua] = Convert.ToInt32(a[28]);
        regist[(int)ENElement.Thunder] = Convert.ToInt32(a[29]);
        regist[(int)ENElement.Dark] = Convert.ToInt32(a[30]);
        regist[(int)ENElement.Light] = Convert.ToInt32(a[31]);
        epithet = a[32];

        return 0;
    }
}


/// <summary>
/// �A�C�e�����̂̊��N���X
/// </summary>
public class ItemData
{
    /// <summary>�A�C�e������</summary>
    public ItemType type;
    /// <summary>�A�C�e���ԍ�</summary>
    public int id;

    public ItemData()
    {
    }
    public ItemData(ItemType type, int id)
    {
        this.type = type;
        this.id = id;
    }
    public ItemData(MasterItem item, int id)
    {
        this.type = ItemType.Item;
        this.id = id;
    }
    public ItemData(MasterWeapon item, int id)
    {
        this.type = ItemType.Weapon;
        this.id = id;
    }
    public ItemData(MasterArmor item, int id)
    {
        this.type = ItemType.Armor;
        this.id = id;
    }
    public ItemData(MasterAcce item, int id)
    {
        this.type = ItemType.Acce;
        this.id = id;
    }
    public ItemData(MasterMaterial item, int id)
    {
        this.type = ItemType.Material;
        this.id = id;
    }
}

/// <summary>
/// ����A�C�e���i���́j
/// </summary>
public class Item : ItemData
{
    /// <summary>�i�� - �X�^�b�N�\�ȃA�C�e���̏ꍇ�A�e���Ȃ�</summary>
    public int quality;
    /// <summary>�X�^�b�N�� - �X�^�b�N�s�̏ꍇ�A�e���Ȃ�</summary>
    public int stack;
}