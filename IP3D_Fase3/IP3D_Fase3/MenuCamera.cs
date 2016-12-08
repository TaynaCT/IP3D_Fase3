using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3D_Fase3
{
    class MenuCamera
    {
        GraphicsDevice device;
        Matrix projection;
        Matrix view;
        Vector3 position;

        CameraFree freeCam;
        CameraSurfaceFollow sfCam;
        CameraThirdPerson tpCam;

        int option;

        public MenuCamera(GraphicsDevice device, Vector3 pos)
        {
            this.device = device;
            position = pos;
            option = 1;
            Select();
            SwitchOption();
        }

        //Seleciona o valor de options apartir do input das teclas
        public void Select()
        {
            //F1 - seleciona camera 3° pessoa
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                option = 1;                
            }
            //F2 - seleciona camera com surface follow
            else if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                option = 2;                
            }
            //F3 - seleciona camera livre 
            else if (Keyboard.GetState().IsKeyDown(Keys.F3))
            {
                option = 3;                
            }
        }

        //seleciona a camera de qacordo com o valor de option
        public void SwitchOption()
        {
            switch (option)
            {
                case 1:
                tpCam = new CameraThirdPerson(device, position);
                projection = tpCam.projection;
                view = tpCam.view;
                break;
                case 2:
                    sfCam = new CameraSurfaceFollow(device);
                projection = sfCam.projection;
                view = sfCam.view;
                break;
                case 3:
                    freeCam = new CameraFree(device);
                projection = freeCam.projection;
                view = freeCam.view;
                break;
            }
        }
        
        public void Update(Point centre, float timePassed, float surfaceFollow)
        {
            switch (option)
            {
                case 1:
                    tpCam.Update(centre, timePassed, surfaceFollow);
                    break;
                case 2:
                    sfCam.Update(centre, timePassed, surfaceFollow);
                    break;
                case 3:
                    freeCam.Update(centre, timePassed);
                    break;
            }
        }

        public Matrix Projection
        {
            get { return projection; }
        }

        public Matrix View
        {
            get { return view; }
        }

    }
}
