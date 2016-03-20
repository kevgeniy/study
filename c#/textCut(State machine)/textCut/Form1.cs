using System;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace textCut
{
    public partial class Form1 : Form
    {
        int length;
        StreamReader fileR;
        StreamWriter fileW;
        OpenFileDialog fileIn;
        SaveFileDialog fileOut;
        bool isEng;
        bool isRu;
        const string endFormat = "Formatting was completed successfully! Would you like to format yet one file?";

        char[] ruVowels = { 'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я', 'А', 'Е', 'Ё', 'И', 'О', 'У', 'Ы', 'Э', 'Ю', 'Я' };
        char[] ruConsonants = {'б', 'в', 'г', 'д', 'ж', 'з', 'к', 'л', 'м', 'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ь', 
                             'Б', 'В', 'Г', 'Д', 'Ж', 'З', 'К', 'Л', 'М', 'Н', 'П', 'Р', 'С', 'Т', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ь'};

        char[] enVowels = { 'a', 'e', 'i', 'o', 'u', 'y' };
        char[] enConsonants = { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'z' };

        public Form1()
        {
            InitializeComponent();
            Initialization();
        }

        private void Initialization()
        {
            length = 0;
            fileR = null;
            fileW = null;
            isEng = false;
            isRu = false;
        }

        private void Length_TextChanged(object sender, EventArgs e)
        {
            if (LengthT.Text == "0" || !Int32.TryParse(LengthT.Text, out length))
                LengthT.Clear();
        }

        private void LengthT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) || LengthT.Text + e.KeyChar == "0")
                e.Handled = true;
        }

        private void Open_Click(object sender, EventArgs e)
        {
            fileIn = new OpenFileDialog();
            fileIn.Title = "Open";
            fileIn.CheckPathExists = true;
            fileIn.CheckFileExists = true;
            fileIn.InitialDirectory = @"C:\Users\Евгений\Documents";
            fileIn.Filter = "Text Files (*.txt)|*.txt";
            if (fileIn.ShowDialog() == DialogResult.OK)
                fileR = new StreamReader(fileIn.FileName, Encoding.GetEncoding(1251));
        }

        private void Save_Click(object sender, EventArgs e)
        {
            fileOut = new SaveFileDialog();
            fileOut.Title = "Save as...";
            fileOut.CreatePrompt = true;
            fileOut.OverwritePrompt = true;
            fileOut.CheckPathExists = true;
            fileOut.InitialDirectory = @"C:\Users\Евгений\Documents";
            fileOut.Filter = "Text Files (*.txt)|*.txt";
            if (fileOut.ShowDialog() == DialogResult.OK)
                fileW = new StreamWriter(fileOut.FileName);
        }

        private void FormatB_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isEng && !isRu)
                {
                    MessageBox.Show("Set the language, please!");
                    return;
                }
                if (MessageBox.Show("Continue?", "", MessageBoxButtons.YesNo).ToString() == "Yes")
                {
                    this.Visible = false;
                    if (fileW == null)
                        fileW = new StreamWriter(fileOut.FileName);
                    FormatMachine a = new FormatMachine();
                    a.Length = length;
                    if (isEng)
                    {
                        a.Consonants = enConsonants;
                        a.Vowels = enVowels;
                    }
                    else
                    {
                        a.Consonants = ruConsonants;
                        a.Vowels = ruVowels;
                    }
                    a.FormatFromFile(fileR, fileW);
                    fileR.Close();
                    fileW.Close();
                    if (MessageBox.Show(endFormat, "", MessageBoxButtons.YesNo).ToString() == "No")
                        this.Close();
                    else
                        end();
                }
            }
            catch (Exception ex)
            {
                    MessageBox.Show(ex.Message);
                    end();
            }
        }

        private void end()
        {
            this.Visible = true;
            fileR = new StreamReader(fileIn.FileName, Encoding.GetEncoding(1251));
            fileW = null;
        }

        private void Eng_CheckedChanged(object sender, EventArgs e)
        { isEng = !isEng; }

        private void Rus_CheckedChanged(object sender, EventArgs e)
        { isRu = !isRu; }
    }
}