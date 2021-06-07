using System.ComponentModel;

namespace Journal.WindowsForms.Models
{
    public interface IModel
    {
        [Browsable(false)]
        object Original { get; }
    }
}
