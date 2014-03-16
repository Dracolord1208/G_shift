
// T-REX REVENGE!!
// pre-Alpha 1a
// with newly added particle effect

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

//#include<ParticleEngine>

namespace T_Rex_Revenge
{

    public class Interactable
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public Vector2 position { get; set; }
        public Vector2 motion { get; set; }
        public Rectangle rect
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y,
                    (int)Width, (int)Height);
            }
        }
    }

    public class Rex : Interactable { }
    //public class Gun : Interactable { }
    public class Gun : Interactable { }
    public class Enemy : Interactable { }
    public class Bullet : Interactable { }
    //public class ParticleEngine { List<Texture2D> textures, Vector2 location }   // <-- i shouldn't have to do this
    //public class theParticalEngine : ParticleEngineB { }
    //public class primaryParticle : ParticleB { }


    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private float fps;
        private float updateInterval = 1.0f;
        private float timeSinceLastUpdate = 0.0f;
        private float framecount = 0;
        
        Rex theBoss;
        Gun miniGun;
        Enemy enemy1;
        Bullet blueBullet;

        Texture2D RexTexture;
        Texture2D GunTexture;
        Texture2D EnemyTexture;
        Texture2D BulletTexture;

        SoundEffect hitWallSound;
        SoundEffect hitPaddleSound;

        //const int SCREEN_WIDTH = 640;
        //const int SCREEN_HEIGHT = 480;
        const int SCREEN_WIDTH = 1000;
        const int SCREEN_HEIGHT = 600;

        private float gunAngle; // = 15f;
        private Vector2 origin; // = new Vector2(50, -100);
        float previousMousePosY = 0;

        private Texture2D background;

        ParticleEngineB mainWeapon;  // !!!*********************
        List<Texture2D> textures; // = new List<Texture2D>();
        //Particle xs;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //Content.RootDirectory = "Images";

            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true; // default rate of 1/60 sec
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // Setup window dimensions.
            // We could also used defaults and query
            ///*
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferMultiSampling = false;
            graphics.ApplyChanges();
            //*/

            // Initialize Rex
            theBoss = new Rex();
            theBoss.position = new Vector2(50, 250);
            theBoss.motion = new Vector2(10f, 1f);
            theBoss.Width = 500;
            theBoss.Height = 300;

            // Initialize Gun
            miniGun = new Gun();
            //miniGun.position.X = theBoss.position.X + 50; // doesn't work
            //miniGun.position = new Vector2(250, 275);
            miniGun.position = new Vector2(theBoss.position.X+320, theBoss.position.Y+175);
            miniGun.motion = new Vector2(10f, 1f);
            miniGun.Width = 150;
            miniGun.Height = 55;
            //miniGun.Width = 200;
            //miniGun.Height = 70;

            gunAngle = 0;
            origin = new Vector2(miniGun.position.X + 10, miniGun.position.Y + 10);

            //private float gunAngle = 35f;

            // Initialize Enemy
            enemy1 = new Enemy();
            //enemy1.position = new Vector2(700, 500);
            enemy1.position = new Vector2(1100, 500);
            enemy1.motion = new Vector2(-5f, 0f);
            enemy1.Width = 50;
            enemy1.Height = 50;

            //textures = new List<Texture2D>();
            //textures.Add(Content.Load<Texture2D>("circle 1a"));
            //textures.Add(Content.Load<Texture2D>("star 1a"));
            //textures.Add(Content.Load<Texture2D>("diamond 1a"));
            //mainWeapon = new ParticleEngine(textures, new Vector2(400, 240));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            background = Content.Load<Texture2D>("light forest");

            // .. stupid problem with Content load directory; unable to load from 'Images' folder..
            //RexTexture = Content.Load<Texture2D>("Rex 1a");
            RexTexture = Content.Load<Texture2D>("Rex 2a");
            GunTexture = Content.Load<Texture2D>("Gun 2a");
            EnemyTexture = Content.Load<Texture2D>("Enemy 1a");
            BulletTexture = Content.Load<Texture2D>("Bullet 1a");

            /*
            ContentManager contentManager = new ContentManager(this.Services, @"Content\Sound Effects\");
            hitWallSound = contentManager.Load<SoundEffect>("boing2");

            ContentManager contentManager2 = new ContentManager(this.Services, @"Content\Sound Effects\");
            hitPaddleSound = contentManager2.Load<SoundEffect>("womp2");
            */

            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("circle 1a"));
            textures.Add(Content.Load<Texture2D>("star 1a"));
            textures.Add(Content.Load<Texture2D>("diamond 1a"));
            mainWeapon = new ParticleEngineB(textures, new Vector2(400, 240));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            //if(mouse.LeftButton.

            if (previousMousePosY == 0)
            {
                previousMousePosY = mouse.Y;
            }
            //Vector2 mousePos = new Vector2(mouse.X, mouse.Y);
            //float previousMousePosY = mouse.Y;



            // Allows the game to exit            
            if (keyboard.IsKeyDown(Keys.Escape))
                this.Exit(); 
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();

            // TODO: Add your update logic here
            
            //Enemy movement
            enemy1.position += enemy1.motion;     // CRITICAL!!

            //gunAngle += 0.01f;
            if(mouse.Y > previousMousePosY)
            {
                //gunAngle += .05f;
                gunAngle -= (0.008f) * (previousMousePosY - mouse.Y);
            }
            if (mouse.Y < previousMousePosY)
            {
                //gunAngle -= .05f;
                gunAngle -= (0.008f) * (previousMousePosY - mouse.Y);
            }

            
            if (enemy1.position.X <= 500 || enemy1.position.X >=
            SCREEN_WIDTH + enemy1.Width*4)
            {
                enemy1.motion = new Vector2(-enemy1.motion.X,
                enemy1.motion.Y); // reverse X direction
                //hitWallSound.Play();
            }
            /*
            if (enemy1.position.Y <= 0 || enemy1.position.Y >= SCREEN_HEIGHT - enemy1.Height)
            {
                enemy1.motion = new Vector2(enemy1.motion.X, -enemy1.motion.Y);
                //hitWallSound.Play();
            }
            */

            theBoss.position += theBoss.motion;
            //miniGun.position += theBoss.motion;
            miniGun.position = new Vector2(theBoss.position.X + 320, theBoss.position.Y + 175);

            theBoss.motion = new Vector2(0, 0);

            // Keyboard input
            if (keyboard.IsKeyDown(Keys.S))
            {
                theBoss.motion = new Vector2(0, 5);
            }
            if (keyboard.IsKeyDown(Keys.W))
            {
                theBoss.motion = new Vector2(0, -5);
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                theBoss.motion = new Vector2(-5, 0);
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                theBoss.motion = new Vector2(5, 0);
            }

            // weak attempt at a jump
            //
            //if(Keys.Space == ButtonState.Released)
            //if ( == ButtonState.Released)
            if (keyboard.IsKeyDown(Keys.Space))
            {
                theBoss.motion = new Vector2(0, -10);
            }

            // fire bullet
            //float x = mouse.LeftButton.;
            ///*
            if(mouse.LeftButton == ButtonState.Pressed)
            {
                //theBoss.motion = new Vector2(20, 0);  // works
            }
            //*/

            // Rex y-boundaries
            if (theBoss.position.Y <= 150)
                theBoss.position = new Vector2(theBoss.position.X, 150);
            if (theBoss.position.Y >= SCREEN_HEIGHT - 300)
                theBoss.position = new Vector2(theBoss.position.X, SCREEN_HEIGHT - theBoss.Height); //300);
            
            // Gun y-boundaries
            //if (miniGun.position.Y <= 175)
            //    miniGun.position = new Vector2(miniGun.position.X, 175);
            if (miniGun.position.Y <= theBoss.position.Y + 175 )
                miniGun.position = new Vector2(miniGun.position.X, theBoss.position.Y+175);
            if (miniGun.position.Y >= SCREEN_HEIGHT - 125)
                miniGun.position = new Vector2(miniGun.position.X, SCREEN_HEIGHT - 125);
            //*/
            
            // Rex x-boundaries
            if (theBoss.position.X <= -150)
                theBoss.position = new Vector2(-150, theBoss.position.Y);
            if (theBoss.position.X >= SCREEN_WIDTH - theBoss.Width)
                theBoss.position = new Vector2(SCREEN_WIDTH - theBoss.Width, theBoss.position.Y);

            // Gun x-boundaries
            if (miniGun.position.X <= 170)
                miniGun.position = new Vector2(170, miniGun.position.Y);
            if (miniGun.position.X >= SCREEN_WIDTH - 180)
                miniGun.position = new Vector2(SCREEN_WIDTH - 180, miniGun.position.Y);
            
            previousMousePosY = mouse.Y;

            //mainWeapon.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            mainWeapon.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, 1000, 600), Color.White);
            //spriteBatch.Draw(background, new Rectangle(0, 0, (int)SCREEN_HEIGHT), (int)SCREEN_WIDTH), Color.White);

            spriteBatch.Draw(RexTexture, theBoss.position, Color.White);
            
            /*
            Rectangle sourceRectangle = new Rectangle(0, 0, theBoss.Width, theBoss.Height);
            //spriteBatch.Draw(RexTexture, theBoss.position, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);
            Vector2 origin = new Vector2(theBoss.Width / 2, theBoss.Height / 2);
            spriteBatch.Draw(RexTexture, theBoss.position, sourceRectangle, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 1);
            */

            //spriteBatch.Draw(GunTexture, miniGun.rect, Color.White);
            //Rectangle sourceRectangle = new Rectangle(0, 0, miniGun.Width, miniGun.Height);
            //spriteBatch.Draw(RexTexture, theBoss.position, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);
            //Vector2 origin = new Vector2(miniGun.Width / 2, miniGun.Height / 2);
            //Vector2 origin = new Vector2(miniGun.position.X+10, miniGun.position.Y+10);
            //spriteBatch.Draw(GunTexture, miniGun.position, sourceRectangle, Color.White, gunAngle, origin, 1.0f, SpriteEffects.None, 1);

            
            //crap
            //int x = (int)miniGun.position.X;            
            //Rectangle sourceRectangle = new Rectangle((int)miniGun.position.X, (int)miniGun.position.Y, miniGun.Width, miniGun.Height);
            //Rectangle sourceRectangle = new Rectangle(
            //origin = new Vector2(miniGun.position.X, miniGun.position.Y);

            //Vector2 location = new Vector2(miniGun.position.X, miniGun.position.Y);
            Rectangle sourceRectangle = new Rectangle(0, 0, miniGun.Width, miniGun.Height);
            origin = new Vector2(miniGun.Width-140, miniGun.Height-35);
            spriteBatch.Draw(GunTexture, miniGun.position, sourceRectangle, Color.White, gunAngle, origin, 1.0f, SpriteEffects.None, 1);
            //spriteBatch.Draw

            spriteBatch.Draw(EnemyTexture, enemy1.rect, Color.White);
            mainWeapon.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
