using Application.IServices;
using Application.Utils;
using Application.ViewModels.DocumentVMs;
using Domain.Entities;
using Domain.Interfaces;

namespace WebAPI.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DocumentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ResubmitDocumentForEarlyReviewAsync(ResubmitEarlyDocumentReq req)
        {
            var review = await _unitOfWork.Review.GetCurrentReviewByTopicIdAsync(req.TopicId);
            var document = new Document
            {
                Id = Guid.NewGuid(),
                ReviewId = review.Id,
                DocumentFileLink = req.NewFile.FileLink,
            };
            await _unitOfWork.Document.CreateDocumentAsync(document);

            var updateTopic = await _unitOfWork.Topic.GetTopicAsync(req.TopicId);
            updateTopic.SetSateAndProgress(8);

            await _unitOfWork.Save();
        }

        public async Task ResubmitDocumentForFinalReviewAsync(ResubmitFinalDocumentReq req)
        {
            var review = await _unitOfWork.Review.GetCurrentReviewByTopicIdAsync(req.TopicId);
            var document = new Document
            {
                Id = Guid.NewGuid(),
                ReviewId = review.Id,
                DocumentFileLink = req.NewFile.FileLink,
            };
            await _unitOfWork.Document.CreateDocumentAsync(document);

            var updateTopic = await _unitOfWork.Topic.GetTopicAsync(req.TopicId);
            updateTopic.SetSateAndProgress(20);

            await _unitOfWork.Save();
        }

        public async Task SupplementationDocumentForFinalReviewAsync(SupplementationFinalDocumentReq req)
        {
            var review = await _unitOfWork.Review.GetCurrentReviewByTopicIdAsync(req.TopicId);
            var document = new Document
            {
                Id = Guid.NewGuid(),
                ReviewId = review.Id,
                DocumentFileLink = req.NewFile.FileLink,
            };
            await _unitOfWork.Document.CreateDocumentAsync(document);

            var updateTopic = await _unitOfWork.Topic.GetTopicAsync(req.TopicId);
            updateTopic.SetSateAndProgress(17);

            await _unitOfWork.Save();
        }

        public async Task SupplementationDocumentForMiddleReviewAsync(SupplementationMiddleDocumentReq req)
        {
            var review = await _unitOfWork.Review.GetCurrentReviewByTopicIdAsync(req.TopicId);
            var document = new Document
            {
                Id = Guid.NewGuid(),
                ReviewId = review.Id,
                DocumentFileLink = req.NewFile.FileLink,
            };
            await _unitOfWork.Document.CreateDocumentAsync(document);

            var updateTopic = await _unitOfWork.Topic.GetTopicAsync(req.TopicId);
            updateTopic.SetSateAndProgress(12);

            await _unitOfWork.Save();
        }       
    }
}
