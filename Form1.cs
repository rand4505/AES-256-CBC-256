/*Programmer: John Grubbs
 *Program: Super Simple AES-256-CBC-256 Crypted String/File generator
 *Final Update: 10 July 2013
 *Start Date: 12 FEB 2013
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace AESEncryptAndDecrypt
{
    public partial class Form1 : Form
    {
        private string Mode = new string(new char[50]);
        private SimplerAES myAES = new SimplerAES();
        private int _KeyCount { get { return myAES.key.Count(); } set { myAES.key.Count(); } }
        private int _VectCount { get { return myAES.vector.Count(); } set { myAES.vector.Count(); } }
        private const int KeyMaxSize = 32;
        private const int VectMaxSize = 32;
        
        private bool CheckCounts(){
            int check=0;
            try {
                if (_KeyCount == KeyMaxSize) { KeyArrayBox.ForeColor = Color.Green; check++; }
                else { KeyArrayBox.ForeColor = Color.Red; check += 10; }
                if (_VectCount == VectMaxSize) { VectorArrayBox.ForeColor = Color.Green; check++; }
                else { VectorArrayBox.ForeColor = Color.Red; check += 10; }
                foreach (Byte Value in myAES.key) {
                    if ((Convert.ToInt32(Value) > Byte.MaxValue) || (Convert.ToInt32(Value) < Byte.MinValue)) {
                        KeyArrayBox.ForeColor = Color.Red;
                        return false;
                    }
                }
                foreach (Byte Value in myAES.vector) {
                    if ((Convert.ToInt32(Value) > Byte.MaxValue) || (Convert.ToInt32(Value) < Byte.MinValue)) {
                        VectorArrayBox.ForeColor = Color.Red;
                        return false;
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Fix the Error." + ex.Message);
                InitArrayBoxes();
            }
            
            if (check == 2) return true;
            else return false;
        }


        public Form1() {
            InitializeComponent();
            InitArrayBoxes();
        }

        private void InitArrayBoxes() {
            KeyArrayBox.Text = "";
            VectorArrayBox.Text = "";
            int x = 0, y = 0;
            foreach (int Num in myAES.key) {
                x++;
                KeyArrayBox.Text += Num + (x==KeyMaxSize ? "" : ",");
            }
            foreach (int Num in myAES.vector) {
                y++;
                VectorArrayBox.Text += Num + (y==VectMaxSize ? "" : ",");
            }
            CheckCounts();
        }

        private string GetFileName(OpenFileDialog Dlg)
        {
            string FileName="";
            Dlg.InitialDirectory = "c:\\";
            Dlg.Filter = "All files (*.*)|*.*";
            Dlg.FilterIndex = 2;
            Dlg.RestoreDirectory = true;
            if (Dlg.ShowDialog() == DialogResult.OK) {
                try {
                    FileName = Dlg.FileName.ToString();
                    if (!Dlg.Equals(null)) Dlg.Dispose();
                }
                catch (Exception ex) {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    if (!Dlg.Equals(null)) Dlg.Dispose();
                }
            }
            return FileName;
        }

        private string RetString(GetString dlg) {
            string Retme = "";
            if (dlg.ShowDialog() == DialogResult.OK) {
                Retme = dlg.Password;
            }
            return Retme;
        }
        
        private void GoGoGadget_Click(object sender, EventArgs e) {
            RTB.Clear();
            if (FileMode_checkBox.Checked == false) {
                if (Mode == "Encrypt") {
                    RTB.AppendText(myAES.Encrypt(InputBox.Text.ToString()));
                }
                else if (Mode == "Decrypt") {
                    RTB.AppendText(myAES.Decrypt(InputBox.Text.ToString()));
                }
                else {
                    MessageBox.Show("Must Pick a Mode, Encrypt or Decrypt");
                }
            }
            else {
                OpenFileDialog myFile = new OpenFileDialog();
                GetString Dlg = new GetString();
                if ((Mode == "Encrypt") || (Mode == "Decrypt")) {
                    SimplerAES Default = new SimplerAES();
                    string pass = RetString(Dlg);
                    byte[] Cvted = Encoding.ASCII.GetBytes(Default.Encrypt(pass));
                    string file = GetFileName(myFile);
                    SimplerAES FileCrypto = new SimplerAES(Cvted, Cvted);
                    RTB.Clear();
                    RTB.AppendText(Mode + (FileCrypto.FileOps( file , Mode, pass) ? " success" : " failed"));
                }
                else MessageBox.Show("Must Pick a Mode, Encrypt or Decrypt");
            }
        }

        private void EncryptButton_CheckedChanged(object sender, EventArgs e) {
            Mode = "Encrypt";
        }

        private void DecryptButton_CheckedChanged(object sender, EventArgs e) {
            Mode = "Decrypt";
        }

        private void UpDateKeyAr_Click(object sender, EventArgs e)
        {
            string[] SplitMe = KeyArrayBox.Text.ToString().Split(",".ToArray());
            int Size = SplitMe.Count();
            byte[] PassToAES = new byte[Size];
            int Pos = 0;
            if (KeyArrayBox.ForeColor != Color.Red) {
                foreach (string Value in SplitMe) {
                    if (Pos == (SplitMe.Count())) break;
                    PassToAES[Pos] = Convert.ToByte(Value);
                    Pos++;
                }
                myAES.key = PassToAES;
                myAES.UpDateArrays();
                InitArrayBoxes();
            }
            else { MessageBox.Show("Fix the error first."); InitArrayBoxes();}
            
        }

        private void VectorUpdateBtn_Click(object sender, EventArgs e) {
            string[] SplitMe = VectorArrayBox.Text.ToString().Split(",".ToArray());
            int Size = SplitMe.Count();
            byte[] PassToAES = new byte[Size];
            int Pos = 0;
            if (VectorArrayBox.ForeColor != Color.Red) {
                foreach (string Value in SplitMe) {
                    if (Pos == (SplitMe.Count())) break;
                    PassToAES[Pos] = Convert.ToByte(Value);
                    Pos++;
                }
                myAES.vector = PassToAES;
                myAES.UpDateArrays();
                InitArrayBoxes();
                CheckCounts();
            }
            else { MessageBox.Show("Fix the error first."); InitArrayBoxes();}
            
        }

        private void randarrays_Click(object sender, EventArgs e) {
            Random SeedPart = new Random(DateTime.Now.Millisecond);
            Random PRNG = new Random(DateTime.Now.Millisecond * SeedPart.Next(50));
            for (int rk = 0; rk < KeyMaxSize; rk++) {
                PRNG.NextBytes(myAES.key);
            }
            
            PRNG = new Random(DateTime.Now.Millisecond * SeedPart.Next(50));
            for (int rv = 0; rv < VectMaxSize; rv++) {
                PRNG.NextBytes(myAES.vector);
            }
            myAES.UpDateArrays();
            InitArrayBoxes();
            CheckCounts();
        }

        private void KeyArrayChanged(object Sender, EventArgs e) {
            try {
                string[] Split = KeyArrayBox.Text.Split(",".ToArray());
                if (Split.Count() != KeyMaxSize) KeyArrayBox.ForeColor = Color.Red;
                else KeyArrayBox.ForeColor = Color.Green;
                foreach (string Value in Split) {
                    if (Value.Length <= 0) goto breakpt;
                    if ((Value.Length > 3) || (Convert.ToInt32(Value) > Byte.MaxValue) || (Convert.ToInt32(Value) < Byte.MinValue)) KeyArrayBox.ForeColor = Color.Red;
                }
            breakpt: ;
            }
            catch (Exception ex){
                MessageBox.Show("Valid ranges for Array Values are 0-255");
                InitArrayBoxes();
            }
        }

        private void VectorArrayChanged(object Sender, EventArgs e) {
            try {
                string[] Split = VectorArrayBox.Text.Split(",".ToArray());
                if (Split.Count() != VectMaxSize) VectorArrayBox.ForeColor = Color.Red;
                else VectorArrayBox.ForeColor = Color.Green;
                foreach (string Value in Split) {
                    if (Value.Length <= 0) goto breakout;
                    if ((Value.Length > 3) || (Convert.ToInt32(Value) > Byte.MaxValue) || (Convert.ToInt32(Value) < Byte.MinValue)) VectorArrayBox.ForeColor = Color.Red;
                }
            breakout: ;
            }
            catch (Exception ex) {
                MessageBox.Show("Valid ranges for Array Values are 0-255");
                InitArrayBoxes();
            }
        }

        private void FileMode_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (FileMode_checkBox.Checked == true) {
                EncryptButton.Text = "File Encrypt";
                DecryptButton.Text = "File Decrypt";
            }
            else {
                EncryptButton.Text = "Encrypt";
                DecryptButton.Text = "Decrypt";
            }
        }
    }
}
