using System.Linq;
using Battleships.Logic.Features;
using Battleships.Logic.Public;

namespace Battleships.Console.Output
{
    internal interface IGridResultPainter
    {
        void PaintResult(ShotResult result);
    }

    internal class GridResultPainter : IGridResultPainter
    {
        private readonly ICursorHelper _cursorHelper;
        private readonly IOutputWriter _outputWriter;

        public GridResultPainter(ICursorHelper cursorHelper, IOutputWriter outputWriter)
        {
            _cursorHelper = cursorHelper;
            _outputWriter = outputWriter;
        }

        public void PaintResult(ShotResult result)
        {
            if (result.Kind == ShotResult.Kinds.Sink)
                PaintShipSink(result);
            else if (result.Kind == ShotResult.Kinds.Hit)
                PaintShipHit(result.Coordinate, 'X');
            else if (result.Coordinate != null)
                PaintShipHit(result.Coordinate, '.');
        }

        private void PaintShipSink(ShotResult result)
        {
            result.SinkShip.ToList().ForEach(x =>
                PaintShipHit(x, '#'));
        }

        private void PaintShipHit(Coordinate coordinate, char hit)
        {
            var x = _cursorHelper.GetLeftForCoordinate(coordinate);
            var y = _cursorHelper.GetTopForCoordinate(coordinate);
            _outputWriter.SetCursorPosition(x, y);
            _outputWriter.Write(hit.ToString());
        }
    }
}