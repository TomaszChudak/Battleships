using Battleships.Logic.Public;

namespace Battleships.Console.Output
{
    internal interface ISoundPlayer
    {
        void PlayResult(ShotResult shotResult);
    }

    internal class SoundPlayer : ISoundPlayer
    {
        public void PlayResult(ShotResult shotResult)
        {
            if (shotResult.Coordinate == null)
                return;
            if (shotResult.Kind == ShotResult.Kinds.GameEnd)
                PlayGameEnd();
            else if (shotResult.Kind == ShotResult.Kinds.Sink)
                PlaySink();
            else if (shotResult.Kind == ShotResult.Kinds.Hit)
                PlayHit();
            else
                PlayWater();
        }

        private void PlayWater()
        {
            Beep(0);
        }

        private void PlayHit()
        {
            Beep(1);
        }

        private void PlaySink()
        {
            Beep(2);
        }

        private void PlayGameEnd()
        {
            Beep(3);
        }

        private void Beep(int count)
        {
            for (var i = 0; i < count; i++)
                System.Console.Beep();
        }
    }
}