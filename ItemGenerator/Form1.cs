using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ItemGenerator
{
    public partial class ItemGenerator : Form
    {
        // コピー操作用
        private List<ItemBase> copyItem;

        // コンテナ
        private List<MasterItem> listItem;
        private List<MasterWeapon> listWeapon;
        private List<MasterArmor> listArmor;
        private List<MasterAcce> listAcce;
        private List<MasterMaterial> listMaterial;

        // アイコン画像
        private Bitmap bmpIcon;
        private const char split = ':';

        public ItemGenerator()
        {
            InitializeComponent();

            // .iniファイルを読み込む
            textBoxPassWord.Text = Properties.Settings.Default.PASSWORD;
            numIconSizeW.Value = Properties.Settings.Default.ICON_WIDTH;
            numIconSizeH.Value = Properties.Settings.Default.ICON_HEIGHT;
            checkBoxEncryption.Checked = Properties.Settings.Default.ENCRYPTION_ENABLE;

            this.MaximizeBox = false;
            tabControl1.Selected += tabControl1_Selected;
            copyItem = new List<ItemBase>();
            listItem = new List<MasterItem>();
            listWeapon = new List<MasterWeapon>();
            listArmor = new List<MasterArmor>();
            listAcce = new List<MasterAcce>();
            listMaterial = new List<MasterMaterial>();
            listBoxItem.Items.Clear();
        }

        // ロード処理
        private void Form1_Load(object sender, EventArgs e)
        {
            // アイコン初期化
            bmpIcon = new Bitmap(Properties.Settings.Default.PATH_ICON_TEXTURE);
            if (bmpIcon == null){
                MessageBox.Show("アイコン画像の読み込みに失敗しました！\n設定タブからアイコン画像を設定してください");
                bmpIcon = new Bitmap(128, 128);
            }
            pictureBoxIconTexture.Image = bmpIcon;
            setIcon(pictureBoxMaterialIcon, (int)numMaterialIcon.Value);
            setIcon(pictureBoxItemIcon, (int)numItemIcon.Value);
            setIcon(pictureBoxWeaponIcon, (int)numWeaponIcon.Value);
            setIcon(pictureBoxArmorIcon, (int)numArmorIcon.Value);
            setIcon(pictureBoxAcceIcon, (int)numAcceIcon.Value);

            // コンボボックス初期化
            int i;
            for (i = 0; i < (int)MasterItem.Group.MAX; i++)
                comboBoxItemType.Items.Add(((MasterItem.Group)i).DispName());
            for (i = 0; i < (int)MasterItem.EffectType.MAX; i++)
                comboBoxItemEffecr.Items.Add(((MasterItem.EffectType)i).DispName());
            for (i = 0; i < (int)MasterWeapon.Group.MAX; i++)
                comboBoxWeaponType.Items.Add(((MasterWeapon.Group)i).DispName());
            for (i = 0; i < (int)ENElement.MAX; i++)
                comboBoxWeaponElement.Items.Add(((ENElement)i).DispName());
            for (i = 0; i < (int)MasterArmor.Group.MAX; i++)
                comboBoxArmorType.Items.Add(((MasterArmor.Group)i).DispName());
            for (i = 0; i < (int)ENOption.OptionType.MAX; i++)
                comboBoxWeaponOptionType.Items.Add(((ENOption.OptionType)i).DispName());
            for (i = 0; i < (int)MasterAcce.Group.MAX; i++)
                comboBoxAcceType.Items.Add(((MasterAcce.Group)i).DispName());
            for (i = 0; i < (int)MasterMaterial.Group.MAX; i++)
                comboBoxMaterialType.Items.Add(((MasterMaterial.Group)i).DispName());

            // コンボボックスの0インデックスを選択状態に設定
            comboBoxItemType.SelectedIndex = 0;
            comboBoxItemEffecr.SelectedIndex = 0;
            comboBoxWeaponElement.SelectedIndex = 0;
            comboBoxWeaponType.SelectedIndex = 0;
            comboBoxWeaponOptionType.SelectedIndex = 0;
            comboBoxAcceType.SelectedIndex = 0;
            comboBoxArmorType.SelectedIndex = 0;
            comboBoxMaterialType.SelectedIndex = 0;
        }

        #region ボタン操作
        // タブ選択時の処理
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            // アイコン表示を更新
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    setIcon(pictureBoxItemIcon, (int)numItemIcon.Value);
                    break;
                case 1:
                    setIcon(pictureBoxWeaponIcon, (int)numWeaponIcon.Value);
                    break;
                case 2:
                    setIcon(pictureBoxArmorIcon, (int)numArmorIcon.Value);
                    break;
                case 3:
                    setIcon(pictureBoxAcceIcon, (int)numAcceIcon.Value);
                    break;
                case 4:
                    setIcon(pictureBoxMaterialIcon, (int)numMaterialIcon.Value);
                    break;
                case 5:
                    break;
                default:
                    break;
            }
        }
        // 追加ボタン
        private void buttonCreate_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex < 0)
            {
                MessageBox.Show("タブが範囲外です！");
                return;
            }

            switch (tabControl1.SelectedIndex)
            {
                case 0:// 道具
                    #region
                    if (string.IsNullOrEmpty(textBoxItemName.Text))
                    {
                        MessageBox.Show("アイテム名が未設定です！");
                        return;
                    }

                    MasterItem mi;
                    mi = listItem.Find(delegate(MasterItem masteritem) { return (masteritem.id == (int)numItemID.Value); });
                    if (mi == null)
                        mi = new MasterItem();
                    else{
                        DialogResult result = MessageBox.Show("既に登録済みのIDです。\n上書きしますか？",
                            "上書きの確認",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button2);

                        if (result == System.Windows.Forms.DialogResult.No)
                        {
                            return;
                        }
                        listItem.Remove(mi);
                        listBoxItem.Items.Remove(mi);
//                        listBoxItem.Items.Remove(string.Format("{0:D4}" + split + mi.name, mi.id));
                    }

                    mi = CreateItem();
                    if (mi == null)
                    {
                        return;
                    }

                    // 追加
                    listItem.Add(mi);
                    listBoxItem.Items.Add(mi);
//                    listBoxItem.Items.Add(string.Format("{0:D4}" + split + mi.name, mi.id));
                    numItemID.Value++;
                    break;
                    #endregion

                case 1:// 武器
                    #region
                    if (string.IsNullOrEmpty(textBoxWeaponName.Text))
                    {
                        MessageBox.Show("アイテム名が未設定です！");
                        return;
                    }

                    MasterWeapon mw;
                    mw = listWeapon.Find(delegate(MasterWeapon masterWeapon) { return (masterWeapon.id == (int)numWeaponID.Value); });
                    if (mw == null)
                        mw = new MasterWeapon();
                    else{
                        DialogResult result = MessageBox.Show("既に登録済みのIDです。\n上書きしますか？",
                            "上書きの確認",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button2);

                        if (result == System.Windows.Forms.DialogResult.No)
                        {
                            return;
                        }
                        listWeapon.Remove(mw);
                        listBoxWeapon.Items.Remove(mw);
//                        listBoxWeapon.Items.Remove(string.Format("{0:D4}" + split + mw.name, mw.id));
                    }

                    mw = CreateWeapon();
                    if (mw == null)
                    {
                        return;
                    }

                    // 追加
                    listWeapon.Add(mw);
                    listBoxWeapon.Items.Add(mw);
//                    listBoxWeapon.Items.Add(string.Format("{0:D4}" + split + mw.name, mw.id));
                    numWeaponID.Value++;
                    break;
                    #endregion

                case 2:// 防具
                    #region
                    if (string.IsNullOrEmpty(textBoxArmorName.Text))
                    {
                        MessageBox.Show("アイテム名が未設定です！");
                        return;
                    }

                    MasterArmor ma;
                    ma = listArmor.Find(delegate(MasterArmor masterArmor) { return (masterArmor.id == (int)numArmorID.Value); });
                    if (ma == null)
                        ma = new MasterArmor();
                    else{
                        DialogResult result = MessageBox.Show("既に登録済みのIDです。\n上書きしますか？",
                            "上書きの確認",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button2);

                        if (result == System.Windows.Forms.DialogResult.No)
                        {
                            return;
                        }
                        listArmor.Remove(ma);
                        listBoxArmor.Items.Remove(ma);
//                        listBoxArmor.Items.Remove(string.Format("{0:D4}" + split + ma.name, ma.id));
                    }

                    ma = CreateArmor();
                    if (ma == null)
                    {
                        return;
                    }

                    // 追加
                    listArmor.Add(ma);
                    listBoxArmor.Items.Add(ma);
//                    listBoxArmor.Items.Add(string.Format("{0:D4}" + split + ma.name, ma.id));
                    numArmorID.Value++;
                    break;
                    #endregion

                case 3:// アクセサリ
                    #region
                    if (string.IsNullOrEmpty(textBoxAcceName.Text))
                    {
                        MessageBox.Show("アイテム名が未設定です！");
                        return;
                    }

                    MasterAcce mc;
                    mc = listAcce.Find(delegate(MasterAcce masterAcce) { return (masterAcce.id == (int)numAcceID.Value); });
                    if (mc == null)
                        mc = new MasterAcce();
                    else{
                        DialogResult result = MessageBox.Show("既に登録済みのIDです。\n上書きしますか？",
                            "上書きの確認",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button2);

                        if (result == System.Windows.Forms.DialogResult.No)
                        {
                            return;
                        }
                        listAcce.Remove(mc);
                        listBoxAcce.Items.Remove(mc);
//                        listBoxAcce.Items.Remove(string.Format("{0:D4}" + split + mc.name, mc.id));
                    }

                    mc = CreateAcce();
                    if (mc == null)
                    {
                        return;
                    }

                    // 追加
                    listAcce.Add(mc);
                    listBoxAcce.Items.Add(mc);
//                    listBoxAcce.Items.Add(string.Format("{0:D4}" + split + mc.name, mc.id));
                    numAcceID.Value++;
                    break;
                    #endregion

                case 4:// マテリアル
                    #region
                    if (string.IsNullOrEmpty(textBoxMaterialName.Text))
                    {
                        MessageBox.Show("アイテム名が未設定です！");
                        return;
                    }

                    MasterMaterial mm;
                    mm = listMaterial.Find(delegate(MasterMaterial masterMaterial) { return (masterMaterial.id == (int)numMaterialID.Value); });
                    if (mm == null)
                        mm = new MasterMaterial();
                    else{
                        DialogResult result = MessageBox.Show("既に登録済みのIDです。\n上書きしますか？",
                            "上書きの確認",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button2);

                        if (result == System.Windows.Forms.DialogResult.No)
                        {
                            return;
                        }
                        listMaterial.Remove(mm);
                        listBoxMaterial.Items.Remove(mm);
//                        listBoxMaterial.Items.Remove(string.Format("{0:D4}" + split + mm.name, mm.id));
                    }

                    mm = CreateMaterial();
                    if (mm == null)
                    {
                        return;
                    }

                    // 追加
                    listMaterial.Add(mm);
                    listBoxMaterial.Items.Add(mm);
//                    listBoxMaterial.Items.Add(string.Format("{0:D4}" + split + mm.name, mm.id));
                    numMaterialID.Value++;
                    break;
                    #endregion

                case 5:// 設定
                    break;
                default:
                    MessageBox.Show("タブが範囲外です！");
                    break;
            }
        }
        // 読み込みボタン
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            SetListBoxItem();
        }
        // 削除ボタン
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeleteListBoxItems();
        }
        // 保存ボタン
        private void buttonSave_Click(object sender, EventArgs e)
        {
            int index = tabControl1.SelectedIndex;
            switch (index)
            {
                case 0:// 道具
                    int result = SaveItem();
                    if (result != 0)
                    {
                        MessageBox.Show("保存に失敗しました！");
                    }
                    break;
                case 1:// 武器
                    result = SaveWeapon();
                    if (result != 0)
                    {
                        MessageBox.Show("保存に失敗しました！");
                    }
                    break;
                case 2:// 防具
                    result = SaveArmor();
                    if (result != 0)
                    {
                        MessageBox.Show("保存に失敗しました！");
                    }
                    break;
                case 3:// アクセサリ
                    result = SaveAcce();
                    if (result != 0)
                    {
                        MessageBox.Show("保存に失敗しました！");
                    }
                    break;
                case 4:// マテリアル
                    result = SaveMaterial();
                    if (result != 0)
                    {
                        MessageBox.Show("保存に失敗しました！");
                    }
                    break;
                case 5:// 設定
                    break;
                default:
                    break;
            }
        }
        // すべて保存ボタン
        private void buttonSaveAll_Click(object sender, EventArgs e)
        {
            int result = SaveAll();
            if (result != 0)
            {
                MessageBox.Show("保存に失敗しました！");
            }
        }
        // 開くボタン
        private void buttonLoadFile_Click(object sender, EventArgs e)
        {
            //if (listItem.Count > 0 || listWeapon.Count > 0 || listArmor.Count > 0 || listAcce.Count > 0 || listMaterial.Count > 0)
            //{
            //    DialogResult result = MessageBox.Show("編集中のデータは破棄されますがよろしいですか？",
            //        "確認",
            //        MessageBoxButtons.YesNo,
            //        MessageBoxIcon.Exclamation,
            //        MessageBoxDefaultButton.Button2);

            //    if (result == System.Windows.Forms.DialogResult.No)
            //    {
            //        return;
            //    }
            //}

            // ファイル読み込み処理
            int res = LoadFromFile();
            if (res != 0)
            {
                MessageBox.Show("読み込みに失敗しました！");
            }
        }
        #endregion

        #region セーブ
        // セーブデータ文字列の作成
        private string CreateItemData()
        {
            // 中身をソートする
            listItem.Sort();
            string dat = "\n" + Properties.Settings.Default.SOF_ITEM + "\n";
            foreach (MasterItem mi in listItem)
            {
                dat += mi.Serialize() + "\n";
            }
            dat += Properties.Settings.Default.EOF_ITEM;
            return dat;
        }
        private string CreateWeaponData()
        {
            // 中身をソートする
            listWeapon.Sort();
            string dat = "\n" + Properties.Settings.Default.SOF_WEAPON + "\n";
            foreach (MasterWeapon mi in listWeapon)
            {
                dat += mi.Serialize() + "\n";
            }
            dat += Properties.Settings.Default.EOF_WEAPON;
            return dat;
        }
        private string CreateArmorData()
        {
            // 中身をソートする
            listArmor.Sort();
            string dat = "\n" + Properties.Settings.Default.SOF_ARMOR + "\n";
            foreach (MasterArmor mi in listArmor)
            {
                dat += mi.Serialize() + "\n";
            }
            dat += Properties.Settings.Default.EOF_ARMOR;
            return dat;
        }
        private string CreateAcceData()
        {
            // 中身をソートする
            listAcce.Sort();
            string dat = "\n" + Properties.Settings.Default.SOF_ACCE + "\n";
            foreach (MasterAcce mi in listAcce)
            {
                dat += mi.Serialize() + "\n";
            }
            dat += Properties.Settings.Default.EOF_ACCE;
            return dat;
        }
        private string CreateMaterialData()
        {
            // 中身をソートする
            listMaterial.Sort();
            string dat = "\n" + Properties.Settings.Default.SOF_MATERIAL + "\n";
            foreach (MasterMaterial mi in listMaterial)
            {
                dat += mi.Serialize() + "\n";
            }
            dat += Properties.Settings.Default.EOF_MATERIAL;
            return dat;
        }

        // セーブ処理
        private int SaveItem()
        {
            try
            {
                // セーブダイアログを開く
                saveFileDialog1.FileName = "MasterItem.txt";

                //[ファイルの種類]に表示される選択肢を指定する
                saveFileDialog1.Filter =
                    "テキスト形式(*.txt)|*.txt|ENファイル(*.en)|*.en|すべてのファイル(*.*)|*.*";

                // データフォルダの絶対パスを取得する
                string stFilePath = System.IO.Path.GetFullPath("Data");
                saveFileDialog1.InitialDirectory = stFilePath;

                //ダイアログを表示する
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // 拡張子を取得する
                    string extension =
                        System.IO.Path.GetExtension(saveFileDialog1.FileName).ToUpper();

                    string dat = CreateItemData();

                    // 拡張子がenなら暗号化する
                    if (extension == ".en" || extension == ".EN")
                    {
                        dat = dat.EncryptString(Properties.Settings.Default.PASSWORD);
                        // ファイルを書き込む
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.OpenFile()))
                        {
                            sw.Write(dat);
                        }
                    }
                    else
                    {
                        // ファイルを書き込む
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.OpenFile()))
                        {
                            string[] datArray = dat.Split(new char[] { '\n', '\r' });
                            foreach (string s in datArray)
                            {
                                sw.WriteLine(s);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }

            return 0;
        }
        private int SaveWeapon()
        {
            try
            {
                // セーブダイアログを開く
                saveFileDialog1.FileName = "MasterWeapon.txt";

                //[ファイルの種類]に表示される選択肢を指定する
                saveFileDialog1.Filter =
                    "テキスト形式(*.txt)|*.txt|ENファイル(*.en)|*.en|すべてのファイル(*.*)|*.*";

                // データフォルダの絶対パスを取得する
                string stFilePath = System.IO.Path.GetFullPath("Data");
                saveFileDialog1.InitialDirectory = stFilePath;

                //ダイアログを表示する
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // 拡張子を取得する
                    string extension =
                        System.IO.Path.GetExtension(saveFileDialog1.FileName).ToUpper();

                    string dat = CreateWeaponData();

                    // 拡張子がenなら暗号化する
                    if (extension == ".en" || extension == ".EN")
                    {
                        dat = dat.EncryptString(Properties.Settings.Default.PASSWORD);
                        // ファイルを書き込む
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.OpenFile()))
                        {
                            sw.Write(dat);
                        }
                    }
                    else
                    {
                        // ファイルを書き込む
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.OpenFile()))
                        {
                            string[] datArray = dat.Split(new char[]{ '\n', '\r' });
                            foreach (string s in datArray)
                            {
                                sw.WriteLine(s);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
            return 0;
        }
        private int SaveArmor()
        {
            try
            {
                // セーブダイアログを開く
                saveFileDialog1.FileName = "MasterArmor.txt";

                //[ファイルの種類]に表示される選択肢を指定する
                saveFileDialog1.Filter =
                    "テキスト形式(*.txt)|*.txt|ENファイル(*.en)|*.en|すべてのファイル(*.*)|*.*";

                // データフォルダの絶対パスを取得する
                string stFilePath = System.IO.Path.GetFullPath("Data");
                saveFileDialog1.InitialDirectory = stFilePath;

                //ダイアログを表示する
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // 拡張子を取得する
                    string extension =
                        System.IO.Path.GetExtension(saveFileDialog1.FileName).ToUpper();

                    string dat = CreateArmorData();

                    // 拡張子がenなら暗号化する
                    if (extension == ".en" || extension == ".EN")
                    {
                        dat = dat.EncryptString(Properties.Settings.Default.PASSWORD);
                        // ファイルを書き込む
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.OpenFile()))
                        {
                            sw.Write(dat);
                        }
                    }
                    else
                    {
                        // ファイルを書き込む
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.OpenFile()))
                        {
                            string[] datArray = dat.Split(new char[] { '\n', '\r' });
                            foreach (string s in datArray)
                            {
                                sw.WriteLine(s);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
            return 0;
        }
        private int SaveAcce()
        {
            try
            {
                // セーブダイアログを開く
                saveFileDialog1.FileName = "MasterAcce.txt";

                //[ファイルの種類]に表示される選択肢を指定する
                saveFileDialog1.Filter =
                    "テキスト形式(*.txt)|*.txt|ENファイル(*.en)|*.en|すべてのファイル(*.*)|*.*";

                // データフォルダの絶対パスを取得する
                string stFilePath = System.IO.Path.GetFullPath("Data");
                saveFileDialog1.InitialDirectory = stFilePath;

                //ダイアログを表示する
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // 拡張子を取得する
                    string extension =
                        System.IO.Path.GetExtension(saveFileDialog1.FileName).ToUpper();

                    string dat = CreateAcceData();

                    // 拡張子がenなら暗号化する
                    if (extension == ".en" || extension == ".EN")
                    {
                        dat = dat.EncryptString(Properties.Settings.Default.PASSWORD);
                        // ファイルを書き込む
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.OpenFile()))
                        {
                            sw.Write(dat);
                        }
                    }
                    else
                    {
                        // ファイルを書き込む
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.OpenFile()))
                        {
                            string[] datArray = dat.Split(new char[] { '\n', '\r' });
                            foreach (string s in datArray)
                            {
                                sw.WriteLine(s);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
            return 0;
        }
        private int SaveMaterial()
        {
            try
            {
                // セーブダイアログを開く
                saveFileDialog1.FileName = "MasterMaterial.txt";

                //[ファイルの種類]に表示される選択肢を指定する
                saveFileDialog1.Filter =
                    "テキスト形式(*.txt)|*.txt|ENファイル(*.en)|*.en|すべてのファイル(*.*)|*.*";

                // データフォルダの絶対パスを取得する
                string stFilePath = System.IO.Path.GetFullPath("Data");
                saveFileDialog1.InitialDirectory = stFilePath;

                //ダイアログを表示する
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // 拡張子を取得する
                    string extension =
                        System.IO.Path.GetExtension(saveFileDialog1.FileName).ToUpper();

                    string dat = CreateMaterialData();

                    // 拡張子がenなら暗号化する
                    if (extension == ".en" || extension == ".EN")
                    {
                        dat = dat.EncryptString(Properties.Settings.Default.PASSWORD);

                        // ファイルを書き込む
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.OpenFile()))
                        {
                            sw.Write(dat);
                        }
                    }
                    else
                    {
                        // ファイルを書き込む
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.OpenFile()))
                        {
                            string[] datArray = dat.Split(new char[] { '\n', '\r' });
                            foreach (string s in datArray)
                            {
                                sw.WriteLine(s);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
            return 0;
        }
        private int SaveAll()
        {
            try
            {
                // セーブダイアログを開く
//                saveFileDialog1.FileName = "MasterData.txt";

                //[ファイルの種類]に表示される選択肢を指定する
                saveFileDialog1.Filter =
                    "テキスト形式(*.txt)|*.txt|ENファイル(*.en)|*.en|すべてのファイル(*.*)|*.*";

                // データフォルダの絶対パスを取得する
                string stFilePath = System.IO.Path.GetFullPath("Data");
                saveFileDialog1.InitialDirectory = stFilePath;

                //ダイアログを表示する
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // 拡張子を取得する
                    string extension =
                        System.IO.Path.GetExtension(saveFileDialog1.FileName).ToUpper();

                    string dat;
                    dat = "" + CreateItemData() +
                        CreateWeaponData() +
                        CreateArmorData() +
                        CreateAcceData() +
                        CreateMaterialData();

                    // 拡張子がenなら暗号化する
                    if (extension == ".en" || extension == ".EN")
                    {

                        dat = dat.EncryptString(Properties.Settings.Default.PASSWORD);

                        // ファイルを書き込む
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.OpenFile()))
                        {
                            sw.Write(dat);
                        }
                    }
                    else
                    {
                        // ファイルを書き込む
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.OpenFile()))
                        {
                            string[] datArray = dat.Split(new char[] { '\n', '\r' });
                            foreach (string s in datArray)
                            {
                                sw.WriteLine(s);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
            return 0;
        }
        #endregion

        #region ロード
        private int LoadFromFile()
        {
            // ディレクトリを設定
            openFileDialog1.InitialDirectory = Application.StartupPath + "\\Data";
            
            //ダイアログを表示
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    // 拡張子を取得する
                    string extension =
                        System.IO.Path.GetExtension(openFileDialog1.FileName).ToUpper();

                    // ダイアログで指定されたファイルを読み込み
                    System.IO.StreamReader sr = new System.IO.StreamReader(
                         openFileDialog1.FileName);

                    // ファイル上書き用にファイル名を保存する
                    saveFileDialog1.FileName = openFileDialog1.SafeFileName;

                    // 3/8 とりあえずファイル読み込みは出来た 3/9はここから
                    string dat = sr.ReadToEnd();
                    Console.WriteLine(dat);

                    // 暗号解読
                    if (extension == ".en" || extension == ".EN")
                    {
                        dat = dat.DecryptString(Properties.Settings.Default.PASSWORD, Encoding.UTF8);
                    }

                    // ワーク
                    List<MasterItem> aItem = new List<MasterItem>();
                    List<MasterWeapon> aWeapon = new List<MasterWeapon>();
                    List<MasterArmor> aArmor = new List<MasterArmor>();
                    List<MasterAcce> aAcce = new List<MasterAcce>();
                    List<MasterMaterial> aMaterial = new List<MasterMaterial>();

                    Console.WriteLine("読み込み開始");

                    #region 一行ずつ読み込む
                    ItemType loadType = ItemType.MAX;
                    string[] datArray = dat.Split(new char[] { '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in datArray)
                    {
                        string ss;
                        ss = s.Replace("\n", "");
                        ss = ss.Replace("\r", "");
                        Console.WriteLine(loadType.ToString(), ss);
                        if (ss == Properties.Settings.Default.SOF_ITEM)
                        {
                            loadType = ItemType.Item;
                        }
                        else if (ss == Properties.Settings.Default.SOF_WEAPON)
                        {
                            loadType = ItemType.Weapon;
                        }
                        else if (ss == Properties.Settings.Default.SOF_ARMOR)
                        {
                            loadType = ItemType.Armor;
                        }
                        else if (ss == Properties.Settings.Default.SOF_ACCE)
                        {
                            loadType = ItemType.Acce;
                        }
                        else if (ss == Properties.Settings.Default.SOF_MATERIAL)
                        {
                            loadType = ItemType.Material;
                        }
                        else if (ss == Properties.Settings.Default.EOF_ITEM)
                        {
                            loadType = ItemType.MAX;
                        }
                        else if (ss == Properties.Settings.Default.EOF_WEAPON)
                        {
                            loadType = ItemType.MAX;
                        }
                        else if (ss == Properties.Settings.Default.EOF_ARMOR)
                        {
                            loadType = ItemType.MAX;
                        }
                        else if (ss == Properties.Settings.Default.EOF_ACCE)
                        {
                            loadType = ItemType.MAX;
                        }
                        else if (ss == Properties.Settings.Default.EOF_MATERIAL)
                        {
                            loadType = ItemType.MAX;
                        }
                        else
                        {
                            switch (loadType)
                            {
                                case ItemType.Item:
                                    MasterItem mi = new MasterItem();
                                    if (mi.Deserialize(s) == 0)
                                        aItem.Add(mi);
                                    break;
                                case ItemType.Weapon:
                                    MasterWeapon mw = new MasterWeapon();
                                    if (mw.Deserialize(s) == 0)
                                        aWeapon.Add(mw);
                                    break;
                                case ItemType.Armor:
                                    MasterArmor ma = new MasterArmor();
                                    if (ma.Deserialize(s) == 0)
                                        aArmor.Add(ma);
                                    break;
                                case ItemType.Acce:
                                    MasterAcce mc = new MasterAcce();
                                    if (mc.Deserialize(s) == 0)
                                        aAcce.Add(mc);
                                    break;
                                case ItemType.Material:
                                    MasterMaterial mm = new MasterMaterial();
                                    if (mm.Deserialize(s) == 0)
                                        aMaterial.Add(mm);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    // それぞれのファイルを上書きしていく
                    if (aItem.Count > 0)
                    {
                        listItem = aItem;
                        listBoxItem.Items.Clear();
                        foreach (MasterItem mi in aItem)
                        {
                            listBoxItem.Items.Add(mi);
                        }
                    }
                    if (aWeapon.Count > 0)
                    {
                        listWeapon = aWeapon;
                        listBoxWeapon.Items.Clear();
                        foreach (MasterWeapon mi in aWeapon)
                        {
                            listBoxWeapon.Items.Add(mi);
                        }
                    }
                    if (aArmor.Count > 0)
                    {
                        listArmor = aArmor;
                        listBoxArmor.Items.Clear();
                        foreach (MasterArmor mi in aArmor)
                        {
                            listBoxArmor.Items.Add(mi);
                        }
                    }
                    if (aAcce.Count > 0)
                    {
                        listAcce = aAcce;
                        listBoxAcce.Items.Clear();
                        foreach (MasterAcce mi in aAcce)
                        {
                            listBoxAcce.Items.Add(mi);
                        }
                    }
                    if (aMaterial.Count > 0)
                    {
                        listMaterial = aMaterial;
                        listBoxMaterial.Items.Clear();
                        foreach (MasterMaterial mi in aMaterial)
                        {
                            listBoxMaterial.Items.Add(mi);
                        }
                    }
                    #endregion

                    sr.Close();
                }
                catch (Exception err)
                {
                    Console.WriteLine(err);
                    return -1;
                }
            }

            return 0;
        }
        #endregion

        #region アイコン関連
        /// <summary>
        /// 指定したPictureBoxにアイコンを表示する
        /// </summary>
        /// <param name="pic">描画するPicTureBox</param>
        /// <param name="num">アイコンID</param>
        void setIcon(PictureBox pic, int num)
        {
            int maxw, maxh, w, h;
            w = (int)numIconSizeW.Value;
            h = (int)numIconSizeH.Value;
            maxw = bmpIcon.Size.Width / w;
            maxh = bmpIcon.Size.Height / h;

            //描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(w, h);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);
            //画像ファイルのImageオブジェクトを作成する
            Bitmap img = bmpIcon;

            //切り取る部分の範囲を決定する。
            Rectangle srcRect = new Rectangle((num % maxw) * w, (num / maxw) * h, w, h);
            //描画する部分の範囲を決定する。
            Rectangle desRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
            //画像の一部を描画する
            g.DrawImage(img, desRect, srcRect, GraphicsUnit.Pixel);
            // 画像を設定
            pic.Image = canvas;

            //Graphicsオブジェクトのリソースを解放する
            g.Dispose();
        }
        private void numMaterialIcon_ValueChanged(object sender, EventArgs e)
        {
            setIcon(pictureBoxMaterialIcon, (int)numMaterialIcon.Value);
        }
        private void numItemIcon_ValueChanged(object sender, EventArgs e)
        {
            setIcon(pictureBoxItemIcon, (int)numItemIcon.Value);
        }
        private void numWeaponIcon_ValueChanged(object sender, EventArgs e)
        {
            setIcon(pictureBoxWeaponIcon, (int)numWeaponIcon.Value);
        }
        private void numArmorIcon_ValueChanged(object sender, EventArgs e)
        {
            setIcon(pictureBoxArmorIcon, (int)numArmorIcon.Value);
        }
        private void numAcceIcon_ValueChanged(object sender, EventArgs e)
        {
            setIcon(pictureBoxAcceIcon, (int)numAcceIcon.Value);
        }
        #endregion

        #region 設定タブの操作

        // アイコン画像設定ボタン
        private void button1_Click(object sender, EventArgs e)
        {
            //ダイアログを表示
            DialogResult result = openFileDialogIconTexture.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = Properties.Settings.Default.PATH_ICON_TEXTURE;

                try
                {
                    // ダイアログで指定されたファイルを読み込み
                    System.IO.Stream stream = openFileDialogIconTexture.OpenFile();
                    bmpIcon = new Bitmap(stream);
                    pictureBoxIconTexture.Image = bmpIcon;

                    // ローカルDataフォルダにコピー
                    path = "Data\\" + openFileDialogIconTexture.SafeFileName;
                    Console.WriteLine(path);
                    bmpIcon.Save(path);
                }
                catch (Exception err)
                {
                    Console.WriteLine(err);
                    MessageBox.Show("設定に失敗しました！");
                    return;
                }

                // ファイルパス設定を保存
                Properties.Settings.Default.PATH_ICON_TEXTURE = path;
                Properties.Settings.Default.Save();

                // アイコン表示を更新
                setIcon(pictureBoxMaterialIcon, (int)numMaterialIcon.Value);
                setIcon(pictureBoxItemIcon, (int)numItemIcon.Value);
                setIcon(pictureBoxWeaponIcon, (int)numWeaponIcon.Value);
                setIcon(pictureBoxArmorIcon, (int)numArmorIcon.Value);
                setIcon(pictureBoxAcceIcon, (int)numAcceIcon.Value);
            }
        }


        // アイコン横サイズ変更
        private void numIconSizeW_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ICON_WIDTH = (int)numIconSizeW.Value;
            Properties.Settings.Default.Save();
        }


        // アイコン縦サイズ変更
        private void numIconSizeH_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ICON_HEIGHT = (int)numIconSizeH.Value;
            Properties.Settings.Default.Save();
        }

        // パスワード設定
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            //押されたキーがエンターキーかどうかの条件分岐
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                //(1)文字列の半角英数字チェック
                Match result;
                result = Regex.Match(textBoxPassWord.Text, "^[a-zA-Z0-9]+$");
                if (string.IsNullOrEmpty(result.Value))
                {
                    MessageBox.Show("半角英数字のみ設定できます！");
                    textBoxPassWord.Text = Properties.Settings.Default.PASSWORD;
                    return;
                }

                // 設定を保存
                Properties.Settings.Default.PASSWORD = textBoxPassWord.Text;
                Properties.Settings.Default.Save();
            }
        }


        // 暗号化フラグの設定
        private void checkBoxEncryption_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ENCRYPTION_ENABLE = checkBoxEncryption.Checked;
            Properties.Settings.Default.Save();
        }
        #endregion

        #region データ作成・設定
        private MasterItem CreateItem()
        {
            MasterItem mi = new MasterItem();

            mi.name = textBoxItemName.Text;
            mi.desc = textBoxItemDesc.Text;
            mi.cost = (int)numItemCost.Value;
            mi.icon_id = (int)numItemIcon.Value;
            mi.rank = (int)numItemRank.Value;
            mi.id = (int)numItemID.Value;
            mi.group = (MasterItem.Group)comboBoxItemType.SelectedIndex;
            mi.weight = (float)numItemWeight.Value * 0.1f;

            mi.stockEnable = checkBoxItemIsStockable.Checked;
            mi.stockMax = (int)numItemStockMax.Value;
            mi.effectType = (MasterItem.EffectType)comboBoxItemEffecr.SelectedIndex;
            mi.val1 = (int)numItemVal1.Value;
            mi.val2 = (int)numItemVal2.Value;

            return mi;
        }
        private MasterWeapon CreateWeapon()
        {
            MasterWeapon mw = new MasterWeapon();

            mw.name = textBoxWeaponName.Text;
            mw.desc = textBoxWeaponDesc.Text;
            mw.cost = (int)numWeaponCost.Value;
            mw.icon_id = (int)numWeaponIcon.Value;
            mw.rank = (int)numWeaponRank.Value;
            mw.id = (int)numWeaponID.Value;
            mw.group = (MasterWeapon.Group)comboBoxWeaponType.SelectedIndex;
            mw.weight = (float)numWeaponWeight.Value * 0.1f;

            mw.element = (ENElement)comboBoxWeaponElement.SelectedIndex;
            mw.isTwoHand = checkBoxWeaponTwoHand.Checked;
            mw.atk = (int)numWeaponAtk.Value;
            mw.matk = (int)numWeaponMAtk.Value;
            mw.def = (int)numWeaponDef.Value;
            mw.mdef = (int)numWeaponMDef.Value;
            mw.hit = (int)numWeaponHit.Value;
            mw.crt = (int)numWeaponCrt.Value;
            mw.eva = (int)numWeaponEva.Value;

            if (listBoxWeaponOption.Items.Count > 0)
            {
                mw.options = new List<ENOption>();
                for (int i = 0; i < listBoxWeaponOption.Items.Count; i++)
                {
                    mw.options.Add(GetWeaponOption((string)listBoxWeaponOption.Items[i]));
                }
            }

            return mw;
        }
        private MasterArmor CreateArmor()
        {
            MasterArmor ma = new MasterArmor();

            ma.name = textBoxArmorName.Text;
            ma.desc = textBoxArmorDesc.Text;
            ma.cost = (int)numArmorCost.Value;
            ma.icon_id = (int)numArmorIcon.Value;
            ma.rank = (int)numArmorRank.Value;
            ma.id = (int)numArmorID.Value;
            ma.group = (MasterArmor.Group)comboBoxArmorType.SelectedIndex;
            ma.weight = (float)numArmorWeight.Value * 0.1f;

            ma.def = (int)numArmorDef.Value;
            ma.mdef = (int)numArmorMDef.Value;
            ma.eva = (int)numArmorEva.Value;

            return ma;
        }
        private MasterAcce CreateAcce()
        {
            MasterAcce mc = new MasterAcce();

            mc.name = textBoxAcceName.Text;
            mc.desc = textBoxAcceDesc.Text;
            mc.cost = (int)numAcceCost.Value;
            mc.icon_id = (int)numAcceIcon.Value;
            mc.rank = (int)numAcceRank.Value;
            mc.id = (int)numAcceID.Value;
            mc.group = (MasterAcce.Group)comboBoxAcceType.SelectedIndex;
            mc.weight = (float)numAcceWeight.Value * 0.1f;

            mc.atk = (int)numAcceAtk.Value;
            mc.matk = (int)numAcceMAtk.Value;
            mc.def = (int)numAcceDef.Value;
            mc.mdef = (int)numAcceMDef.Value;
            mc.hit = (int)numAcceHit.Value;
            mc.crt = (int)numAcceCrt.Value;
            mc.eva = (int)numAcceEva.Value;

            mc.ability[(int)Ability.HP] = (int)numAcceHP.Value;
            mc.ability[(int)Ability.TP] = (int)numAcceTP.Value;
            mc.ability[(int)Ability.STR] = (int)numAcceSTR.Value;
            mc.ability[(int)Ability.VIT] = (int)numAcceVIT.Value;
            mc.ability[(int)Ability.INT] = (int)numAcceINT.Value;
            mc.ability[(int)Ability.DEX] = (int)numAcceDEX.Value;
            mc.ability[(int)Ability.AGI] = (int)numAcceAGI.Value;
            mc.ability[(int)Ability.LUC] = (int)numAcceLUC.Value;

            return mc;
        }
        private MasterMaterial CreateMaterial()
        {
            MasterMaterial mm = new MasterMaterial();

            mm.name = textBoxMaterialName.Text;
            mm.desc = textBoxMaterialDesc.Text;
            mm.cost = (int)numMaterialCost.Value;
            mm.icon_id = (int)numMaterialIcon.Value;
            mm.rank = (int)numMaterialRank.Value;
            mm.id = (int)numMaterialID.Value;
            mm.group = (MasterMaterial.Group)comboBoxMaterialType.SelectedIndex;
            mm.weight = (float)numMaterialWeight.Value * 0.1f;

            mm.atk = (int)numMaterialAtk.Value;
            mm.matk = (int)numMaterialMAtk.Value;
            mm.def = (int)numMaterialDef.Value;
            mm.mdef = (int)numMaterialMDef.Value;
            mm.hit = (int)numMaterialHit.Value;
            mm.crt = (int)numMaterialCrt.Value;
            mm.eva = (int)numMaterialEva.Value;

            mm.ability[(int)Ability.HP] = (int)numMaterialHP.Value;
            mm.ability[(int)Ability.TP] = (int)numMaterialTP.Value;
            mm.ability[(int)Ability.STR] = (int)numMaterialSTR.Value;
            mm.ability[(int)Ability.VIT] = (int)numMaterialVIT.Value;
            mm.ability[(int)Ability.INT] = (int)numMaterialINT.Value;
            mm.ability[(int)Ability.DEX] = (int)numMaterialDEX.Value;
            mm.ability[(int)Ability.AGI] = (int)numMaterialAGI.Value;
            mm.ability[(int)Ability.LUC] = (int)numMaterialLUC.Value;

            mm.regist[(int)ENElement.Slash] = (int)numSlash.Value;
            mm.regist[(int)ENElement.Strike] = (int)numStrike.Value;
            mm.regist[(int)ENElement.Thrust] = (int)numThrust.Value;
            mm.regist[(int)ENElement.Fire] = (int)numFire.Value;
            mm.regist[(int)ENElement.Aqua] = (int)numAqua.Value;
            mm.regist[(int)ENElement.Thunder] = (int)numThunder.Value;
            mm.regist[(int)ENElement.Dark] = (int)numDark.Value;
            mm.regist[(int)ENElement.Light] = (int)numLight.Value;

            mm.epithet = textBoxMaterialEpithetJP.Text;

            return mm;
        }
        private void LoadItem(MasterItem mi)
        {
            if (mi == null)
            {
                // クリア
                textBoxItemName.Text = "";
                textBoxItemDesc.Text = "";
                numItemCost.Value = 0;
                numItemIcon.Value = 0;
                numItemRank.Value = 0;
                numItemID.Value = 0;
                comboBoxItemType.SelectedIndex = 0;
                numItemWeight.Value = 0;
                checkBoxItemIsStockable.Checked = false;
                numItemStockMax.Value = 0;
                comboBoxItemEffecr.SelectedIndex = 0;
                numItemVal1.Value = 0;
                numItemVal2.Value = 0;
            }
            else
            {
                textBoxItemName.Text = mi.name;
                textBoxItemDesc.Text = mi.desc;
                numItemCost.Value = mi.cost;
                numItemIcon.Value = mi.icon_id;
                numItemRank.Value = mi.rank;
                numItemID.Value = mi.id;
                comboBoxItemType.SelectedIndex = (int)mi.group;
                numItemWeight.Value = (int)(mi.weight * 10);
                checkBoxItemIsStockable.Checked = mi.stockEnable;
                numItemStockMax.Value = mi.stockMax;
                comboBoxItemEffecr.SelectedIndex = (int)mi.effectType;
                numItemVal1.Value = mi.val1;
                numItemVal2.Value = mi.val2;
            }
            setIcon(pictureBoxItemIcon, (int)numItemIcon.Value);
        }
        private void LoadWeapon(MasterWeapon mw)
        {
            if (mw == null)
            {
                textBoxWeaponName.Text = "";
                textBoxWeaponDesc.Text = "";
                numWeaponCost.Value = 0;
                numWeaponIcon.Value = 0;
                numWeaponRank.Value = 0;
                numWeaponID.Value = 0;
                comboBoxWeaponType.SelectedIndex = 0;
                numWeaponWeight.Value = 0;
                comboBoxWeaponElement.SelectedIndex = 0;
                checkBoxWeaponTwoHand.Checked = false;
                numWeaponAtk.Value = 0;
                numWeaponMAtk.Value = 0;
                numWeaponDef.Value = 0;
                numWeaponMDef.Value = 0;
                numWeaponHit.Value = 0;
                numWeaponCrt.Value = 0;
                numWeaponEva.Value = 0;
                comboBoxWeaponOptionType.SelectedIndex = 0;
                numWeaponOptionVal1.Value = 0;
                numWeaponOptionVal2.Value = 0;
                listBoxWeaponOption.Items.Clear();
            }
            else
            {
                textBoxWeaponName.Text = mw.name;
                textBoxWeaponDesc.Text = mw.desc;
                numWeaponCost.Value = mw.cost;
                numWeaponIcon.Value = mw.icon_id;
                numWeaponRank.Value = mw.rank;
                numWeaponID.Value = mw.id;
                comboBoxWeaponType.SelectedIndex = (int)mw.group;
                numWeaponWeight.Value = (int)(mw.weight * 10);
                comboBoxWeaponElement.SelectedIndex = (int)mw.element;
                checkBoxWeaponTwoHand.Checked = mw.isTwoHand;
                numWeaponAtk.Value = mw.atk;
                numWeaponMAtk.Value = mw.matk;
                numWeaponDef.Value = mw.def;
                numWeaponMDef.Value = mw.mdef;
                numWeaponHit.Value = mw.hit;
                numWeaponCrt.Value = mw.crt;
                numWeaponEva.Value = mw.eva;
                comboBoxWeaponOptionType.SelectedIndex = 0;
                numWeaponOptionVal1.Value = 0;
                numWeaponOptionVal2.Value = 0;
                listBoxWeaponOption.Items.Clear();
                if (mw.options != null)
                {
                    for (int i = 0; i < mw.options.Count; i++)
                    {
                        AddWeaponOption(mw.options[i]);
                    }
                }
            }
            setIcon(pictureBoxWeaponIcon, (int)numWeaponIcon.Value);
        }
        private void LoadArmor(MasterArmor ma)
        {
            if (ma == null)
            {
                textBoxArmorName.Text = "";
                textBoxArmorDesc.Text = "";
                numArmorCost.Value = 0;
                numArmorIcon.Value = 0;
                numArmorRank.Value = 0;
                numArmorID.Value = 0;
                comboBoxArmorType.SelectedIndex = 0;
                numArmorWeight.Value = 0;
                numArmorDef.Value = 0;
                numArmorMDef.Value = 0;
                numArmorEva.Value = 0;
            }
            else
            {
                textBoxArmorName.Text = ma.name;
                textBoxArmorDesc.Text = ma.desc;
                numArmorCost.Value = ma.cost;
                numArmorIcon.Value = ma.icon_id;
                numArmorRank.Value = ma.rank;
                numArmorID.Value = ma.id;
                comboBoxArmorType.SelectedIndex = (int)ma.group;
                numArmorWeight.Value = (int)(ma.weight * 10);
                numArmorDef.Value = ma.def;
                numArmorMDef.Value = ma.mdef;
                numArmorEva.Value = ma.eva;
            }
            setIcon(pictureBoxArmorIcon, (int)numArmorIcon.Value);
        }
        private void LoadAcce(MasterAcce mc)
        {
            if (mc == null)
            {
                textBoxAcceName.Text = "";
                textBoxAcceDesc.Text = "";
                numAcceCost.Value = 0;
                numAcceIcon.Value = 0;
                numAcceRank.Value = 0;
                numAcceID.Value = 0;
                comboBoxAcceType.SelectedIndex = 0;
                numAcceWeight.Value = 0;
                numAcceAtk.Value = 0;
                numAcceMAtk.Value = 0;
                numAcceDef.Value = 0;
                numAcceMDef.Value = 0;
                numAcceHit.Value = 0;
                numAcceCrt.Value = 0;
                numAcceEva.Value = 0;

                numAcceHP.Value = 0;
                numAcceTP.Value = 0;
                numAcceSTR.Value = 0;
                numAcceVIT.Value = 0;
                numAcceINT.Value = 0;
                numAcceDEX.Value = 0;
                numAcceAGI.Value = 0;
                numAcceLUC.Value = 0;
            }
            else
            {
                textBoxAcceName.Text = mc.name;
                textBoxAcceDesc.Text = mc.desc;
                numAcceCost.Value = mc.cost;
                numAcceIcon.Value = mc.icon_id;
                numAcceRank.Value = mc.rank;
                numAcceID.Value = mc.id;
                comboBoxAcceType.SelectedIndex = (int)mc.group;
                numAcceWeight.Value = (int)(mc.weight * 10);
                numAcceAtk.Value = mc.atk;
                numAcceMAtk.Value = mc.matk;
                numAcceDef.Value = mc.def;
                numAcceMDef.Value = mc.mdef;
                numAcceHit.Value = mc.hit;
                numAcceCrt.Value = mc.crt;
                numAcceEva.Value = mc.eva;

                numAcceHP.Value = mc.ability[(int)Ability.HP];
                numAcceTP.Value = mc.ability[(int)Ability.TP];
                numAcceSTR.Value = mc.ability[(int)Ability.STR];
                numAcceVIT.Value = mc.ability[(int)Ability.VIT];
                numAcceINT.Value = mc.ability[(int)Ability.INT];
                numAcceDEX.Value = mc.ability[(int)Ability.DEX];
                numAcceAGI.Value = mc.ability[(int)Ability.AGI];
                numAcceLUC.Value = mc.ability[(int)Ability.LUC];
            }

            setIcon(pictureBoxAcceIcon, (int)numAcceIcon.Value);
        }
        private void LoadMaterial(MasterMaterial mm)
        {
            if (mm == null)
            {
                textBoxMaterialName.Text = "";
                textBoxMaterialDesc.Text = "";
                numMaterialCost.Value = 0;
                numMaterialIcon.Value = 0;
                numMaterialRank.Value = 0;
                numMaterialID.Value = 0;
                comboBoxMaterialType.SelectedIndex = 0;
                numMaterialWeight.Value = 0;
                numMaterialAtk.Value = 0;
                numMaterialMAtk.Value = 0;
                numMaterialDef.Value = 0;
                numMaterialMDef.Value = 0;
                numMaterialHit.Value = 0;
                numMaterialCrt.Value = 0;
                numMaterialEva.Value = 0;

                numMaterialHP.Value = 0;
                numMaterialTP.Value = 0;
                numMaterialSTR.Value = 0;
                numMaterialVIT.Value = 0;
                numMaterialINT.Value = 0;
                numMaterialDEX.Value = 0;
                numMaterialAGI.Value = 0;
                numMaterialLUC.Value = 0;

                numSlash.Value = 0;
                numStrike.Value = 0;
                numThrust.Value = 0;
                numFire.Value = 0;
                numAqua.Value = 0;
                numThunder.Value = 0;
                numDark.Value = 0;
                numLight.Value = 0;

                textBoxMaterialEpithetJP.Text = "";
            }
            else
            {
                textBoxMaterialName.Text = mm.name;
                textBoxMaterialDesc.Text = mm.desc;
                numMaterialCost.Value = mm.cost;
                numMaterialIcon.Value = mm.icon_id;
                numMaterialRank.Value = mm.rank;
                numMaterialID.Value = mm.id;
                comboBoxMaterialType.SelectedIndex = (int)mm.group;
                numMaterialWeight.Value = (int)(mm.weight * 10);
                numMaterialAtk.Value = mm.atk;
                numMaterialMAtk.Value = mm.matk;
                numMaterialDef.Value = mm.def;
                numMaterialMDef.Value = mm.mdef;
                numMaterialHit.Value = mm.hit;
                numMaterialCrt.Value = mm.crt;
                numMaterialEva.Value = mm.eva;

                numMaterialHP.Value = mm.ability[(int)Ability.HP];
                numMaterialTP.Value = mm.ability[(int)Ability.TP];
                numMaterialSTR.Value = mm.ability[(int)Ability.STR];
                numMaterialVIT.Value = mm.ability[(int)Ability.VIT];
                numMaterialINT.Value = mm.ability[(int)Ability.INT];
                numMaterialDEX.Value = mm.ability[(int)Ability.DEX];
                numMaterialAGI.Value = mm.ability[(int)Ability.AGI];
                numMaterialLUC.Value = mm.ability[(int)Ability.LUC];

                numSlash.Value = mm.regist[(int)ENElement.Slash];
                numStrike.Value = mm.regist[(int)ENElement.Strike];
                numThrust.Value = mm.regist[(int)ENElement.Thrust];
                numFire.Value = mm.regist[(int)ENElement.Fire];
                numAqua.Value = mm.regist[(int)ENElement.Aqua];
                numThunder.Value = mm.regist[(int)ENElement.Thunder];
                numDark.Value = mm.regist[(int)ENElement.Dark];
                numLight.Value = mm.regist[(int)ENElement.Light];

                textBoxMaterialEpithetJP.Text = mm.epithet;
            }
            setIcon(pictureBoxMaterialIcon, (int)numMaterialIcon.Value);
        }
        private void buttonWeaponOptionAdd_Click(object sender, EventArgs e)
        {
            // オプションを追加
            AddWeaponOption(GetWeaponOption());
        }
        /// <summary>フォーム内の入力データからオプションデータを作成する</summary>
        ENOption GetWeaponOption()
        {
            ENOption op = new ENOption();
            op.type = (ENOption.OptionType)comboBoxWeaponOptionType.SelectedIndex;
            op.value1 = (int)numWeaponOptionVal1.Value;
            op.value2 = (int)numWeaponOptionVal2.Value;

            return op;
        }
        /// <summary>文字列フォーマットからオプションデータを復元する</summary>
        ENOption GetWeaponOption(string format)
        {
            string[] s = format.Split(split);
            if (s.Length < 2)
            {
                MessageBox.Show("フォーマットが正しくありません！");
                return null;
            }
            string[] ss = s[1].Split(',');
            ENOption op = new ENOption();
            if (ss.Length > 0)
                op.type = (ENOption.OptionType)Convert.ToInt32(ss[0]);
            if (ss.Length > 1)
                op.value1 = Convert.ToInt32(ss[1]);
            if (ss.Length > 2)
                op.value2 = Convert.ToInt32(ss[2]);

            return op;
        }
        /// <summary>入力したフォームのデータをリストボックスに追加</summary>
        void AddWeaponOption(ENOption op)
        {
            // 表示フォーマットを設定
            string s;
            s = op.GetDesc() + "\t\t" + split +
                (int)op.type + "," + op.value1 + "," + op.value2;
            listBoxWeaponOption.Items.Add(s);
        }
        /// <summary>オプション（データ）から入力フォームを表示</summary>
        void SetWeaponOption(ENOption op)
        {
            comboBoxWeaponOptionType.SelectedIndex = (int)op.type;
            numWeaponOptionVal1.Value = op.value1;
            numWeaponOptionVal2.Value = op.value2;
        }
        /// <summary>文字列フォーマットから入力フォームを表示</summary>
        void SetWeaponOption(string format)
        {
            string[] s = format.Split(split);
            if (s.Length < 2)
            {
                MessageBox.Show("フォーマットが正しくありません！");
                return;
            }
            string[] ss = s[1].Split(',');
            if (ss.Length > 0)
                comboBoxWeaponOptionType.SelectedIndex = Convert.ToInt32(ss[0]);
            if (ss.Length > 1)
                numWeaponOptionVal1.Value = Convert.ToInt32(ss[1]);
            if (ss.Length > 2)
                numWeaponOptionVal2.Value = Convert.ToInt32(ss[2]);
        }
        #endregion

        #region リストボックス操作
        private void SetListBoxItem()
        {
            int index = tabControl1.SelectedIndex;
            switch (index)
            {
                case 0:// 道具
                    if (listBoxItem.SelectedItem == null)
                    {
                        //                            MessageBox.Show("読み込むアイテムが選択されていません！");
                        return;
                    }

                    MasterItem mi = (MasterItem)listBoxItem.SelectedItem;
                    if (mi == null) return;

                    // 読み込み
                    LoadItem(mi);
                    break;

                case 1:// 武器
                    if (listBoxWeapon.SelectedItem == null) return;

                    MasterWeapon mw = (MasterWeapon)listBoxWeapon.SelectedItem;
                    if (mw == null) return;

                    // 読み込み
                    LoadWeapon(mw);
                    break;

                case 2:// 防具
                    if (listBoxArmor.SelectedItem == null) return;

                    MasterArmor ma = (MasterArmor)listBoxArmor.SelectedItem;
                    if (ma == null) return;

                    // 読み込み
                    LoadArmor(ma);
                    break;

                case 3:// アクセサリ
                    if (listBoxAcce.SelectedItem == null) return;

                    MasterAcce mc = (MasterAcce)listBoxAcce.SelectedItem;
                    if (mc == null) return;

                    // 読み込み
                    LoadAcce(mc);
                    break;
                case 4:// マテリアル
                    if (listBoxMaterial.SelectedItem == null) return;

                    MasterMaterial mm = (MasterMaterial)listBoxMaterial.SelectedItem;
                    if (mm == null) return;

                    // 読み込み
                    LoadMaterial(mm);
                    break;
                case 5:// 設定
                    break;
                default:
                    MessageBox.Show("タブが範囲外です！");
                    break;
            }
        }

        private void DeleteListBoxItems()
        {
            int index = tabControl1.SelectedIndex;
            switch (index)
            {
                case 0:// 道具
                    if (listBoxItem.SelectedItems == null || listBoxItem.Items.Count <= 0)
                    {
                        MessageBox.Show("削除するアイテムが選択されていません！");
                        return;
                    }

                    if (Properties.Settings.Default.CONFIRM_DELETE)
                    {
                        //メッセージボックスを表示する
                        DialogResult result = MessageBox.Show("選択したアイテムを削除しますか？",
                            "確認",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button2);

                        if (result == DialogResult.No)
                            return; // 中止
                    }
                    MasterItem mi;
                    int max = listBoxItem.SelectedItems.Count;
                    int idx = 0;
                    for (int i = 0; i < max; i++)
                    {
                        mi = (MasterItem)listBoxItem.SelectedItems[idx];
                        if (mi == null)
                        {
                            idx++;
                            continue;
                        }

                        // 削除
                        listItem.Remove(mi);
                        listBoxItem.Items.Remove(mi);
                    }
                    break;

                case 1:// 武器
                    if (listBoxWeapon.SelectedItems == null || listBoxWeapon.SelectedItems.Count <= 0)
                    {
                        MessageBox.Show("削除するアイテムが選択されていません！");
                        return;
                    }

                    if (Properties.Settings.Default.CONFIRM_DELETE)
                    {
                        //メッセージボックスを表示する
                        DialogResult result = MessageBox.Show("選択したアイテムを削除しますか？",
                            "確認",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button2);

                        if (result == DialogResult.No)
                            return; // 中止
                    }

                    MasterWeapon mw;
                    max = listBoxWeapon.SelectedItems.Count;
                    idx = 0;
                    for (int i = 0; i < max; i++)
                    {
                        mw = (MasterWeapon)listBoxWeapon.SelectedItems[idx];
                        if (mw == null)
                        {
                            idx++;
                            continue;
                        }
                        // 削除
                        listWeapon.Remove(mw);
                        listBoxWeapon.Items.Remove(mw);
                    }
                    break;

                case 2:// 防具
                    if (listBoxArmor.SelectedItem == null)
                    {
                        MessageBox.Show("削除するアイテムが選択されていません！");
                        return;
                    }

                    if (Properties.Settings.Default.CONFIRM_DELETE)
                    {
                        //メッセージボックスを表示する
                        DialogResult result = MessageBox.Show("選択したアイテムを削除しますか？",
                            "確認",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button2);

                        if (result == DialogResult.No)
                            return; // 中止
                    }

                    MasterArmor ma;
                    max = listBoxArmor.SelectedItems.Count;
                    idx = 0;
                    for (int i = 0; i < max; i++)
                    {
                        ma = (MasterArmor)listBoxArmor.SelectedItems[idx];
                        if (ma == null)
                        {
                            idx++;
                            continue;
                        }
                        // 削除
                        listArmor.Remove(ma);
                        listBoxArmor.Items.Remove(ma);
                    }
                    break;

                case 3:// アクセサリ
                    if (listBoxAcce.SelectedItem == null)
                    {
                        MessageBox.Show("削除するアイテムが選択されていません！");
                        return;
                    }

                    if (Properties.Settings.Default.CONFIRM_DELETE)
                    {
                        //メッセージボックスを表示する
                        DialogResult result = MessageBox.Show("選択したアイテムを削除しますか？",
                            "確認",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button2);

                        if (result == DialogResult.No)
                            return; // 中止
                    }

                    MasterAcce mc;
                    max = listBoxAcce.SelectedItems.Count;
                    idx = 0;
                    for (int i = 0; i < max; i++)
                    {
                        mc = (MasterAcce)listBoxAcce.SelectedItems[idx];
                        if (mc == null)
                        {
                            idx++;
                            continue;
                        }
                        // 削除
                        listAcce.Remove(mc);
                        listBoxAcce.Items.Remove(mc);
                    }
                    break;

                case 4:// マテリアル
                    if (listBoxMaterial.SelectedItem == null)
                    {
                        MessageBox.Show("削除するアイテムが選択されていません！");
                        return;
                    }

                    if (Properties.Settings.Default.CONFIRM_DELETE)
                    {
                        //メッセージボックスを表示する
                        DialogResult result = MessageBox.Show("選択したアイテムを削除しますか？",
                            "確認",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button2);

                        if (result == DialogResult.No)
                            return; // 中止
                    }

                    MasterMaterial mm;
                    max = listBoxMaterial.SelectedItems.Count;
                    idx = 0;
                    for (int i = 0; i < max; i++)
                    {
                        mm = (MasterMaterial)listBoxMaterial.SelectedItems[idx];
                        if (mm == null)
                        {
                            idx++;
                            continue;
                        }
                        // 削除
                        listMaterial.Remove(mm);
                        listBoxMaterial.Items.Remove(mm);
                    }
                    break;

                case 5:// 設定
                    break;
                default:
                    break;
            }
        }

        // キー押下
        private void listBoxItem_KeyDown(object sender, KeyEventArgs e)
        {
            //押されたキーがエンターキーかどうかの条件分岐
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                SetListBoxItem();
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.Delete)
            {
                DeleteListBoxItems();
            }
        }
        private void listBoxWeapon_KeyDown(object sender, KeyEventArgs e)
        {
            //押されたキーがエンターキーかどうかの条件分岐
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                SetListBoxItem();
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.Delete)
            {
                DeleteListBoxItems();
            }
        }
        private void listBoxArmor_KeyDown(object sender, KeyEventArgs e)
        {
            //押されたキーがエンターキーかどうかの条件分岐
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                SetListBoxItem();
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.Delete)
            {
                DeleteListBoxItems();
            }
        }
        private void listBoxAcce_KeyDown(object sender, KeyEventArgs e)
        {
            //押されたキーがエンターキーかどうかの条件分岐
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                SetListBoxItem();
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.Delete)
            {
                DeleteListBoxItems();
            }
        }
        private void listBoxMaterial_KeyDown(object sender, KeyEventArgs e)
        {
            //押されたキーがエンターキーかどうかの条件分岐
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                if (listBoxWeaponOption.SelectedItem != null)
                {
                    // 読み込み
                    SetWeaponOption((string)listBoxWeaponOption.SelectedItem);
                }
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.Delete)
            {
                DeleteListBoxItems();
            }
        }
        // ダブルクリック
        private void listBoxItem_DoubleClick(object sender, EventArgs e)
        {
            SetListBoxItem();
        }
        private void listBoxWeapon_DoubleClick(object sender, EventArgs e)
        {
            SetListBoxItem();
        }
        private void listBoxArmor_DoubleClick(object sender, EventArgs e)
        {
            SetListBoxItem();
        }
        private void listBoxAcce_DoubleClick(object sender, EventArgs e)
        {
            SetListBoxItem();
        }
        private void listBoxMaterial_DoubleClick(object sender, EventArgs e)
        {
            SetListBoxItem();
        }

        private void listBoxWeaponOption_KeyDown(object sender, KeyEventArgs e)
        {
            //押されたキーがエンターキーかどうかの条件分岐
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                // 読み込み
                SetWeaponOption((string)listBoxWeaponOption.SelectedItem);
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.Delete)
            {
                // 削除
                listBoxWeaponOption.Items.Remove(listBoxWeaponOption.SelectedItem);
            }
        }
        private void listBoxWeaponOption_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxWeaponOption.SelectedItem != null)
            {
                // 読み込み
                SetWeaponOption((string)listBoxWeaponOption.SelectedItem);
            }
        }
        #endregion

        /// <summary>
        /// Menu - ファイルを開く
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemLoadFile_Click(object sender, EventArgs e)
        {
            // ファイル読み込み処理
            int res = LoadFromFile();
            if (res != 0)
            {
                MessageBox.Show("読み込みに失敗しました！");
            }
        }
        /// <summary>
        /// 名前をつけて保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            int index = tabControl1.SelectedIndex;
            switch (index)
            {
                case 0:// 道具
                    int result = SaveItem();
                    if (result != 0)
                    {
                        MessageBox.Show("保存に失敗しました！");
                    }
                    break;
                case 1:// 武器
                    result = SaveWeapon();
                    if (result != 0)
                    {
                        MessageBox.Show("保存に失敗しました！");
                    }
                    break;
                case 2:// 防具
                    result = SaveArmor();
                    if (result != 0)
                    {
                        MessageBox.Show("保存に失敗しました！");
                    }
                    break;
                case 3:// アクセサリ
                    result = SaveAcce();
                    if (result != 0)
                    {
                        MessageBox.Show("保存に失敗しました！");
                    }
                    break;
                case 4:// マテリアル
                    result = SaveMaterial();
                    if (result != 0)
                    {
                        MessageBox.Show("保存に失敗しました！");
                    }
                    break;
                case 5:// 設定
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Menu - すべて保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemSaveAll_Click(object sender, EventArgs e)
        {
            int result = SaveAll();
            if (result != 0)
            {
                MessageBox.Show("保存に失敗しました！");
            }
        }

        // 武器のオプション種別コンボボックス選択時
        private void comboBoxWeaponOptionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 説明文の更新
        }

        //　アイテム使用効果コンボボックス選択時
        private void comboBoxItemEffecr_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 説明文の更新
            labelItemEffectDesc.Text = "説明\n" + ((MasterItem.EffectType)comboBoxItemEffecr.SelectedIndex).DispDescription();
        }

        // アイコンクリック
        private void pictureBoxItemIcon_Click(object sender, EventArgs e)
        {
            // アイコン選択ダイアログ表示
            IconSelectDialog icondialog = new IconSelectDialog(bmpIcon, Properties.Settings.Default.ICON_WIDTH, Properties.Settings.Default.ICON_HEIGHT);
            DialogResult result = icondialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                numItemIcon.Value = icondialog.selectedIndex;
                setIcon(pictureBoxItemIcon, (int)numItemIcon.Value);
            }
        }
        private void pictureBoxWeaponIcon_Click(object sender, EventArgs e)
        {
            // アイコン選択ダイアログ表示
            IconSelectDialog icondialog = new IconSelectDialog(bmpIcon, Properties.Settings.Default.ICON_WIDTH, Properties.Settings.Default.ICON_HEIGHT);
            DialogResult result = icondialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                numWeaponIcon.Value = icondialog.selectedIndex;
                setIcon(pictureBoxWeaponIcon, (int)numWeaponIcon.Value);
            }
        }
        private void pictureBoxArmorIcon_Click(object sender, EventArgs e)
        {
            // アイコン選択ダイアログ表示
            IconSelectDialog icondialog = new IconSelectDialog(bmpIcon, Properties.Settings.Default.ICON_WIDTH, Properties.Settings.Default.ICON_HEIGHT);
            DialogResult result = icondialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                numArmorIcon.Value = icondialog.selectedIndex;
                setIcon(pictureBoxArmorIcon, (int)numArmorIcon.Value);
            }
        }
        private void pictureBoxAcceIcon_Click(object sender, EventArgs e)
        {
            // アイコン選択ダイアログ表示
            IconSelectDialog icondialog = new IconSelectDialog(bmpIcon, Properties.Settings.Default.ICON_WIDTH, Properties.Settings.Default.ICON_HEIGHT);
            DialogResult result = icondialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                numAcceIcon.Value = icondialog.selectedIndex;
                setIcon(pictureBoxAcceIcon, (int)numAcceIcon.Value);
            }
        }
        private void pictureBoxMaterialIcon_Click(object sender, EventArgs e)
        {
            // アイコン選択ダイアログ表示
            IconSelectDialog icondialog = new IconSelectDialog(bmpIcon, Properties.Settings.Default.ICON_WIDTH, Properties.Settings.Default.ICON_HEIGHT);
            DialogResult result = icondialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                numMaterialIcon.Value = icondialog.selectedIndex;
                setIcon(pictureBoxMaterialIcon, (int)numMaterialIcon.Value);
            }
        }


        // 切り取り
        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 型チェック
                if (ActiveControl.GetType() != typeof(ListBox))
                    return;
                ListBox lb = (ListBox)ActiveControl;
                if (lb == null) { return; }
                Console.WriteLine("aaaa");

                List<ItemBase> work = new List<ItemBase>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        // コピー
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        // ペースト
        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        // 削除
        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteListBoxItems();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        // 貼り付け/挿入
        private void InserttoolStripMenuItem_Click(object sender, EventArgs e)
        {
            InserttoolStripMenuItem.Checked = !InserttoolStripMenuItem.Checked;
            AddToolStripMenuItem.Checked = !AddToolStripMenuItem.Checked;
        }

        // 貼り付け/上書き
        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InserttoolStripMenuItem.Checked = !InserttoolStripMenuItem.Checked;
            AddToolStripMenuItem.Checked = !AddToolStripMenuItem.Checked;
        }






    }
}
