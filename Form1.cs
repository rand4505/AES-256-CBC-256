/*Programmer: John Grubbs
 *Program: Super Simple AES-256-CBC-128 Crypted String/File generator
 *Final Update: 10 OCT 2013
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
using System.Security.Cryptography;
using jp.takel.PseudoRandom;

namespace AES256
{
    public partial class Form1 : Form
    {
        private MersenneTwister MT = new MersenneTwister();
        private int Seed = 0;
        private bool AmUsingBcrypt = false;
        private bool AmUsingPassword = false;
        public enum Algo { AES256CBC256, AES256CBC128, AES256CFB128, AES128CBC128, ECDH512, ECDH384, ECDH256};
        private string[] AlgoArrayComp = { "256-AES-256-CBC", "256-AES-128-CBC(Nist Rijndael)", "256-AES-128-CFB", "128-AES-128-CBC", "ECDH 512", "ECDH 384", "ECDH 256" };
        private Algo AlgorithmChoice = new Algo();
        private string Mode = new string(new char[50]);
        private SimplerAES myAES = new SimplerAES();
        private int _KeyCount { get { return myAES.key.Count(); } set { myAES.key.Count(); } }
        private int _VectCount { get { return myAES.vector.Count(); } set { myAES.vector.Count(); } }
        private int kSize = 32;
        private int vSize = 16;
        
        private bool CheckCounts() {
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

        protected string PromptForBCryptHashGeneration() {
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

        protected void CvtStringToKeyVect(string HashedString) {
            byte[] ArrayPullFrom = new byte[kSize + vSize];
            if(HashedString.Length < (kSize+vSize)) {
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
            foreach(byte kValue in myAES.key) { 
                myAES.key[kCount] = ArrayPullFrom[kCount];
                kCount++;
            }
            foreach(byte cValue in myAES.vector) { 
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

        public void CallGC() {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForPendingFinalizers();
        }

        ~Form1() {
            RTB.Clear();
            InputBox.Text.Remove(0);
            VectorArrayBox.Text.Remove(0);
            KeyArrayBox.Text.Remove(0);
            CallGC();
            CallGC();
        }

        protected void InitArrayBoxes() {
            KeyArrayBox.Text = "";
            VectorArrayBox.Text = "";
            int x = 0, y = 0;
            foreach (int Num in myAES.key) {
                x++;
                KeyArrayBox.Text += Num + (x==kSize ? "" : ",");
            }
            x = 0;
            foreach (int Num in myAES.vector) {
                y++;
                VectorArrayBox.Text += Num + (y==vSize ? "" : ",");
            }
            y = 0;
            CheckCounts();
        }

        protected string GetFileName(OpenFileDialog Dlg) {
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

        protected string RetString(GetString dlg) {
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
        
        protected string RetString(GetString dlg, int NeededLength) {
            string Retme = "";
            dlg.Text = "Password must be longer than " + NeededLength + " chars";
            if (dlg.ShowDialog() == DialogResult.OK) {
                if (dlg.Password.Length > NeededLength) {
                    Retme = dlg.Password;
                }
                else {
                    MessageBox.Show("Password must be longer than " + NeededLength + " chars");
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
                    try {
                        RTB.AppendText(Mode + (FileCrypto.FileOps(file, Mode, pass) ? " success" : " failed"));
                    }
                    catch (Exception Error) {
                        RTB.AppendText("Error duing file decryption, stack dump as follows:\n" + Error.Message);
                    }
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

        private void UpDateKeyAr_Click(object sender, EventArgs e) {
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
            Size = 0; Pos = 0;
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
            Size = 0; Pos = 0;
        }

        protected void randarrays_Click(object sender, EventArgs e) {
            //System.Security.Cryptography.RNGCryptoServiceProvider CSRND;
            //if (Seed != 0) { CSRND =  new RNGCryptoServiceProvider(Seed.ToString()); }
            //else CSRND = new RNGCryptoServiceProvider((Environment.TickCount.ToString()));
            if (Seed != 0) { MT = new MersenneTwister((uint)Seed); }
            else { MT = new MersenneTwister((uint)MT.Next()); }

            byte[] newkey = new byte[kSize];
            byte[] newvect = new byte[vSize];
            byte[] Singlton = new byte[1];
            MT.NextBytes(newkey);
            //for (int rk = 0; rk < kSize; rk++) {
                //was CSRND
            //    MT.NextBytes(Singlton);
            //    myAES.key[rk] = Singlton[0];
            //}
            MT.NextBytes(newvect);
            //for (int rv = 0; rv < vSize; rv++) {
                //was CSRND
            //    MT.NextBytes(Singlton);
            //    myAES.vector[rv] = Singlton[0];
            //}
            retry1:
            myAES = new SimplerAES((AES256.SimplerAES.Algo)AlgorithmChoice, newkey, newvect);
            try { myAES.UpDateArrays(); }
            catch (CryptographicException ex) { goto retry1; }
            InitArrayBoxes();
            CheckCounts();
            Singlton[0] = (byte)0;
        }

        protected void KeyArrayChanged(object Sender, EventArgs e) {
            try {
                string[] Split = KeyArrayBox.Text.Split(",".ToArray());
                if (Split.Count() != kSize) KeyArrayBox.ForeColor = Color.Red;
                else KeyArrayBox.ForeColor = Color.Green;
                foreach (string Value in Split) {
                    if (Value.Length <= 0) { 
                        KeyArrayBox.ForeColor = Color.Red;
                        goto breakpt; 
                    }
                    if ((Value.Length > 3) ||
                        (Convert.ToInt32(Value) > Byte.MaxValue) ||
                        (Convert.ToInt32(Value) < Byte.MinValue)) KeyArrayBox.ForeColor = Color.Red;
                }
                breakpt: ;
                Split = null;
            }
            catch (Exception ex) {
                MessageBox.Show("Valid ranges for Array Values are 0-255\n" + ex.Message);
                InitArrayBoxes();
            }
        }

        protected void VectorArrayChanged(object Sender, EventArgs e) {
            try {
                string[] Split = VectorArrayBox.Text.Split(",".ToArray());
                if (Split.Count() != vSize) VectorArrayBox.ForeColor = Color.Red;
                else VectorArrayBox.ForeColor = Color.Green;
                foreach (string Value in Split) {
                    if (Value.Length <= 0) { 
                        VectorArrayBox.ForeColor = Color.Red; 
                        goto breakout; 
                    }
                    if ((Value.Length > 3) || 
                        (Convert.ToInt32(Value) > Byte.MaxValue) ||
                        (Convert.ToInt32(Value) < Byte.MinValue)) VectorArrayBox.ForeColor = Color.Red;
                }
                breakout: ;
                Split = null;
            }
            catch (Exception ex) {
                MessageBox.Show("Valid ranges for Array Values are 0-255\n" + ex.Message);
                InitArrayBoxes();
            }
        }

        private void FileMode_checkBox_CheckedChanged(object sender, EventArgs e) {
            if (FileMode_checkBox.Checked == true) {
                EncryptButton.Text = "File Encrypt";
                DecryptButton.Text = "File Decrypt";
            }
            else {
                EncryptButton.Text = "Encrypt";
                DecryptButton.Text = "Decrypt";
            }
        }

        private void SeedBox_TextChanged(object sender, EventArgs e) {
            if (SeedBox.Text.Length > 0) {
                try {
                    Seed = Convert.ToInt32(SeedBox.Text);
                }
                catch (Exception ex) {
                    SeedBox.Text = "0";
                    Seed = 0;
                }
            }
        }

        private void AlgoBox_SelectedIndexChanged(object sender, EventArgs e) {
            AlgorithmChoice = (Algo)AlgoBox.SelectedIndex;
            if (AlgorithmChoice == Algo.AES128CBC128) {
                kSize = 16; vSize = 16;
                randarrays_Click(sender, e);
            }
            else if (AlgorithmChoice == Algo.AES256CBC128) {
                kSize = 32; vSize = 16;
                randarrays_Click(sender, e);
            }
            else if (AlgorithmChoice == Algo.AES256CBC256) {
                kSize = 32; vSize = 32;
                randarrays_Click(sender, e);
            }
            else if (AlgorithmChoice == Algo.AES256CFB128) {
                kSize = 32; vSize = 16;
                randarrays_Click(sender, e);
            }
            /*else if (AlgorithmChoice == Algo.ECDH512) {       //the below three are busted for XP, intentional gimping by MS, only work for Vista and up
                myAES = new SimplerAES((AES256.SimplerAES.Algo)Algo.ECDH512, true);
            }
            else if (AlgorithmChoice == Algo.ECDH384) {
                myAES = new SimplerAES((AES256.SimplerAES.Algo)Algo.ECDH384, true);
            }
            else if (AlgorithmChoice == Algo.ECDH256) {
                myAES = new SimplerAES((AES256.SimplerAES.Algo)Algo.ECDH256, true);
            }*/
            InitArrayBoxes();
            CheckCounts();
        }

        protected void BCryptBox1_CheckedChanged(object sender, EventArgs e) {
            if (BCryptBox1.Checked == true) {
                AmUsingBcrypt = true;
                AmUsingPassword = false; Password.Checked = false;
            }
            else if (BCryptBox1.Checked == false) {
                AmUsingBcrypt = false;
            }

            if (AmUsingBcrypt == true) {
                CvtStringToKeyVect(PromptForBCryptHashGeneration());
            }
        }

        protected void UsePasswordInsteadOfKeys() {
            try {
                string password = "";
                int pksize = 0, pvsize = 0;
                GetString dlg = new GetString();
                password = RetString(dlg, 20);
                myAES = new SimplerAES((SimplerAES.Algo)AlgorithmChoice);
                pksize = (myAES.kSize) / 8;
                pvsize = (myAES.vSize) / 8;
                var key = new Rfc2898DeriveBytes(password, Encoding.ASCII.GetBytes(myAES.CallSalt));
                myAES.key = key.GetBytes(pksize);
                myAES.vector = key.GetBytes(pvsize);
                myAES = new SimplerAES((SimplerAES.Algo)AlgorithmChoice, myAES.key, myAES.vector);
                InitArrayBoxes();
                password.Remove(0);
                pksize = 0; pvsize = 0;
            }
            catch(Exception Xedout){ }
        }

        private void Password_CheckedChanged(object sender, EventArgs e) {
            if (Password.Checked == true) {
                AmUsingPassword = true;
                AmUsingBcrypt = false; BCryptBox1.Checked = false;
                SeedBox.Enabled = false;
                Seedlabel.Enabled = false;
                BCryptBox1.Enabled = false;
                UpDateKeyAr.Enabled = false;
                VectorUpdateBtn.Enabled = false;
                randarrays.Enabled = false;
            }
            else if (Password.Checked == false) {
                AmUsingPassword = false;
                SeedBox.Enabled = true;
                Seedlabel.Enabled = true;
                BCryptBox1.Enabled = true;
                UpDateKeyAr.Enabled = true;
                VectorUpdateBtn.Enabled = true;
                randarrays.Enabled = true;
            }

            if (AmUsingPassword == true) {
                UsePasswordInsteadOfKeys();
            }
        }
    }
}
