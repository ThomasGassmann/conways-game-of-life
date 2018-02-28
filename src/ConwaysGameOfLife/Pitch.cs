namespace ConwaysGameOfLife
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Pitch : DrawableGameComponent
    {
        public int PITCH_WIDTH
        {
            get
            {
                return this.Pitch_Width;
            }
        }

        public int PITCH_HEIGHT
        {
            get
            {
                return this.Pitch_Heigth;
            }
        }

        public const int UPDATE_INTERVAL = 33;

        bool[, ,] cells;

        Rectangle[,] rects;

        SpriteBatch spriteBatch;

        Texture2D livingCellTexture;

        Input input;

        int Pitch_Heigth = 100;

        int Pitch_Width = 100;

        public Pitch(Game game, SpriteBatch spriteBatch, Input input) : base(game)
        {
            this.Pitch_Heigth = Properties.Settings.Default.PitchHeight;
            this.Pitch_Width = Properties.Settings.Default.PitchWidth;
            cells = new bool[2, this.PITCH_WIDTH + 2, this.PITCH_HEIGHT + 2];
            this.rects = new Rectangle[this.PITCH_WIDTH, this.PITCH_HEIGHT];
            this.spriteBatch = spriteBatch;
            this.input = input;
        }

        protected override void LoadContent()
        {
            livingCellTexture = new Texture2D(this.GraphicsDevice, 1, 1);
            var colors = new Color[1];
            colors[0] = Color.White;
            livingCellTexture.SetData(0, new Rectangle(0, 0, 1, 1), colors, 0, 1);
            base.LoadContent();
        }

        int milliSecondsSinceLastUpdate = 0;
        
        int currentIndex = 0, futureIndex = 1;

        int oldWidth, oldHeight;

        public override void Update(GameTime gameTime)
        {
            if (this.input.SpaceTrigger)
            {
                this.CreateRandomCells(28);
            }

            if (this.input.Figure1Trigger)
            {
                this.Figure1();
            }

            if (this.input.ResetTrigger)
            {
                this.ResetCells();
            }

            if (this.input.Figure2Trigger)
            {
                this.Figure2();
            }

            this.milliSecondsSinceLastUpdate += gameTime.ElapsedGameTime.Milliseconds;
            if (this.milliSecondsSinceLastUpdate >= Pitch.UPDATE_INTERVAL)
            {
                this.milliSecondsSinceLastUpdate = 0;
                for (int y = 1; y < this.PITCH_HEIGHT; y++)
                {
                    for (int x = 1; x < this.PITCH_WIDTH; x++)
                    {
                        var neighBoursCount = 0;
                        if (this.cells[currentIndex, x - 1, y])
                        {
                            neighBoursCount++;
                        }
                        
                        if (this.cells[currentIndex, x + 1, y])
                        {
                            neighBoursCount++;
                        }

                        if (this.cells[currentIndex, x, y - 1])
                        {
                            neighBoursCount++;
                        }

                        if (this.cells[currentIndex, x, y + 1])
                        {

                        }

                        if (this.cells[currentIndex, x - 1, y - 1])
                        {
                            neighBoursCount++;
                        }

                        if (this.cells[currentIndex, x - 1, y + 1])
                        {
                            neighBoursCount++;
                        }

                        if (this.cells[currentIndex, x + 1, y + 1])
                        {
                            neighBoursCount++;
                        }

                        if (this.cells[currentIndex, x + 1, y - 1])
                        {
                            neighBoursCount++;
                        }

                        if (neighBoursCount == 3)
                        {
                            this.cells[futureIndex, x, y] = true;
                        }
                        else if (neighBoursCount < 2)
                        {
                            this.cells[futureIndex, x, y] = false;
                        }
                        else if (neighBoursCount > 3)
                        {
                            this.cells[futureIndex, x, y] = false;
                        }
                        else if (neighBoursCount == 2 && this.cells[currentIndex, x, y])
                        {
                            this.cells[futureIndex, x, y] = true;
                        }
                    }
                }

                if (this.currentIndex == 0)
                {
                    this.currentIndex = 1;
                    this.futureIndex = 0;
                }
                else
                {
                    this.currentIndex = 0;
                    this.futureIndex = 1;
                }
            }

            var width = this.GraphicsDevice.Viewport.Width;
            var height = this.GraphicsDevice.Viewport.Height;
            if (this.oldWidth != width || this.oldHeight != height)
            {
                var cellWidth = width / this.PITCH_WIDTH;
                var cellHeight = height / this.PITCH_HEIGHT;
                var cellSize = Math.Min(cellWidth, cellHeight);
                var offsetX = (width - (cellSize * this.PITCH_WIDTH)) / 2;
                var offsetY = (height - (cellSize * this.PITCH_HEIGHT)) / 2;
                for (int y = 0; y < this.PITCH_HEIGHT; y++)
                {
                    for (int x = 0; x < this.PITCH_HEIGHT; x++)
                    {
                        this.rects[x, y] = new Rectangle(offsetX + x * cellSize, offsetY + y * cellSize, cellSize, cellSize);
                    }
                }

                this.oldHeight = height;
                this.oldWidth = width;
            }

            base.Update(gameTime);
        }

        private void Figure2()
        {
            var r = new Random();
            for (int y = 1; y < this.PITCH_HEIGHT + 1; y++)
            {
                for (int x = 1; x < this.PITCH_WIDTH + 1; x++)
                {
                    if (r.Next(-1, 1) == 0)
                    {
                        this.cells[currentIndex, x, y] = false;
                    }
                    else
                    {
                        this.cells[currentIndex, x, y] = true;
                    }
                }
            }
        }

        private void Figure1()
        {
            for (int y = 1; y < this.PITCH_HEIGHT + 1; y++)
            {
                for (int x = 1; x < this.PITCH_WIDTH + 1; x++)
                {
                    if (x == y || this.PITCH_WIDTH - x == y)
                    {
                        this.cells[currentIndex, x, y] = true;
                    }
                }
            }
        }

        private void ResetCells()
        {
            for (int y = 1; y < this.PITCH_HEIGHT + 1; y++)
            {
                for (int x = 1; x < this.PITCH_WIDTH + 1; x++)
                {
                    this.cells[currentIndex, x, y] = false;
                }
            }
        }

        private void CreateRandomCells(int probability)
        {
            var r = new Random();
            for (int x = 1; x < this.PITCH_WIDTH + 1; x++)
            {
                for (int y = 1; y < this.PITCH_HEIGHT + 1; y++)
                {
                    if (r.Next(0, probability) == 0)
                    {
                        this.cells[currentIndex, x, y] = true;
                    }
                    else
                    {
                        this.cells[currentIndex, x, y] = false;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            for (int y = 1; y < this.PITCH_HEIGHT + 1; y++)
            {
                for (int x = 1; x < this.PITCH_WIDTH + 1; x++)
                {
                    if (this.cells[currentIndex, x, y])
                    {
                        this.spriteBatch.Draw(this.livingCellTexture, this.rects[x - 1, y - 1], Color.White);
                    }
                }
            }

            this.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}