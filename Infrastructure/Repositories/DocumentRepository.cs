using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DocumentRepository : GenericRepository<Document>, IDocumentRepository
    {
        public DocumentRepository(SRMSContext context) : base(context)
        {
        }

        public async Task CreateDocumentAsync(Document document)
        {
            await AddAsync(document);
        }

        public async Task<Document> GetDocumentByIdAsync(Guid id)
        {
            return await Find(x => x.Id.Equals(id)).FirstAsync();
        }

        public IQueryable<Document> GetIncludeDocumentsAsync(Guid documentId)
        {
            return Find(x => x.Id.Equals(documentId));
        }

        public async Task<Topic> GetIncludeTopicAsync(Guid documentId)
        {
            return await Find(x => x.Id == documentId)
                        .Include(x => x.Review)
                        .ThenInclude(x => x.Topic)
                        .Select(x => x.Review.Topic).FirstAsync();
        }

        public void UpdateDocumentAsync(Document document)
        {
            Update(document);
        }
    }
}
