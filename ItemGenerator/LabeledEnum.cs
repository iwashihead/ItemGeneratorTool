/*
 * 列挙型にラベルを付加するカスタム属性クラス
 * .Net3.0以降であれば、拡張メソッドを使用した実装の方が高速でかつ
 * 利用が容易(コード量は多くなる)であるため、拡張メソッドでの実装を推奨
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 列挙型のフィールドにラベル文字列を付加するカスタム属性
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class LabeledEnumAttribute : Attribute
{
    /// <summary>
    /// ラベル文字列
    /// </summary>
    private string label;

    /// <summary>
    /// LabeledEnumAttribute クラスの新しいインスタンスを初期化
    /// </summary>
    /// <param name="label">ラベル文字列</param>
    public LabeledEnumAttribute(string label)
    {
        this.label = label;
    }

    /// <summary>
    /// 属性で指定されたラベル文字列を取得する
    /// </summary>
    /// <param name="value">ラベル付きフィールド</param>
    /// <returns>ラベル文字列</returns>
    public static string GetLabel(Enum value)
    {
        //EnumのTypeを取得する
        //  type = {Name = "フォーマット形式共通" FullName = "共通.フォーマット形式共通"}
        var type = value.GetType();
        //
        //Enumのフィールドを取得する
        //  name = "Default"
        var name = Enum.GetName(type, value);
        //
        //クラスの配列を取得する
        //  myClass = {共通.LabeledEnumAttribute[1]}
        var myClass =
                    (LabeledEnumAttribute[])type.GetField(name)
                    .GetCustomAttributes(typeof(LabeledEnumAttribute), false);

        return myClass[0].label;
    }
}
