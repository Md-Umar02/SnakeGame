using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
	public partial class SnakeGame : Form
	{
		private int snakeSize = 20;
		private int score = 0;
		private int direction = 0;
		private Point food = new Point();
		private Random rand = new Random();
		private List<Point> snake = new List<Point>();

		public SnakeGame()
		{
			InitializeComponent();
			InitializeGame();
		}

		private void InitializeGame()
		{
			this.KeyDown -= OnKeyDown;
			timer1.Tick -= Update;

			snake.Clear();
			snake.Add(new Point(0, 0)); 
			direction = 0; 
			score = 0; 
			GenerateFood();

			this.KeyDown += OnKeyDown;
			timer1.Tick += Update;
			timer1.Interval = 100;
			timer1.Start();
		}

		private void GenerateFood()
		{
			int maxX = (this.ClientSize.Width / snakeSize) - 1;
			int maxY = (this.ClientSize.Height / snakeSize) - 1;
			food = new Point(rand.Next(0, maxX), rand.Next(0, maxY));
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Right:
					if (direction != 2) direction = 0;
					break;
				case Keys.Down:
					if (direction != 3) direction = 1;
					break;
				case Keys.Left:
					if (direction != 0) direction = 2;
					break;
				case Keys.Up:
					if (direction != 1) direction = 3;
					break;
			}
		}

		private void Update(object sender, EventArgs e)
		{
			MoveSnake();
			CheckCollision();
			this.Invalidate(); 
		}

		private void MoveSnake()
		{
			Point head = snake[0];
			Point newHead = new Point(head.X, head.Y);

			switch (direction)
			{
				case 0:
					newHead.X++;
					break;
				case 1:
					newHead.Y++;
					break;
				case 2: 
					newHead.X--;
					break;
				case 3:
					newHead.Y--;
					break;
			}

			snake.Insert(0, newHead);

			if (newHead == food)
			{
				score++;
				GenerateFood();
			}
			else
			{
				snake.RemoveAt(snake.Count - 1);
			}
		}

		private void CheckCollision()
		{
			Point head = snake[0];

			if (head.X < 0 || head.Y < 0 || head.X >= this.ClientSize.Width / snakeSize || head.Y >= this.ClientSize.Height / snakeSize)
			{
				GameOver();
			}

			for (int i = 1; i < snake.Count; i++)
			{
				if (head == snake[i])
				{
					GameOver();
				}
			}
		}

		private void GameOver()
		{
			timer1.Stop();

			MessageBox.Show("Game Over! Your score: " + score);

			InitializeGame();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics g = e.Graphics;

			foreach (Point p in snake)
			{
				g.FillRectangle(Brushes.Green, p.X * snakeSize, p.Y * snakeSize, snakeSize, snakeSize);
			}

			g.FillRectangle(Brushes.Red, food.X * snakeSize, food.Y * snakeSize, snakeSize, snakeSize);
			label1.Text = "Score: " + score;
		}
	}
}