using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models
{
    public class NoteDetailViewModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        [UIHint("Starred")]
        public bool isStarred { get; set; }

        public int NoteId { get; set; }

        public DateTimeOffset CreateUtc { get; set; }

        public DateTimeOffset? ModifiedUtc { get; set; }
    }
}
