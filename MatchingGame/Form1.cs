using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        Random random = new Random();

        Label firstClicked = null;
        Label secondClicked = null;

        // Each of these letters is an interesting icon for each cards
        List<string> icons = new List<string>()
        {
            "s", "s", ".", ".", "2", "2", "7", "7",
            "y", "y", "~", "~", "x", "x", "8", "8"
        };

        /// Assign each icon from the list of icons to a random square
        private void AssignIconsToSquares()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                // tableLayoutPanel1.Controlsには16のラベルコントロールが入っている
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
            AssignIconsToSquares();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            firstClicked = null;
            secondClicked = null;
        }
        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }
            // アイコン色と背景色が一致したものがない=全てのアイコンが表示されている = ゲームクリア
            MessageBox.Show("You matched all the icons!", "Congratulations");
            Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // timerが動いている = 2枚めくったあとの暗記時間 = クリックは無視
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                // 色が黒 = openされている
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }

                // timer isn't running かつ firstClicked isn't null
                // のときにここに到達する.
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                // クリア判定
                CheckForWinner();

                // 2つのカードが一致 = カードはめくられたまま
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked.ForeColor = Color.Azure;
                    secondClicked.ForeColor = Color.Azure;
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }


                // 2つの異なるカードがめくられたので、3/4秒間表示する(タイマースタート)
                timer1.Start(); // timer1_Tick()が実行される
            }
        }
    }
}
