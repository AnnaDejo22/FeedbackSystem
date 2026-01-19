using FeedbackSystem.Contracts.DTO;
using FeedbackSystem.API.Enum;

namespace FeedbackSystem.API.Services.Interfaces;

public interface IFeedbackService
{
    Task<FeedbackDTO> SubmitFeedbackAsync(FeedbackDTO feedbackDto);
    Task<List<FeedbackDTO>> GetAllFeedbacks();
    Task<FeedbackDTO> UpdateFeedbackStatusAsync(int id, FeedBackStatus status);
    Task<bool> DeleteFeedbackAsync(int id);
    Task<List<FeedbackDTO>> SortFeedbacksByStatusAsync(FeedBackStatus status);
}
