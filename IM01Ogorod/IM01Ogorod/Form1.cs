using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace IM01Ogorod
{
    public partial class Form1 : Form
    {
        Dictionary<CheckBox, Cell> field = new Dictionary<CheckBox, Cell>();
        Budget budget = new Budget();
        int day = 0;

        public Form1()
        {
            InitializeComponent();

            foreach (CheckBox cb in tableLayoutPanel1.Controls) field.Add(cb, new Cell());

        }
        private void UpdateBox(CheckBox cb)
        {
            Color c = Color.White;
            switch (field[cb].state)
            {
                case CellState.Empty:

                    c = Color.White;
                    break;
                case CellState.Growing:
                    c = Color.Black;
                    break;
                case CellState.Green:
                    c = Color.Green;
                    break;
                case CellState.Yellow:
                    c = Color.Gold;
                    break;
                case CellState.Red:
                    c = Color.Red;
                    break;
                case CellState.Overgrow:
                    c = Color.Brown;
                    break;
            }
            cb.BackColor = c;
        }

        private void Cut(CheckBox cb)
        {
            cb.ForeColor = Color.Black;
            budget.CalculatePrice(field[cb].state);
            field[cb].Cut();
            UpdateBox(cb);
        }

        private void StartGrow(CheckBox cb)
        {
            cb.ForeColor = Color.White;
            budget.CalculatePrice(field[cb].state);
            field[cb].StartGrow();
            UpdateBox(cb);
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (sender as CheckBox);
            if (budget.money >= 1)
            {
                if (cb.Checked) StartGrow(cb);
                else Cut(cb);
            }

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            foreach (CheckBox cb in tableLayoutPanel1.Controls)
            {
                field[cb].Step();
                UpdateBox(cb);
            }

            day++;
            if (budget.money <= 1) textBox2.Text = " Not enough money";
            else textBox2.Text = "Money: " + budget.money;
            textBox1.Text = "Day: " + day;

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e){}
        private void textBox1_TextChanged(object sender, EventArgs e){}
        private void textBox2_TextChanged(object sender, EventArgs e){}
        private void button1_Click(object sender, EventArgs e)
        { 
            if (timer1.Interval <= 25) timer1.Interval = 1;
            else timer1.Interval -= 25; 
        }
        private void button2_Click(object sender, EventArgs e)
        {  
            timer1.Interval += 25; 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (CheckBox cb in tableLayoutPanel1.Controls)
            {
                field[cb].progress = 0;
                
            }
            budget.money = 100;
            day = 0;

        }
    }
    enum CellState
    {
        Empty,
        Growing,
        Green,
        Yellow,
        Red,
        Overgrow
    }

    class Budget
    {
        public int money = 100;

        Dictionary<CellState, int> price = new Dictionary<CellState, int>
        {
            [CellState.Empty] = -2,
            [CellState.Growing] = 0,
            [CellState.Green] = 0,
            [CellState.Yellow] = 3,
            [CellState.Red] = 5,
            [CellState.Overgrow] = -1
        };
        internal void CalculatePrice(CellState state)
        {
            money += price[state];
        }
    }
    class Cell
    {
        const int prGrowing = 20;
        const int prGreen = 60;
        const int prYellow = 80;
        const int prRed = 100;

        public int progress = 0;
        public CellState state
        {
            get
            {
                if (progress == 0) return CellState.Empty;
                if (progress < prGrowing) return CellState.Growing;
                else if (progress < prGreen) return CellState.Green;
                else if (progress < prYellow) return CellState.Yellow;
                else if (progress < prRed) return CellState.Red;
                else return CellState.Overgrow;
            }
        }

        internal void StartGrow()
        {
            progress++;
        }

        internal void Cut()
        {
            progress = 0;

        }
        internal void Step()
        {
            if ((state != CellState.Overgrow) && (state != CellState.Empty))
                progress++;
        }

    }
}
