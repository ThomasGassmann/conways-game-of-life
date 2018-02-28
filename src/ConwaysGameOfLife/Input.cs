namespace ConwaysGameOfLife
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

    public class Input : GameComponent
    {
        public Input(Game game): base(game)
        {
        }

        KeyboardState oldState;

        public override void Update(GameTime gameTime)
        {
            var currentState = Keyboard.GetState();
            this.SpaceTrigger = false;
            this.ResetTrigger = false;
            this.Figure1Trigger = false;
            this.Figure2Trigger = false;
            if (currentState.IsKeyDown(Keys.Space) && !this.oldState.IsKeyDown(Keys.Space))
            {
                this.SpaceTrigger = true;
            }

            if (currentState.IsKeyDown(Keys.R) && !this.oldState.IsKeyDown(Keys.R))
            {
                this.ResetTrigger = true;
            }

            if (currentState.IsKeyDown(Keys.F) && !this.oldState.IsKeyDown(Keys.F))
            {
                this.Figure1Trigger = true;
            }

            if (currentState.IsKeyDown(Keys.G) && !this.oldState.IsKeyDown(Keys.G))
            {
                this.Figure2Trigger = true;
            }

            this.oldState = currentState;
        }

        public bool SpaceTrigger { get; private set; }

        public bool ResetTrigger { get; private set; }

        public bool Figure1Trigger { get; private set; }

        public bool Figure2Trigger { get; private set; }
    }
}
