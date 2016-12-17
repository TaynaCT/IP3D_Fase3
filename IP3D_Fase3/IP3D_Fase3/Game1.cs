﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace IP3D_Fase3
{
    
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map mapa;
        CameraSurfaceFollow camera;
        ClsTank tank, tank2;
        Dustgen gen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            camera = new CameraSurfaceFollow(GraphicsDevice);
            gen = new Dustgen(graphics.GraphicsDevice, camera);
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
            mapa = new Map(GraphicsDevice, Content, camera);
            tank = new ClsTank(GraphicsDevice, Content, camera, mapa, new Vector2(10, 10));
            tank2 = new ClsTank(GraphicsDevice, Content, camera, mapa, new Vector2(10, 6));
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), (float)gameTime.ElapsedGameTime.TotalSeconds

            camera.Update(new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), (float)gameTime.ElapsedGameTime.TotalSeconds, mapa.SurfaceFollow(camera.position.X, camera.position.Z));
            camera.View();
            tank.Update(1, gameTime);
            tank2.Update(2, gameTime);
            gen.ciclo();

            if (Collisions.TankCollision(tank.tankModel, tank.BoneTransforms, tank2.tankModel, tank2.BoneTransforms))
            {
                tank.Position = tank.LasPosition;
                tank2.Position = tank2.LasPosition;
            }

            //COLISÕES
            Console.WriteLine(Collisions.TankCollision(tank.tankModel, tank.BoneTransforms,
                tank2.tankModel, tank2.BoneTransforms));
            if (tank2.GetSetBullet != null && tank2.GetSetBullet.BulletFlag)
            {
               if( Collisions.BulletCollision(tank.tankModel, tank.BoneTransforms, tank2.GetSetBullet.BulletModel, tank2.GetSetBullet.WorldMatrix))
                {
                    tank2.GetSetBullet.BulletFlag = false;
                    tank2.GetSetBullet = null;
                }

                Console.WriteLine("BULLET TANK2 - TANK 1 = " + Collisions.BulletCollision(tank.tankModel, tank.BoneTransforms,
                                                                                       tank2.GetSetBullet.BulletModel, tank2.GetSetBullet.WorldMatrix));
            }
            if (tank.GetSetBullet != null && tank.GetSetBullet.BulletFlag)
            {
                if(Collisions.BulletCollision(tank2.tankModel, tank2.BoneTransforms,tank.GetSetBullet.BulletModel, tank.GetSetBullet.WorldMatrix))
                {
                    tank.GetSetBullet.BulletFlag = false;
                    tank.GetSetBullet = null;
                }

                Console.WriteLine("BULLET TANK1 - TANK2 = " + Collisions.BulletCollision(tank2.tankModel, tank2.BoneTransforms,
                                                                                         tank.GetSetBullet.BulletModel, tank.GetSetBullet.WorldMatrix));
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            mapa.Draw(GraphicsDevice);
            tank.Draw();
            tank2.Draw();
            gen.Draw();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

