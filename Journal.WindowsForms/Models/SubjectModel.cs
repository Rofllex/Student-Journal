using Journal.ClientLib.Entities;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Journal.WindowsForms.Models
{
    public class SubjectModel 
    {
        public static SubjectModel[] FromOriginal(IEnumerable<Subject> subjects)
            => subjects.ToList().ConvertAll(s => new SubjectModel(s)).ToArray();
        
        public static implicit operator Subject(SubjectModel subj)
                => subj.Original;

        public SubjectModel(Subject original)
        {
            Original = original;
        }

        [Browsable(false)]
        public Subject Original { get; private set; }

        [DisplayName("Название")]
        public string Name => Original.Name;

        public override string ToString()
            => Name;
    }

}
