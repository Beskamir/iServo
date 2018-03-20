using System.Windows;

namespace Controller.CellData
{
    public class CellContents
    {
        public ServoVec2<int> Position { get; set; }

        public CellContents(ServoVec2<int> initPosition)
        {
            Position = initPosition;
        }
    }
}