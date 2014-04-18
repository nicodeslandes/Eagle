using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using Caliburn.Micro;

namespace Eagle.ViewModels
{
    public interface IFilter
    {
        LineViewModel FilterLine(LineViewModel line);
    }

    [Export(typeof(IFilter))]
    [Export]
    public class Filter : PropertyChangedBase, IFilter
    {
        private string _filterExpression;
        private Regex _filterRegex;

        public LineViewModel FilterLine(LineViewModel line)
        {
            if (line == null)
                return null;

            return _filterRegex == null || _filterRegex.IsMatch(line.Text) ? line : null;
        }

        public string FilterExpression
        {
            get { return _filterExpression; }
            set
            {
                if (_filterExpression != value)
                {
                    _filterExpression = value;
                    if (string.IsNullOrWhiteSpace(_filterExpression))
                    {
                        _filterRegex = null;
                    }
                    else
                    {
                        _filterRegex = new Regex(_filterExpression, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    }

                    NotifyOfPropertyChange(() => FilterExpression);
                }
            }
        }
    }
}