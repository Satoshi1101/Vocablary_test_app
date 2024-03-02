using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic.FileIO;

namespace CSVget02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// CSVファイル読み込み処理
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private void cmdCsvImport_Click(object sender, EventArgs e)
        {
            string strFilePath = txtPath.Text;
            try
            {
                //未入力チェック
                if (strFilePath.Trim() == "")
                {
                    MessageBox.Show("ファイルパスを入力して下さい。"
                                    , "未入力チェック");
                }

                //ファイル存在チェック
                if (System.IO.File.Exists(strFilePath) == false)
                {
                    MessageBox.Show("ファイルが存在しません。"
                                    , "ファイル存在チェック");
                }

                //TextFieldParserのインスタンス生成
                var objParser = new TextFieldParser(strFilePath
                                                   , Encoding.GetEncoding("Shift_JIS"));

                using (objParser)
                {
                    //--CSV読み込み準備 START --//
                    // 区切り文字をカンマに設定
                    objParser.TextFieldType = FieldType.Delimited;
                    objParser.SetDelimiters(",");

                    // 空白があった場合にTrimしない
                    objParser.TrimWhiteSpace = false;
                    //--CSV読み込み準備  END  --//


                    //--CSV読み込み処理  START --//
                    // ファイルの最後まで1行ずつループ
                    while (!objParser.EndOfData)
                    {
                        string strVeiwResult = "";
                        // フィールドを読込
                        string[] arrayRow = objParser.ReadFields();
                        string[,] data = ReadCSV(strFilePath);

                        foreach (string strField in arrayRow)
                        {
                            // カンマ区切りで出力文字列を生成
                            strVeiwResult = strVeiwResult + strField + ",";
                        }



                        //列の末尾のカンマを削除
                        strVeiwResult = strVeiwResult.TrimEnd(',');

                        //テキストボックスに表示
                        txtResult.Text = txtResult.Text + strVeiwResult + Environment.NewLine;
                    }
                    //--CSV読み込み処理  END  --//
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message
                                                   , "エラー");
            }
        }

        // CSVファイルの内容を2次元配列に格納
        static string[,] ReadCSV(string filePath)
        {
            // CSVファイルの各行を配列として読み込む
            string[] lines = File.ReadAllLines(filePath, Encoding.GetEncoding("Shift_JIS"));


            // CSVファイルの行数と列数を取得
            int rowCount = lines.Length;
            int columnCount = lines[0].Split(',').Length;

            // 2次元配列を作成
            string[,] data = new string[rowCount, columnCount];

            // CSVファイルからデータを2次元配列に格納
            for (int i = 0; i < rowCount; i++)
            {
                string[] fields = lines[i].Split(',');
                for (int j = 0; j < columnCount; j++)
                {
                    data[i, j] = fields[j];
                }
            }

            return data;
        }

        private void txtPath_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtResult_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

}

