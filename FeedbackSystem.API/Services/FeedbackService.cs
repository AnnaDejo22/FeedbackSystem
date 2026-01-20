using FeedbackSystem.API.Data;
using FeedbackSystem.API.Models;
using FeedbackSystem.API.Services.Interfaces;
using FeedbackSystem.Contracts.DTO;
using FeedbackSystem.API.Enum;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackSystem.API.Services;

public class FeedbackService : IFeedbackService
{
    private readonly AppDbContext _context;

    public FeedbackService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FeedbackDTO> SubmitFeedbackAsync(FeedbackDTO feedbackDto)
    {
        var feedbackEntity = new Feedback
        {
            UserName = feedbackDto.UserName,
            Feedbacks = feedbackDto.FeedbackText,
            Rating = feedbackDto.Rating,
            Status = FeedBackStatus.New,
            FeedbackCategory=feedbackDto.FeedbackCategory,
            SubmittedOn = DateTime.Now
        };

        _context.Feedbacks.Add(feedbackEntity);
        await _context.SaveChangesAsync();

        return new FeedbackDTO
        {
            Id = feedbackEntity.Id,
            UserName = feedbackEntity.UserName,
            FeedbackText = feedbackEntity.Feedbacks,
            Rating = feedbackEntity.Rating,
            Status = feedbackEntity.Status.ToString(),
            FeedbackCategory=feedbackEntity.FeedbackCategory,
            SubmittedOn = feedbackEntity.SubmittedOn
        };
    }

    public async Task<List<FeedbackDTO>> GetFeedbacks(int? status)
    {
        List<Feedback> feedbackEntities;
        if(status==null)
            feedbackEntities = _context.Feedbacks.ToList();
        else
            feedbackEntities= _context.Feedbacks.Where(fb=> (FeedBackStatus)fb.Status==(FeedBackStatus)status).ToList();

        return feedbackEntities.Select(feedback => new FeedbackDTO
        {
            Id = feedback.Id,
            UserName = feedback.UserName,
            FeedbackText = feedback.Feedbacks,
            Rating = feedback.Rating,
            Status = feedback.Status.ToString(),
            SubmittedOn = feedback.SubmittedOn
        }).ToList();
    }

    public async Task<FeedbackDTO> UpdateFeedbackStatusAsync(int id, FeedBackStatus status)
    {
        var feedback = await _context.Feedbacks.FindAsync(id);
        if (feedback == null)
            throw new Exception("Feedback not found");

        feedback.Status = status;
        await _context.SaveChangesAsync();

        return new FeedbackDTO
        {
            Id = feedback.Id,
            UserName = feedback.UserName,
            FeedbackText = feedback.Feedbacks,
            Rating = feedback.Rating,
            Status = feedback.Status.ToString(),
            SubmittedOn = feedback.SubmittedOn
        };
    }

    public async Task<bool> DeleteFeedbackAsync(int id)
    {
        var feedback = await _context.Feedbacks.FindAsync(id);
        if (feedback == null)
            return false;
        _context.Feedbacks.Remove(feedback);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<FeedbackDTO>> SortFeedbacksByStatusAsync(FeedBackStatus status)
    {
        var feedbacks = _context.Feedbacks.Where(fb => fb.Status == status).ToList();

        var feedbackDtos = feedbacks.Select(feedback => new FeedbackDTO
        {
            Id = feedback.Id,
            UserName = feedback.UserName,
            FeedbackText = feedback.Feedbacks,
            Rating = feedback.Rating,
            Status = feedback.Status.ToString(),
            SubmittedOn = feedback.SubmittedOn
        }).ToList();
        return feedbackDtos;
    }
}