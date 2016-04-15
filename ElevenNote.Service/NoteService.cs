using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Service
{
    public class NoteService
    {
        private readonly Guid _userId;

        public NoteService(Guid userId)
        {
            _userId = userId;
        }

        public IEnumerable<NoteListItemViewModel> GetNotes()
        {
            using (var ctx = new ElevenNoteDbContext())
            {
                return
                    ctx
                        .Notes
                        .Where(e => e.OwnerId == _userId)
                        .Select(
                            e =>
                                new NoteListItemViewModel
                                {
                                    NoteId = e.NoteId,
                                    Title = e.Title,
                                    IsStarred = e.IsStarred,
                                    CreatedUtc = e.CreatedUtc
                                })
                        .ToArray();
            }
        }

        public bool CreateNote(NoteCreateViewModel vm)
        {
            using (var ctx = new ElevenNoteDbContext())
            {
                var entity =
                    new NoteEntity
                    {
                        OwnerId = _userId,
                        Title = vm.Title,
                        Content = vm.Content,
                        CreatedUtc = DateTimeOffset.UtcNow
                    };

                ctx.Notes.Add(entity);

                return ctx.SaveChanges() == 1;
            }
        }

        public NoteDetailViewModel GetNoteById(int noteId)
        {
            NoteEntity entity;

            using (var ctx = new ElevenNoteDbContext())
            {
                entity =
                    ctx
                        .Notes
                        .Single(e => e.OwnerId == _userId && e.NoteId == noteId);
            }

            return
                new NoteDetailViewModel
                {
                    Title = entity.Title,
                    Content = entity.Content,
                    NoteId = entity.NoteId,
                    isStarred = entity.IsStarred,
                    CreateUtc = entity.CreatedUtc,
                    ModifiedUtc = entity.ModifiedUtc
                };
        }

        public bool UpdateNote(NoteDetailViewModel vm)
        {
            using (var ctx = new ElevenNoteDbContext())
            {
                var entity =
                    ctx
                        .Notes
                        .Single(e => e.OwnerId == _userId && e.NoteId == vm.NoteId);

                entity.Title = vm.Title;
                entity.Content = vm.Content;
                entity.IsStarred = vm.isStarred;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        } 

        public bool DeleteNote(int noteId)
        {
            using (var ctx = new ElevenNoteDbContext())
            {
                var entity =
                    ctx
                        .Notes
                        .Single(e => e.OwnerId == _userId && e.NoteId == noteId);

                ctx.Notes.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
