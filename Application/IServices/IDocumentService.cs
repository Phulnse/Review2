using Application.ViewModels.DocumentVMs;

namespace Application.IServices
{
    public interface IDocumentService
    {
        Task ResubmitDocumentForEarlyReviewAsync(ResubmitEarlyDocumentReq req);
        Task SupplementationDocumentForMiddleReviewAsync(SupplementationMiddleDocumentReq req);
        Task SupplementationDocumentForFinalReviewAsync(SupplementationFinalDocumentReq req);
        Task ResubmitDocumentForFinalReviewAsync(ResubmitFinalDocumentReq req);
    }
}
