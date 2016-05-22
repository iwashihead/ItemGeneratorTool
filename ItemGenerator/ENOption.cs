using System.Collections;
using System.Collections.Generic;

/// <summary>能力値種類</summary>
public enum Ability {
    /// <summary>ヒットポイント</summary>
    HP,
    /// <summary>テクニカルポイント</summary>
    TP,
    /// <summary>力</summary>
    STR,
    /// <summary>生命</summary>
    VIT,
    /// <summary>知性</summary>
    INT,
    /// <summary>器用</summary>
    DEX,
    /// <summary>敏捷</summary>
    AGI,
    /// <summary>運</summary>
    LUC,

    MAX
}

/// <summary>非戦闘スキル</summary>
public enum ENSkill { Negotiation, BlackSmith, Alchemy, Cooking, Scout, Gather, MAX }

/// <summary>属性種類</summary>
public enum ENElement {
    /// <summary>1番目 斬撃属性</summary>
    Slash,
    /// <summary>2番目 打撃属性</summary>
    Strike,
    /// <summary>3番目 刺突属性</summary>
    Thrust,
    /// <summary>4番目 炎属性</summary>
    Fire,
    /// <summary>5番目 水属性</summary>
    Aqua,
    /// <summary>6番目 雷属性</summary>
    Thunder,
    /// <summary>7番目 闇属性</summary>
    Dark,
    /// <summary>8番目 光属性</summary>
    Light,

    MAX
}

/// <summary>
/// オプション - 追加効果
/// </summary>
public class ENOption
{
    public enum OptionType
    {
        /// <summary>
        /// <para>ステータスボーナス - 上昇or下降</para>
        /// <para>影響度1 : ステータスの種類, enum Ability参照</para>
        /// <para>影響度2 : 上昇or下降する値</para>
        /// </summary>
        STATUS_ADD,
        /// <summary>
        /// <para>ステータスボーナス(割合) - 上昇or下降</para>
        /// <para>影響度1 : ステータスの種類, enum Ability参照</para>
        /// <para>影響度2 : 上昇or下降する割合</para>
        /// <para> ex) 影響度1=3, 影響度2=10 なら 「STRが10％上昇」 となる</para>
        /// </summary>
        STATUS_MULTI,
        /// <summary>
        /// <para>属性耐性ボーナス(割合) - 上昇or下降</para>
        /// <para>影響度1 : 属性の種類, enum ENElement参照</para>
        /// <para>影響度2 : 上昇or下降する割合</para>
        /// <para> ex) 影響度1=5, 影響度2=10 なら 「水耐性が10％上昇」 となる</para>
        /// </summary>
        ELEMENT_MULTI,
        /// <summary>
        /// <para>バッドステータス耐性ボーナス(割合) - 上昇or下降</para>
        /// <para>影響度1 : バッドステータスの種類, enum ENElement参照</para>
        /// <para>影響度2 : 上昇or下降する割合</para>
        /// <para> ex) 影響度1=5, 影響度2=10 なら 「水耐性が10％上昇」 となる</para>
        /// </summary>
        STATE_MULTI,
        /// <summary>スキル習得, 影響度1=スキルID</summary>
        GAIN_SKILL,
        /// <summary>ドロップアイテムボーナス, 影響度1=上昇%(20なら20%アップ)</summary>
        DROP_BONUS,
        /// <summary>お金のドロップボーナス, 影響度1=上昇%(20なら20%アップ)</summary>
        MONEY_BONUS,

        MAX
    }

    /// <summary>オプションの種類</summary>
    public OptionType type;
    /// <summary>影響値１</summary>
    public int value1;
    /// <summary>影響値２</summary>
    public int value2;
}

/// <summary>
/// 列挙型をゲーム内の文字列に変換する拡張メソッド用クラス
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// オプションのゲーム内表示文字列を取得する
    /// 例）HP+100, ダメージ32%上昇 ...等
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
    /// <summary>ゲーム内文字列を取得します</summary>
    public static string DispName(this Ability abl)
    {
        switch (abl)
        {
            case Ability.AGI:
                return "敏捷";
            case Ability.INT:
                return "知力";
            case Ability.HP:
                return "ライフ";
            case Ability.LUC:
                return "運";
            case Ability.TP:
                return "マナ";
            case Ability.STR:
                return "力";
            case Ability.DEX:
                return "器用";
            case Ability.VIT:
                return "生命";
            default:
                return string.Empty;
        }
    }
    /// <summary>ゲーム内文字列を取得します</summary>
    public static string DispName(this MasterItem.Group val)
    {
        switch (val)
        {
            case MasterItem.Group.Book:
                return "本・巻物";
            case MasterItem.Group.Food:
                return "料理";
            case MasterItem.Group.FoodSrc:
                return "食材";
            case MasterItem.Group.Herb:
                return "ハーブ";
            case MasterItem.Group.Key:
                return "鍵";
            case MasterItem.Group.Lacryma:
                return "結晶石";
            case MasterItem.Group.Misc:
                return "その他";
            case MasterItem.Group.Potion:
                return "薬品";
            case MasterItem.Group.Seed:
                return "種";
            case MasterItem.Group.Tool:
                return "道具";
            case MasterItem.Group.Torch:
                return "たいまつ";
            default:
                return string.Empty;
        }
    }
    /// <summary>ゲーム内文字列を取得します</summary>
    public static string DispName(this MasterWeapon.Group val)
    {
        switch (val)
        {
            case MasterWeapon.Group.Axe:
                return "斧";
            case MasterWeapon.Group.Bow:
                return "弓矢";
            case MasterWeapon.Group.Hammer:
                return "鈍器";
            case MasterWeapon.Group.Spear:
                return "槍";
            case MasterWeapon.Group.Staff:
                return "杖";
            case MasterWeapon.Group.Sword:
                return "剣";
            default:
                return string.Empty;
        }
    }
    /// <summary>ゲーム内文字列を取得します</summary>
    public static string DispName(this MasterArmor.Group val)
    {
        switch (val)
        {
            case MasterArmor.Group.Body:
                return "鎧";
            case MasterArmor.Group.Hand:
                return "小手";
            case MasterArmor.Group.Head:
                return "帽子";
            case MasterArmor.Group.Leg:
                return "靴";
            case MasterArmor.Group.Shield:
                return "盾";
            default:
                return string.Empty;
        }
    }
    /// <summary>ゲーム内文字列を取得します</summary>
    public static string DispName(this MasterAcce.Group val)
    {
        switch (val)
        {
            case MasterAcce.Group.Orb:
                return "オーブ";
            case MasterAcce.Group.Pendant:
                return "首飾り";
            case MasterAcce.Group.Ring:
                return "指輪";
            default:
                return string.Empty;
        }
    }
    /// <summary>ゲーム内文字列を取得します</summary>
    public static string DispName(this MasterMaterial.Group val)
    {
        switch (val)
        {
            case MasterMaterial.Group.Bone:
                return "骨・牙";
            case MasterMaterial.Group.Cotton:
                return "綿";
            case MasterMaterial.Group.Leather:
                return "革";
            case MasterMaterial.Group.Metal:
                return "金属";
            case MasterMaterial.Group.Stone:
                return "石材";
            case MasterMaterial.Group.Wood:
                return "木材";
            default:
                return string.Empty;
        }
    }
    /// <summary>ゲーム内文字列を取得します</summary>
    public static string DispName(this ENElement val)
    {
        switch (val)
        {
            case ENElement.Aqua:
                return "水";
            case ENElement.Dark:
                return "闇";
            case ENElement.Fire:
                return "火";
            case ENElement.Light:
                return "光";
            case ENElement.Slash:
                return "切断";
            case ENElement.Strike:
                return "打撃";
            case ENElement.Thrust:
                return "刺突";
            case ENElement.Thunder:
                return "雷";
            default:
                return string.Empty;
        }
    }
    /// <summary>ゲーム内文字列を取得します</summary>
    public static string DispName(this MasterItem.EffectType val)
    {
        switch (val)
        {
            case MasterItem.EffectType.AddOption:
                return "アイテムにオプションをランダムに追加する";
            case MasterItem.EffectType.AddState:
                return "状態異常を付加する";
            case MasterItem.EffectType.CreateMaterial:
                return "マテリアルをランダムに生成する";
            case MasterItem.EffectType.HealL:
                return "ライフを回復する";
            case MasterItem.EffectType.HealLM:
                return "ライフとマナを回復する";
            case MasterItem.EffectType.HealM:
                return "マナを回復する";
            case MasterItem.EffectType.HealState:
                return "状態異常を回復する";
            case MasterItem.EffectType.MixOption:
                return "2つのアイテムをランダム合成する";
            case MasterItem.EffectType.None:
                return "なし";
            case MasterItem.EffectType.Regeneration:
                return "ライフの自動回復";
            case MasterItem.EffectType.Revive:
                return "戦闘不能からの回復";
            case MasterItem.EffectType.SubOption:
                return "アイテムのオプションを消す";
            case MasterItem.EffectType.Torch:
                return "周囲を照らす";
            case MasterItem.EffectType.Skill:
                return "戦闘中に指定スキルを実行する";
            default:
                return string.Empty;
        }
    }
    /// <summary>val1とval2の設定説明を表示する</summary>
    public static string DispDescription(this MasterItem.EffectType val)
    {
        switch (val)
        {
            case MasterItem.EffectType.AddOption:
                return "オプションランダム追加" + 
                    "\nval1=最低オプションランク \nval2=最高オプションランク";
            case MasterItem.EffectType.AddState:
                return "状態異常を付加する" +
                    "\nval1=ステートID\nval2=付加ターン数";
            case MasterItem.EffectType.CreateMaterial:
                return "マテリアルをランダムに生成する" +
                    "\nval1=最低マテリアルランク\nval2=最高マテリアルランク";
            case MasterItem.EffectType.HealL:
                return "ライフを回復する" +
                    "\nval1=回復量\nval2=1:範囲（ 味方全体, それ以外:単体 ）";
            case MasterItem.EffectType.HealLM:
                return "ライフとマナを回復する" +
                    "\nval1=回復量\nval2=1:範囲（ 味方全体, それ以外:単体 ）";
            case MasterItem.EffectType.HealM:
                return "マナを回復する" +
                    "\nval1=回復量\nval2=1:範囲（ 味方全体, それ以外:単体 ）";
            case MasterItem.EffectType.HealState:
                return "状態異常を回復する" +
                    "\nval1=ステートID\nval2=";
            case MasterItem.EffectType.MixOption:
                return "2つのアイテムをランダム合成する" +
                    "\nval1=\nval2=";
            case MasterItem.EffectType.None:
                return "なし";
            case MasterItem.EffectType.Regeneration:
                return "ライフの自動回復" +
                    "\nval1=回復量(総量)\nval2=持続ターン数";
            case MasterItem.EffectType.Revive:
                return "戦闘不能からの回復" +
                    "\nval1=復活時のHP回復率（％）\nval2=成功率（％）";
            case MasterItem.EffectType.SubOption:
                return "アイテムのオプションを消す" +
                    "\nval1=成功率（％）\nval2=";
            case MasterItem.EffectType.Torch:
                return "周囲を照らす" +
                    "\nval1=明るさ（Max100）\nval2=持続ターン数";
            case MasterItem.EffectType.Skill:
                return "戦闘中に指定スキルを実行" +
                    "\nval1=スキルID\nval2=1:移動時の使用を制限, それ以外:対応スキルの仕様に準じる";
            default:
                return string.Empty;
        }
    }
    /// <summary>ゲーム内文字列を取得します</summary>
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