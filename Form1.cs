/*Programmer: John Grubbs
 *Program: Super Simple AES-256-CBC-128 Crypted String/File generator
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
using Entropy;
using System.Windows.Forms;
using BCrypt.Net;

namespace AES256
{
    public partial class Form1 : Form
    {
        private int Seed = 0;
        private bool AmUsingBcrypt = false;
        public enum Algo { AES256CBC256, AES256CBC128, AES256CFB128, AES128CBC128};
        private string[] AlgoArrayComp = { "256-AES-256-CBC", "256-AES-128-CBC(Nist Rijndael)", "256-AES-128-CFB", "128-AES-128-CBC" };
        private Algo AlgorithmChoice = new Algo();
        private string Mode = new string(new char[50]);
        private SimplerAES myAES = new SimplerAES();
        private int _KeyCount { get { return myAES.key.Count(); } set { myAES.key.Count(); } }
        private int _VectCount { get { return myAES.vector.Count(); } set { myAES.vector.Count(); } }
        private int kSize = 32;
        private int vSize = 16;
        
        private bool CheckCounts(){
            int check=0;
            try {
                if (_KeyCount == kSize) { KeyArrayBox.ForeColor = Color.Green; check++; }
                else { KeyArrayBox.ForeColor = Color.Red; check += 10; }
                if (_VectCount == vSize) { VectorArrayBox.ForeColor = Color.Green; check++; }
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

        private string PromptForBCryptHashGeneration() {
            string Original="",Retme = "";
            GetString dlg = new GetString();
            dlg.Text = "Enter Password >20 chars";
            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.OK) {
                if (dlg.Password.Length > 20) {
                    Retme = dlg.Password;
                }
                else {
                    MessageBox.Show("Password must be longer than 20 chars");
                    dlg.ClearPass();
                    Retme = RetString(dlg);
                }
                string salt = BCrypt.Net.BCrypt.GenerateSalt(12);//4096 iterations
                Original = Retme;
                Retme = BCrypt.Net.BCrypt.HashPassword(Retme, salt);
                bool Check = BCrypt.Net.BCrypt.Verify(Original, Retme);
                Retme = Retme.TrimStart("$2a$12$".ToArray());
                if (Check == false) throw new Exception("BCrypt: original string failed verification check against hash.");
            }
            return Retme;
        }

        private void CvtStringToKeyVect(string HashedString) {
            byte[] ArrayPullFrom = new byte[kSize + vSize];
            if(HashedString.Length < (kSize+vSize)){
                //prompt for longer password
                int NumToInit = ((kSize+ vSize)-HashedString.Length);
                for (int z = HashedString.Length - 1; NumToInit > 0; --NumToInit) {
                    ArrayPullFrom[HashedString.Length + NumToInit-1] = (byte)3;
                }
            }
            int HashCounter = 0;
            foreach (byte Value in ArrayPullFrom) {
                if (HashCounter == HashedString.Length) break;
                ArrayPullFrom[HashCounter] = (Encoding.ASCII.GetBytes(HashedString))[HashCounter];
                HashCounter++;
            }
            int kCount=0,vCount=0;
            foreach(byte kValue in myAES.key){ 
                myAES.key[kCount] = ArrayPullFrom[kCount];
                kCount++;
            }
            foreach(byte cValue in myAES.vector){ 
                myAES.vector[vCount] = ArrayPullFrom[kCount + vCount];
                vCount++;
            }
            myAES = new SimplerAES((SimplerAES.Algo)AlgorithmChoice, myAES.key, myAES.vector);
            InitArrayBoxes();
        }


        public Form1() {
            InitializeComponent();
            InitArrayBoxes();
            AlgoBox.SelectedIndex = 0;
            BCryptBox1.Checked = false;
        }

        private void InitArrayBoxes() {
            KeyArrayBox.Text = "";
            VectorArrayBox.Text = "";
            int x = 0, y = 0;
            foreach (int Num in myAES.key) {
                x++;
                KeyArrayBox.Text += Num + (x==kSize ? "" : ",");
            }
            foreach (int Num in myAES.vector) {
                y++;
                VectorArrayBox.Text += Num + (y==vSize ? "" : ",");
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
            dlg.Text = "Password must be longer than 16 chars";
            if (dlg.ShowDialog() == DialogResult.OK) {
                if (dlg.Password.Length > 16) {
                    Retme = dlg.Password;
                }
                else {
                    MessageBox.Show("Password must be longer than 16 chars");
                    dlg.ClearPass();
                    Retme = RetString(dlg);
                }
            }
            return Retme;
        }
        
        private string RetString(GetString dlg, int NeededLength) {
            string Retme = "";
            dlg.Text = "Password must be longer than "+NeededLength+" chars";
            if (dlg.ShowDialog() == DialogResult.OK) {
                if (dlg.Password.Length > NeededLength) {
                    Retme = dlg.Password;
                }
                else {
                    MessageBox.Show("Password must be longer than "+NeededLength+" chars");
                    dlg.ClearPass();
                    Retme = RetString(dlg);
                }
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
                    string pass = RetString(Dlg, 20);
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
            //Random SeedPart = new Random(DateTime.Now.Millisecond);
            //Random PRNG = new Random(DateTime.Now.Millisecond * SeedPart.Next(50));
            Entropy.Data SPRNG;
            if (Seed != 0) { SPRNG = new Entropy.Data(Seed); }
            else SPRNG = new Entropy.Data();
            myAES.key = new byte[kSize];
            myAES.vector = new byte[vSize];
            for (int rk = 0; rk < kSize; rk++) {
                myAES.key[rk] = SPRNG.Retme;
                //SPRNG = new Entropy.Data();
                //PRNG.NextBytes(myAES.key);
            }
            //PRNG = new Random(DateTime.Now.Millisecond * SeedPart.Next(50));
            for (int rv = 0; rv < vSize; rv++) {
                myAES.vector[rv] = SPRNG.Retme;
                //SPRNG = new Entropy.Data();
                //PRNG.NextBytes(myAES.vector);
            }
            myAES = new SimplerAES((AES256.SimplerAES.Algo)AlgorithmChoice, myAES.key, myAES.vector);
            myAES.UpDateArrays();
            InitArrayBoxes();
            CheckCounts();
        }

        private void KeyArrayChanged(object Sender, EventArgs e) {
            try {
                string[] Split = KeyArrayBox.Text.Split(",".ToArray());
                if (Split.Count() != kSize) KeyArrayBox.ForeColor = Color.Red;
                else KeyArrayBox.ForeColor = Color.Green;
                foreach (string Value in Split) {
                    if (Value.Length <= 0) goto breakpt;
                    if ((Value.Length > 3) ||
                        (Convert.ToInt32(Value) > Byte.MaxValue) ||
                        (Convert.ToInt32(Value) < Byte.MinValue)) KeyArrayBox.ForeColor = Color.Red;
                }
            breakpt: ;
            }
            catch (Exception ex){
                MessageBox.Show("Valid ranges for Array Values are 0-255\n" + ex.Message);
                InitArrayBoxes();
            }
        }

        private void VectorArrayChanged(object Sender, EventArgs e) {
            try {
                string[] Split = VectorArrayBox.Text.Split(",".ToArray());
                if (Split.Count() != vSize) VectorArrayBox.ForeColor = Color.Red;
                else VectorArrayBox.ForeColor = Color.Green;
                foreach (string Value in Split) {
                    if (Value.Length <= 0) goto breakout;
                    if ((Value.Length > 3) || 
                        (Convert.ToInt32(Value) > Byte.MaxValue) ||
                        (Convert.ToInt32(Value) < Byte.MinValue)) VectorArrayBox.ForeColor = Color.Red;
                }
            breakout: ;
            }
            catch (Exception ex) {
                MessageBox.Show("Valid ranges for Array Values are 0-255\n" + ex.Message);
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

        private void SeedBox_TextChanged(object sender, EventArgs e)
        {
            if (SeedBox.Text.Length > 0) {
                try {
                    Seed = Convert.ToInt32(SeedBox.Text);
                }
                catch (Exception ex) {
                    SeedBox.Text = "";
                    Seed = 0;
                }
            }
        }

        private void AlgoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlgorithmChoice = (Algo)AlgoBox.SelectedIndex;
            if (AlgorithmChoice == Algo.AES128CBC128) {
                kSize = 16; vSize = 16;
            }
            else if (AlgorithmChoice == Algo.AES256CBC128) {
                kSize = 32; vSize = 16;
            }
            else if (AlgorithmChoice == Algo.AES256CBC256) {
                kSize = 32; vSize = 32;
            }
            else if (AlgorithmChoice == Algo.AES256CFB128) {
                kSize = 32; vSize = 16;
            }
            randarrays_Click(sender, e);
        }

        private void BCryptBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (BCryptBox1.Checked == true) {
                AmUsingBcrypt = true;
            }
            else if (BCryptBox1.Checked == false) {
                AmUsingBcrypt = false;
            }

            if (AmUsingBcrypt == true) {
                CvtStringToKeyVect(PromptForBCryptHashGeneration());
            }
        }

        
    }
}
