using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>アイテム種別</summary>
public enum ItemType
{ Item, Weapon, Armor, Acce, Material,        MAX }

/// <summary>
/// マスターアイテム系の基底クラス
/// </summary>
public class ItemBase : IComparable
{
    /// <summary>データの区切り用文字</summary>
    protected const char SPLIT = ',';
    /// <summary>オプション用 値区切り文字</summary>
    protected const char SPLIT_OP = '|';
    /// <summary>オプション用 インデックス区切り文字</summary>
    protected const char SPLIT_OP_END = ';';

    // ▼データ
    /// <summary>index:0 派生したタイプごとに割り振られたID</summary>
    public int id;
    /// <summary>index:1 名前</summary>
    public string name;
    /// <summary>index:2 説明</summary>
    public string desc;
    /// <summary>index:3 アイテムの大部類</summary>
    public ItemType type;
    /// <summary>index:4 アイコン番号</summary>
    public int icon_id;
    /// <summary>index:5 値段</summary>
    public int cost;
    /// <summary>index:6 ランク、レア度</summary>
    public int rank;
    /// <summary>index:7 重量</summary>
    public float weight;

    // コンストラクタ
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
/// 道具マスタークラス
/// </summary>
public class MasterItem : ItemBase
{
    /// <summary>道具分類</summary>
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
    /// <summary>index:8 分類</summary>
    public Group group;
    /// <summary>index:9 スタック可能かどうか</summary>
    public bool stockEnable;
    /// <summary>index:10 最大スタック数、stackEnableがtrueの場合のみ有効</summary>
    public int stockMax;

    /// <summary>
    /// アイテムの使用効果 列挙型
    /// </summary>
    public enum EffectType
    {
        /// <summary>使用不可</summary>
        None,
        /// <summary>ライフ回復 val1=回復量, val2=回復％</summary>
        HealL,
        /// <summary>マナ回復 val1=回復量, val2=回復％</summary>
        HealM,
        /// <summary>ライフマナ回復 val1=ライフ回復％, val2=マナ回復％</summary>
        HealLM,
        /// <summary>状態異常回復(死亡以外) val1=ライフ回復量, val2=ライフ回復％</summary>
        HealState,
        /// <summary>状態異常付加</summary>
        AddState,
        /// <summary>死亡からの復活</summary>
        Revive,
        /// <summary>リジェネ(自動回復) val1=ターン毎回復量, val2=回復ターン数</summary>
        Regeneration,
        /// <summary>オプション合成</summary>
        MixOption,
        /// <summary>オプションランダム追加</summary>
        AddOption,
        /// <summary>オプションを消す</summary>
        SubOption,
        /// <summary>たいまつ</summary>
        Torch,
        /// <summary>マテリアルランダム生成</summary>
        CreateMaterial,
        /// <summary>戦闘中に指定スキルを実行</summary>
        Skill,

        MAX
    }

    /// <summary>index:11 使用効果</summary>
    public EffectType effectType;
    /// <summary>index:12 使用効果用変数１</summary>
    public int val1;
    /// <summary>index:13 使用効果用変数１</summary>
    public int val2;

    // アイテム使用デリゲート

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
        // データ作成
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
/// 武器マスタークラス
/// </summary>
public class MasterWeapon : ItemBase
{
    /// <summary>武器分類</summary>
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
    /// <summary>index:8 分類</summary>
    public Group group;
    /// <summary>index:9 属性</summary>
    public ENElement element;
    /// <summary>index:10 物理攻撃力</summary>
    public int atk;
    /// <summary>index:11 魔法攻撃力</summary>
    public int matk;
    /// <summary>index:12 物理防御力</summary>
    public int def;
    /// <summary>index:13 魔法防御力</summary>
    public int mdef;
    /// <summary>index:14 命中</summary>
    public int hit;
    /// <summary>index:15 クリティカル</summary>
    public int crt;
    /// <summary>index:16 回避</summary>
    public int eva;
    /// <summary>index:17 両手持ちフラグ</summary>
    public bool isTwoHand;

    /// <summary>index:18 オプション - ユニーク用</summary>
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

        // オプション
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
        // データ作成
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

        // オプション
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
                        Console.WriteLine("武器オプション例外！");
                        continue;
                    }
                    ENOption option = new ENOption();
                    option.type = (ENOption.OptionType)Convert.ToInt32(str[0]);
                    option.value1 = Convert.ToInt32(str[1]);
                    option.value2 = Convert.ToInt32(str[2]);

                    // オプション追加
                    options.Add(option);
                }
            }
        }

        return 0;
    }
}

/// <summary>
/// マスター防具クラス
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
    /// <summary>index:8 防具分類</summary>
    public Group group;
    /// <summary>index:9 物理防御力</summary>
    public int def;
    /// <summary>index:10 魔法防御力</summary>
    public int mdef;
    /// <summary>index:11 回避</summary>
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
        // データ作成
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
/// マスターアクセサリクラス
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
    /// <summary>index:8 アクセサリ分類</summary>
    public Group group;
    /// <summary>index:9 物理攻撃力</summary>
    public int atk;
    /// <summary>index:10 魔法攻撃力</summary>
    public int matk;
    /// <summary>index:11 物理防御力</summary>
    public int def;
    /// <summary>index:12 魔法防御力</summary>
    public int mdef;
    /// <summary>index:13 命中</summary>
    public int hit;
    /// <summary>index:14 クリティカル</summary>
    public int crt;
    /// <summary>index:15 回避</summary>
    public int eva;
    /// <summary>index:16~ 能力値上昇</summary>
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
        // データ作成
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
/// マスターマテリアルクラス
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
    /// <summary>index:8 マテリアル分類</summary>
    public Group group;
    /// <summary>index:9 物理攻撃力</summary>
    public int atk;
    /// <summary>index:10 魔法攻撃力</summary>
    public int matk;
    /// <summary>index:11 物理防御影響率</summary>
    public int def;
    /// <summary>index:12 魔法防御影響率</summary>
    public int mdef;
    /// <summary>index:13 命中影響率</summary>
    public int hit;
    /// <summary>index:14 クリティカル影響率</summary>
    public int crt;
    /// <summary>index:15 回避影響率</summary>
    public int eva;
    /// <summary>index:16~23 能力値</summary>
    public int[] ability = new int[(int)Ability.MAX];
    /// <summary>index:24~ 属性抵抗</summary>
    public int[] regist = new int[(int)ENElement.MAX];
    /// <summary>index:32 接頭語 アイテム名の前に表示されます</summary>
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
        // データ作成
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
/// アイテム実体の基底クラス
/// </summary>
public class ItemData
{
    /// <summary>アイテム分類</summary>
    public ItemType type;
    /// <summary>アイテム番号</summary>
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
/// 道具アイテム（実体）
/// </summary>
public class Item : ItemData
{
    /// <summary>品質 - スタック可能なアイテムの場合、影響なし</summary>
    public int quality;
    /// <summary>スタック数 - スタック不可の場合、影響なし</summary>
    public int stack;
}