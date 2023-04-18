using Vekalat.Core.Entities;
using Vekalat.InfraStructure.Data;
using System.Linq;
using static Vekalat.Application.Features.BlogSubjectFeature;

namespace Ehda.InfraStructure.Data.Repositories
{
    public class BlogSubjectRepository : Repository<BlogSubject>, IBlogSubjectRepository
    {
        private readonly VekalatDataContext _context;
        public BlogSubjectRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<BlogSubject> GetAllWithFilter(BlogSubjectFilterInput blogSubjectFilterInput)
        {
            var items = _context.BlogSubjects
            .OrderByDescending(e => e.Id)
         .AsQueryable();

            if (!string.IsNullOrEmpty(blogSubjectFilterInput.SearchFilter))
                items = items.Where(e => e.Title.Contains(blogSubjectFilterInput.SearchFilter));
            return items;
        }



        //public IQueryable<News_SubjectFeature.News_SubjectDto> GetNews_SubjectForAdmin(string search)
        //{
        //    var result = _context.News_Subjects.AsNoTracking().AsQueryable();
        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        result = result.Where(s => EF.Functions.Like(s.Title, $"%{search}%"));
        //    }
        //    return result.OrderByDescending(c => c.Id).Select(c => new News_SubjectFeature.News_SubjectDto()
        //    {
        //        Id = c.Id,
        //        Title = c.Title,

        //    });
        //}

        //public IQueryable<News_SubjectFeature.News_SubjectDto> GetNews_SubjectList()
        //{
        //    var result = _context.News_Subjects.AsNoTracking().AsQueryable();
        //    return result.OrderByDescending(c => c.Id).Select(c => new News_SubjectFeature.News_SubjectDto()
        //    {
        //        Id = c.Id,
        //        Title = c.Title,
        //    });
        //}

        //public IQueryable<News_SubjectFeature.News_SubjectDto> GetTopNews_SubjectList()
        //{
        //    var result = _context.News_Subjects.AsNoTracking().AsQueryable();
        //    return result.OrderByDescending(c => c.Id).Take(6).Select(c => new News_SubjectFeature.News_SubjectDto()
        //    {
        //        Id = c.Id,
        //        Title = c.Title
        //    });
        //}
        //public async Task<bool> EditNews_SubjectInfo(int News_SubjectId, News_SubjectFeature.EditNews_SubjectInfoDto EditNews_SubjectInfoDto, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var News_Subject = await _context.News_Subjects.FirstOrDefaultAsync(u => u.Id == News_SubjectId);
        //        if (News_Subject == null)
        //            return false;

        //        News_Subject.Title = EditNews_SubjectInfoDto.Title;
        //        _context.Update(News_Subject);
        //        await _context.SaveChangesAsync(cancellationToken);
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public async Task<List<BlogSubject>> GetSubjectList()
        //{
        //    return await _context.News_Subjects.ToListAsync();
        //}

    }
}
