﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SharpLesson1
{
    class Game
    {
        private static readonly int ITER_NUM = 20;
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics buffer;

        public static List<BaseObject> _objs;

        public static int Height { get; set; }
        public static int Width { get; set; }

        static Game()
        {

        }

        /// <summary>
        /// Загрузка объектов
        /// </summary>
        private static void Load()
        {
            _objs = new List<BaseObject>();
            Random rnd = new Random();
            int size = 0;

            for (int i = 0; i < ITER_NUM; i++)
            {
                size = rnd.Next(Asteroid.minSize, Asteroid.maxSize + 1);
                //астероиды
                _objs.Add(new Asteroid(new Point(Width, rnd.Next(0, Height + 1)), new Point(rnd.Next(Asteroid.minSpeed, Asteroid.maxSpeed), 0), new Size(size, size)));
                //звезды
                _objs.Add(new Star(new Point(Width, rnd.Next(0, Height + 1)), new Point(rnd.Next(Star.minSpeed, Star.maxSpeed), 0), new Size(Star.starSize, Star.starSize)));
                //корабли
                size = rnd.Next(Ship.minSize, Ship.maxSize + 1);
                _objs.Add(new Ship(new Point(Width, rnd.Next(0, Height + 1)), new Point(rnd.Next(Ship.minSpeed, Ship.maxSpeed), 0), new Size(size, size)));
                //пули
                _objs.Add(new Bullet(new Point(0, rnd.Next(0, Height + 1)), new Point(rnd.Next(Bullet.minSpeed, Bullet.maxSpeed), 0), new Size(Bullet.bulletSize, Bullet.bulletSize)));
            }
        }

        /// <summary>
        /// Инициализация игровых настроек и запуск таймера обновления игровых переменных
        /// </summary>
        /// <param name="form">Форма, на которой будет отрисовываться игра</param>
        public static void Init(Form form)
        {
            // Графическое устройство для вывода графики            
            Graphics g;

            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;

            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            Load();

            Timer timer = new Timer { Interval = 100 };
            timer.Start();
            timer.Tick += Timer_Tick;
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        /// <summary>
        /// Отрисовка объектов
        /// </summary>
        public static void Draw()
        {
            //buffer.Graphics.Clear(Color.Black);
            //buffer.Graphics.DrawRectangle(Pens.White, new Rectangle(100, 100, 200, 200));
            //buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(100, 100, 200, 200));
            //buffer.Render();

            //отрисовка массива объектов
            buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            buffer.Render();
        }

        /// <summary>
        /// Обновление положения игровых объектов в пространстве
        /// </summary>
        public static void Update()
        {
            foreach (BaseObject obj in _objs)
                obj.Update();
        }
    }
}
