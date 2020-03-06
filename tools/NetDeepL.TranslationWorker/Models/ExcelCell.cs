using ClosedXML.Excel;

namespace NetDeepL.TranslationWorker.Models
{
    public class ExcelCell
    {
        private readonly IXLCell _cell;

        public ExcelCell(IXLCell cell)
        {
            _cell = cell;
        }

        public string Text => _cell.Value?.ToString();
        public IXLAddress Address => _cell.Address;
    }
}
