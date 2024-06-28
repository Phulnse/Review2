using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IDocumentRepository
    {
        Task CreateDocumentAsync(Document document);
        void UpdateDocumentAsync(Document document);
        Task<Document> GetDocumentByIdAsync(Guid id);
        Task<Topic> GetIncludeTopicAsync(Guid documentId);
        IQueryable<Document> GetIncludeDocumentsAsync(Guid documentId);
    }
}
