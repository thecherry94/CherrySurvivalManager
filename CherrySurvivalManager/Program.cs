using System;
using SFML.Window;

namespace CherrySurvivalManager {
    class Program {
        static void Main(string[] args) {
            var window = new SimpleWindow();
            window.Run();
        }
    }

    class SimpleWindow {
        public void Run() {
            var mode = new SFML.Window.VideoMode(1280, 800);
            var window = new SFML.Graphics.RenderWindow(mode, "SFML");
            window.KeyPressed += Window_KeyPressed;
            window.Closed += Window_Closed;


            while (window.IsOpen) {
                window.DispatchEvents();
                window.Display();
            }
        }

        private void Window_Closed(object sender, EventArgs e) {
            var window = (SFML.Window.Window)sender;
            window.Close();
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e) {
            var window = (SFML.Window.Window)sender;
            switch (e.Code) {
                case SFML.Window.Keyboard.Key.Escape: 
                    window.Close();   
                break;
            }
        }
    }
}
