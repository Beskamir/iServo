using System.Windows.Shapes;
using Controller.CellData;

namespace Controller
{
    public class GridCell
    {
        public Rectangle Representation { get; set; }
        public CellContents CellContents { get; set; }
        public GridCell(Rectangle representation, CellContents cellContents)
        {
            Representation = representation;
            CellContents = cellContents;
        }
    }
}