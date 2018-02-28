namespace ConwaysGameOfLife
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class ConwaysGame : Game
    {
        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;

        Input input;

        Pitch pitch;

        public ConwaysGame() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.graphics.PreferredBackBufferWidth = Properties.Settings.Default.Width;
            this.graphics.PreferredBackBufferHeight = Properties.Settings.Default.Height;
            if (Properties.Settings.Default.FullScreen)
            {
                this.graphics.ToggleFullScreen();
            }
        }

        protected override void Initialize()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.input = new Input(this);
            this.pitch = new Pitch(this, this.spriteBatch, this.input);
            this.input.UpdateOrder = 1;
            this.pitch.UpdateOrder = 2;
            this.Components.Add(this.input);
            this.Components.Add(this.pitch);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                base.Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}