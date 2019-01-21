using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EmguCVSandbox.Form1;
using static EmguCVSandbox.ObjectsStructure;

namespace EmguCVSandbox
{
    public partial class OnScreenDisplay : Form
    {
        private readonly List<CardInfo> cards;
        private readonly List<MobInfo> mobsOnBattlefield;
        private readonly List<QuestInfo> questOnBattlefield;
        private readonly List<HeroAllyInfo> HeroesAllyOnBattlefield;
        private readonly List<MyAllyInfo> AllyOnBattlefield;
        private readonly CheckBox formCheckbox;
        private readonly DataGridView gridSets;
        private readonly Point windowLocation;
        int screenW = 0;
        int screenH = 0;

        public OnScreenDisplay(List<MyAllyInfo> AllyOnBattlefield, List<CardInfo> cards,List<MobInfo> mobsOnBattlefield, List<QuestInfo> questOnBattlefield, List<HeroAllyInfo> heroesAllyOnBattlefield,CheckBox formCheckbox,  Point windowLocation)
        {
            InitializeComponent();
            this.TopMost = true; 
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; 
            //this.WindowState = FormWindowState.Maximized; 
            this.MinimizeBox = this.MaximizeBox = false;
            this.MinimumSize = this.MaximumSize = this.Size; 
            this.TransparencyKey = this.BackColor = Color.Red;

            

            //int initialStyle = GetWindowLong(this.Handle, -20);
            //SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
            formCheckbox.CheckedChanged += new EventHandler(formCheck_checkChanged);
            this.cards = cards;
            this.mobsOnBattlefield = mobsOnBattlefield;
            this.questOnBattlefield = questOnBattlefield;
            HeroesAllyOnBattlefield = heroesAllyOnBattlefield;
            this.AllyOnBattlefield = AllyOnBattlefield;
            this.formCheckbox = formCheckbox;
            this.windowLocation = windowLocation;

            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;


        }


        private void formCheck_checkChanged(object sender, EventArgs e)
        {
            if (!formCheckbox.Checked)
            {
                this.Close();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // Set the form click-through
                cp.ExStyle |= 0x80000 /* WS_EX_LAYERED */ | 0x20 /* WS_EX_TRANSPARENT */;
                return cp;
            }
        }


        

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //mobs
            int mobsXOff = windowLocation.X + 260;
            int mobsYOff = windowLocation.Y + 350-120;
            int heroXOffset = windowLocation.X + 350;
            int heroYOffset = windowLocation.Y + 550+120;
            int handXOffset = windowLocation.X + 410;
            int handYOffset = windowLocation.Y + 840;
            
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            e.Graphics.DrawString(this.Width + "x" + this.Height, this.Font, Brushes.Red, new PointF(this.Width / 2, this.Height / 2));
            ////e.Graphics.DrawString(this.Width + "x" + this.Height, this.Font, b, new Point(this.Width / 2, this.Height / 2));

            SolidBrush monsterInfoColor = new SolidBrush(Color.Pink);
            foreach (var mob in mobsOnBattlefield)
            {
                string active = mob.active ? "active" : "exaused";
                e.Graphics.FillEllipse(monsterInfoColor, mob.location.X - 3 + mobsXOff, mob.location.Y - 3 + mobsYOff, 6, 6);
                e.Graphics.DrawString(mob.name, this.Font, monsterInfoColor, new Point(mob.location.X - 30 + mobsXOff, mob.location.Y + 5 + mobsYOff));
                e.Graphics.DrawString(active, this.Font, monsterInfoColor, new Point(mob.location.X - 30 + mobsXOff, mob.location.Y + 25 + mobsYOff));
                e.Graphics.DrawString("Att:" + mob.attack, this.Font, monsterInfoColor, new Point(mob.location.X - 30 + mobsXOff, mob.location.Y + 45 + mobsYOff));
                e.Graphics.DrawString("HP:" + mob.hp, this.Font, monsterInfoColor, new Point(mob.location.X - 30 + mobsXOff, mob.location.Y + 65 + mobsYOff));
            }
            //quests
            SolidBrush questInfoBrush = new SolidBrush(Color.Yellow);
            foreach (var quest in questOnBattlefield)
            {
                e.Graphics.FillEllipse(questInfoBrush, quest.location.X - 3 + mobsXOff, quest.location.Y - 3 + mobsYOff, 6, 6);
                e.Graphics.DrawString(quest.name, this.Font, questInfoBrush, new Point(quest.location.X - 30 + mobsXOff, quest.location.Y + 5 + mobsYOff));
                e.Graphics.DrawString("Value:" + quest.value, this.Font, questInfoBrush, new Point(quest.location.X - 30 + mobsXOff, quest.location.Y + 25 + mobsYOff));

            }
            //cards
            SolidBrush cardsInfoBrush = new SolidBrush(Color.Violet);
            foreach (var card in cards)
            {
                e.Graphics.DrawEllipse(new Pen(Color.Violet), card.location.X - 20 + handXOffset, card.location.Y - 20 + handYOffset, 40, 40);
                e.Graphics.DrawString(card.value, this.Font, cardsInfoBrush, new Point(card.location.X - 30 + handXOffset, card.location.Y + 5 + handYOffset));
            }
            //heroes
            SolidBrush heroInfoBrush = new SolidBrush(Color.LightGreen);
            foreach (var hero in HeroesAllyOnBattlefield)
            {
                string active = hero.active ? "active" : "exaused";
                e.Graphics.FillEllipse(heroInfoBrush, hero.location.X - 3 + heroXOffset, hero.location.Y - 3 + heroYOffset, 6, 6);
                e.Graphics.DrawString(hero.name, this.Font, heroInfoBrush, new Point(hero.location.X - 30 + heroXOffset, hero.location.Y + 5 + heroYOffset));
                e.Graphics.DrawString(active, this.Font, heroInfoBrush, new Point(hero.location.X - 30 + heroXOffset, hero.location.Y + 25 + heroYOffset));
                e.Graphics.DrawString("Att:" + hero.attack, this.Font, heroInfoBrush, new Point(hero.location.X - 30 + heroXOffset, hero.location.Y + 45 + heroYOffset));
                e.Graphics.DrawString("HP:" + hero.hp, this.Font, heroInfoBrush, new Point(hero.location.X - 30 + heroXOffset, hero.location.Y + 65 + heroYOffset));
                e.Graphics.DrawString("Lore:" + hero.lore, this.Font, heroInfoBrush, new Point(hero.location.X - 30 + heroXOffset, hero.location.Y + 85 + heroYOffset));
            }

            //allies
            SolidBrush allyInfoBrush = new SolidBrush(Color.LightBlue);
            foreach (var ally in AllyOnBattlefield)
            {
                string active = ally.active ? "active" : "exaused";
                e.Graphics.FillEllipse(allyInfoBrush, ally.location.X - 3 + heroXOffset, ally.location.Y - 3 + heroYOffset, 6, 6);
                e.Graphics.DrawString(ally.name, this.Font, allyInfoBrush, new Point(ally.location.X - 30 + heroXOffset, ally.location.Y + 5 + heroYOffset));
                e.Graphics.DrawString(active, this.Font, allyInfoBrush, new Point(ally.location.X - 30 + heroXOffset, ally.location.Y + 25 + heroYOffset));
                e.Graphics.DrawString("Att:" + ally.attack, this.Font, allyInfoBrush, new Point(ally.location.X - 30 + heroXOffset, ally.location.Y + 45 + heroYOffset));
                e.Graphics.DrawString("HP:" + ally.hp, this.Font, allyInfoBrush, new Point(ally.location.X - 30 + heroXOffset, ally.location.Y + 65 + heroYOffset));
                e.Graphics.DrawString("Lore:" + ally.lore, this.Font, allyInfoBrush, new Point(ally.location.X - 30 + heroXOffset, ally.location.Y + 85 + heroYOffset));
            }


        }

        private void OnScreenDisplay_Load(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
