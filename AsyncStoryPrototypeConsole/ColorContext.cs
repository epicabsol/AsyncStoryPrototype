using System;

namespace AsyncStoryPrototypeConsole
{
    public class ColorContext : IDisposable
    {
        public ColorContext(int foregroudColorCode)
        {
            SetForegroundColor(foregroudColorCode);
        }

        public void Dispose()
        {
            ResetForegroundColor();
        }

        private void SetForegroundColor(int colorCode)
        {
            Console.Write("\u001b[38;5;" + colorCode + "m");
        }

        private void ResetForegroundColor()
        {
            Console.Write("\u001b[0m");
        }
    }
}
