using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

/// <summary>
/// ランダム生成される文字列に関するデータを集めたクラス
/// 種別ごとにGetRandomメソッドを呼び出すことでランダムに名前を取得できる
/// </summary>
public class ENName
{
    public bool isLoad = false;

    /// <summary>男性の名前</summary>
    public List<string> MaleName;
    /// <summary>女性の名前</summary>
    public List<string> FemaleName;
    /// <summary>モンスターの名前</summary>
    public List<string> MonsterName;
    /// <summary>特殊アイテムの名前</summary>
    public List<string> ArtifactName;

    /// <summary>
    /// リスト内の文字列をランダムに取得する
    /// </summary>
    /// <returns></returns>
    public string GetRandom(List<string> list)
    {
        if (list == null || list.Count <= 0)
        {
            return "";
        }
        Random rand = new Random();
        int r = rand.Next(0, list.Count);
        return list[r];
    }

    /// <summary>
    /// ロード処理
    /// 正常で0、失敗で-1を返します
    /// </summary>
    /// <param name="file">読み込むテキストアセット</param>
    /// <param name="list">格納するリスト</param>
    /// <returns></returns>
    public int Load(string file, ref List<string> list)
    {
        // ロード処理
        if (file == null)
        {
            return -1;
        }

        string str = file;
        List<string> a = new List<string>();

        // データファイルをパースする
        Regex regexString = new Regex(
             " *(\"(?<name>[^\"]+)\"|(?<name>[^\r^\n]+)) *[\r\n]"
             , RegexOptions.ExplicitCapture);
        Match match = regexString.Match(str, 0);

        while (match.Success)
        {
            // 要素を追加していく
            a.Add(match.Groups["name"].Value);
            match = regexString.Match(str, match.Index + match.Length);
        }

        list = a;

        return 0;
    }
}