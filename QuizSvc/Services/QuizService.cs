using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Quiz.Protos;
using QuizSvc.Application.Commands.CampaignQuizSettings;
using QuizSvc.Application.Commands.Campaigns;
using QuizSvc.Application.Commands.Dimensions;
using QuizSvc.Application.Commands.Interactions;
using QuizSvc.Application.Commands.Leads;
using QuizSvc.Application.Commands.Questions;
using QuizSvc.Application.Commands.QuizAttempts;
using QuizSvc.Application.Commands.Quizs;
using QuizSvc.Application.Commands.Recommendations;
using QuizSvc.Application.Commands.ScoringRules;
using QuizSvc.Application.Queries.CampaignQuizSettings;
using QuizSvc.Application.Queries.Campains;
using QuizSvc.Application.Queries.Dimensions;
using QuizSvc.Application.Queries.Interactions;
using QuizSvc.Application.Queries.Leads;
using QuizSvc.Application.Queries.Questions;
using QuizSvc.Application.Queries.QuizAttempts;
using QuizSvc.Application.Queries.QuizResults;
using QuizSvc.Application.Queries.Quizs;
using QuizSvc.Application.Queries.Recommendations;
using QuizSvc.Application.Queries.ScoringRules;

namespace QuizSvc.Services;

public class QuizService : QuizV2Grpc.QuizV2GrpcBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<QuizService> _logger;

    public QuizService(IMediator mediator, IMapper mapper, ILogger<QuizService> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    #region Campain
    public override async Task<CampaignMiniResponse> CreateCampaign(CreatedCampaignRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Create Campaign Function");

        var command = _mapper.Map<CreatedCampaignCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<CampaignMiniResponse>(result);

        return response;
    }

    public override async Task<CampaignResponse> GetCampaignDetail(GetCampaignDetailRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Campaign Detail Function");

        var query = _mapper.Map<GetCampaignDetailQuery>(request);

        var result = await _mediator.Send(query);

        var response = _mapper.Map<CampaignResponse>(result);

        return response;
    }

    public override async Task<CampaignMiniResponse> UpdateCampaign(UpdatedCampaignRequest request, ServerCallContext context)
    {
        var command = _mapper.Map<UpdatedCampaignCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<CampaignMiniResponse>(result);

        return response;
    }

    public override async Task<PagedCampaignMiniResponse> GetCampaignList(GetCampaignListRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Campaign List Function");

        var query = _mapper.Map<GetCampaignListQuery>(request);

        var result = await _mediator.Send(query);

        var response = new PagedCampaignMiniResponse
        {
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize,
        };
        response.Data.AddRange(_mapper.Map<List<CampaignMiniResponse>>(result.Data));
        return response;
    }


    #endregion Campain

    #region CampainQuizSetting
    public override async Task<CampaignQuizSettingMiniResponse> CreateCampaignQuizSetting(CreateCampaignQuizSettingProto request, ServerCallContext context)
    {
        _logger.LogInformation("Create Campaign quiz setting Function");

        var command = _mapper.Map<CreateCampaignQuizSettingCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<CampaignQuizSettingMiniResponse>(result);

        return response;
    }

    public override async Task<CampaignQuizSettingMiniResponse> UpdateCampaignQuizSetting(UpdatedCampaignQuizSettingRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Update Campaign quiz setting Function");

        var command = _mapper.Map<UpdateCampaignQuizSettingCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<CampaignQuizSettingMiniResponse>(result);

        return response;
    }

    public override async Task<CampaignQuizSettingResponseProto> GetCampaignQuizSettingDetail(GetCampaignQuizSettingDetailRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Campaign quiz setting detail Function");

        var query = new GetCampaignQuizSettingDetailQuery { Id = Guid.Parse(request.Id) };

        var result = await _mediator.Send(query);

        var response = _mapper.Map<CampaignQuizSettingResponseProto>(result);

        return response;
    }

    public override async Task<Empty> DeleteCampaignQuizSetting(DeleteCampaignQuizSettingRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Deleted CampaignQuizSetting Function");

        var command = new DeleteCampaignQuizSettingCommand() { Id = Guid.Parse(request.Id) };

        await _mediator.Send(command);

        return new Empty();
    }

    #endregion CampainQuizSetting

    #region Quiz
    public override async Task<QuizMiniResponse> CreateQuiz(CreatedQuizRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Create Quiz Function");

        var command = _mapper.Map<CreatedQuizCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<QuizMiniResponse>(result);

        return response;
    }

    public override async Task<QuizMiniResponse> CloneQuiz(CloneQuizRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Clone Quiz Function");

        var command = _mapper.Map<CloneQuizCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<QuizMiniResponse>(result);

        return response;
    }

    public override async Task<QuizMiniResponse> DeepCopyQuiz(DeepCopyQuizRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Deep Copy Quiz Function");

        var command = _mapper.Map<DeepCopyQuizCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<QuizMiniResponse>(result);

        return response;
    }

    public override async Task<QuizMiniResponse> UpdateQuiz(UpdatedQuizRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Update Quiz Function");

        var command = _mapper.Map<UpdateQuizCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<QuizMiniResponse>(result);

        return response;
    }

    public override async Task<PagedQuizMiniResponse> GetQuizList(GetQuizListRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Quiz List Function");

        var query = _mapper.Map<GetQuizListQuery>(request);

        var result = await _mediator.Send(query);

        var response = new PagedQuizMiniResponse
        {
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize,
        };

        response.Data.AddRange(_mapper.Map<List<QuizMiniResponse>>(result.Data));
        return response;
    }

    public override async Task<QuizResponse> GetQuizDetail(GetQuizDetailRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Quiz Detail Function");

        var query = _mapper.Map<GetQuizDetailQuery>(request);

        var result = await _mediator.Send(query);

        var response = _mapper.Map<QuizResponse>(result);

        return response;
    }

    public override async Task<GetQuizInCampaignDetailResponse> GetQuizInCampaignDetail(GetQuizInCampaignDetailRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Quiz in Campaign Detail Function");

        var query = _mapper.Map<GetQuizInCampaignDetailQuery>(request);

        var result = await _mediator.Send(query);

        var response = _mapper.Map<GetQuizInCampaignDetailResponse>(result);

        return response;
    }


    #endregion Quiz

    #region Question
    public override async Task<QuestionSuccessResponse> UpdateQuestion(UpdatedQuestionRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Update Question Function");

        var command = _mapper.Map<UpdateQuestionCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<QuestionSuccessResponse>(result);

        return response;
    }

    public override async Task<Empty> DeletedQuestion(DeletedQuestionRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Deleted Question Function");

        var command = new DeletedQuestionCommand() { Id = Guid.Parse(request.Id) };

        await _mediator.Send(command);

        return new Empty();
    }

    public override async Task<QuestionSuccessResponse> CreateQuizQuestion(CreatedQuizQuestionRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Create Quiz Question Function");

        var command = _mapper.Map<CreateQuizQuestionCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<QuestionSuccessResponse>(result);

        return response;
    }

    public override async Task<QuestionMiniResponse> GetQuestionDetail(GetQuestionDetailRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Question Detail Function");

        var query = new GetQuestionDetailQuery() { Id = Guid.Parse(request.Id) };

        var result = await _mediator.Send(query);

        var response = _mapper.Map<QuestionMiniResponse>(result);

        return response;
    }

    #endregion Question

    #region Dimension
    public override async Task<DimensionMiniResponse> CreateDimension(CreatedNewDimensionRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Create Dimension Function");

        var command = _mapper.Map<CreatedDimensionCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<DimensionMiniResponse>(result);

        return response;
    }

    public override async Task<Empty> SyncDimension(SyncDimensionRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Update Question Function");

        var command = _mapper.Map<SyncDimensionCommand>(request);

        await _mediator.Send(command);

        return new Empty();
    }

    public override async Task<DimensionResponse> GetDimensionDetail(GetDimensionDetailRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Dimension Detail Function");

        var query = _mapper.Map<GetDimensionDetailQuery>(request);

        var result = await _mediator.Send(query);

        var response = _mapper.Map<DimensionResponse>(result);

        return response;
    }

    public override async Task<DimensionMiniResponse> UpdateDimension(UpdatedQuizDimensionRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Update Dimension Function");

        var command = _mapper.Map<UpdateQuizDimensionCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<DimensionMiniResponse>(result);

        return response;
    }

    public override async Task<PagedDimensionMiniResponse> GetQuizDimensionList(GetQuizDimensionListRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Dimension List Function");

        var query = _mapper.Map<GetQuizDimensionListQuery>(request);

        var result = await _mediator.Send(query);

        var response = new PagedDimensionMiniResponse
        {
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize,
        };

        response.Data.AddRange(_mapper.Map<List<QuizDimensionMiniResponse>>(result.Data));

        return response;
    }

    #endregion Dimension

    #region QuizAttempts
    public override async Task<QuizAttemptMiniResponse> CreateQuizAttempt(CreateQuizAttemptRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Create Quiz Attempt Function");

        var command = _mapper.Map<CreateQuizAttemptCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<QuizAttemptMiniResponse>(result);

        return response;
    }

    public override async Task<QuizAttemptMiniResponse> UpdateQuizAttempt(UpdateQuizAttemptRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Update Quiz Attempt Function");

        var command = _mapper.Map<UpdateQuizAttemptCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<QuizAttemptMiniResponse>(result);

        return response;
    }

    public override async Task<QuizAttemptResponse> GetQuizAttemptQuery(GetQuizAttemptQueryRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Quiz Attempt Detail By UserId/LeadId Function");

        var command = _mapper.Map<GetQuizAttemptQueryQuery>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<QuizAttemptResponse>(result);

        return response;
    }


    #endregion QuizAttempts

    #region QuizResult
    public override async Task<QuizResultMiniResponse> SubmitQuizAttempt(SubmitQuizAttemptRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Create Quiz Attempt Function");

        var command = _mapper.Map<SubmitQuizAttemptCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<QuizResultMiniResponse>(result);

        return response;
    }

    public override async Task<QuizResultResponse> GetQuizResultDetail(GetQuizResultDetailRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Quiz Result Detail Function");

        var query = _mapper.Map<GetQuizResultDetailQuery>(request);

        var result = await _mediator.Send(query);

        var response = _mapper.Map<QuizResultResponse>(result);

        return response;
    }

    public override async Task<PagedCampaignQuizResultResponse> GetQuizResultList(GetQuizResultListRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Quiz Result List Function");

        var query = _mapper.Map<GetQuizResultListQuery>(request);

        var result = await _mediator.Send(query);

        var response = new PagedCampaignQuizResultResponse
        {
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize,
        };

        response.Data.AddRange(_mapper.Map<List<CampaignQuizResultResponse>>(result.Data));

        return response;
    }

    #endregion QuizResult

    #region Lead
    public override async Task<LeadMiniResponse> UpdateLead(UpdateLeadRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Update Lead Function");

        var command = _mapper.Map<UpdateLeadCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<LeadMiniResponse>(result);

        return response;
    }

    public override async Task<LeadResponse> GetLeadDetail(GetLeadDetailRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Lead Detail Function");

        var command = _mapper.Map<GetLeadDetailQuery>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<LeadResponse>(result);

        return response;
    }

    public override async Task<PagedLeadMiniResponse> GetLeadList(GetLeadListRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Lead List Function");

        var query = _mapper.Map<GetLeadListQuery>(request);

        var result = await _mediator.Send(query);

        var response = new PagedLeadMiniResponse
        {
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize,
        };

        response.Data.AddRange(_mapper.Map<List<LeadMiniResponse>>(result.Data));

        return response;
    }
    #endregion Lead

    #region Lead Interaction & Scoring
    public override async Task<Empty> LogInteraction(LogInteractionRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Log Interaction Function for Lead {LeadId}", request.LeadId);

        var command = _mapper.Map<LogInteractionCommand>(request);

        await _mediator.Send(command);

        return new Empty();
    }

    public override async Task<LeadEngagementResponse> GetLeadEngagement(GetLeadEngagementRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Lead Engagement Function for Lead {LeadId}", request.LeadId);
        var query = _mapper.Map<GetLeadEngagementQuery>(request);
        var result = await _mediator.Send(query);
        return _mapper.Map<LeadEngagementResponse>(result);
    }

    public override async Task<ScoringRuleResponse> UpdateScoringRule(UpdateScoringRuleRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Update Scoring Rule Function for Tenant {Tenant}", request.Tenant);
        var command = _mapper.Map<UpdateScoringRuleCommand>(request);
        var result = await _mediator.Send(command);
        return _mapper.Map<ScoringRuleResponse>(result);
    }

    public override async Task<ScoringRulesResponse> GetScoringRules(GetScoringRulesRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Scoring Rules Function for Tenant {Tenant}", request.Tenant);

        var query = _mapper.Map<GetScoringRulesQuery>(request);

        var result = await _mediator.Send(query);
        
        var response = new ScoringRulesResponse();

        response.Rules.AddRange(_mapper.Map<List<ScoringRuleResponse>>(result));

        return response;
    }

    public override async Task<PagedInteractionLogMiniResponse> GetLeadInteractions(GetLeadInteractionsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Lead Interactions Function for Lead {LeadId}", request.LeadId);

        var query = _mapper.Map<GetLeadInteractionsQuery>(request);

        var result = await _mediator.Send(query);

        var response = new PagedInteractionLogMiniResponse
        {
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        };

        response.Data.AddRange(_mapper.Map<List<InteractionLogMiniResponse>>(result.Data));

        return response;
    }
    #endregion Lead Interaction & Scoring

    #region Recommendation
    public override async Task<RecommendationsResponse> GetRecommendations(GetRecommendationsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Recommendations Function for Tenant {Tenant}", request.Tenant);

        var query = _mapper.Map<GetRecommendationsQuery>(request);

        var result = await _mediator.Send(query);

        var response = new RecommendationsResponse();
        response.Data.AddRange(_mapper.Map<List<RecommendationProto>>(result));

        return response;
    }

    public override async Task<RecommendationProto> GetRecommendationDetail(GetRecommendationDetailRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Recommendation Detail Function for Id {Id}", request.Id);

        var query = _mapper.Map<GetRecommendationDetailQuery>(request);

        var result = await _mediator.Send(query);

        if (result == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Recommendation not found"));
        }

        return _mapper.Map<RecommendationProto>(result);
    }

    public override async Task<RecommendationMiniResponse> CreateRecommendation(CreateRecommendationRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Create Recommendation Function for Tenant {Tenant}", request.Tenant);

        var command = _mapper.Map<CreateRecommendationCommand>(request);

        var result = await _mediator.Send(command);

        return _mapper.Map<RecommendationMiniResponse>(result);
    }

    public override async Task<RecommendationMiniResponse> UpdateRecommendation(UpdateRecommendationRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Update Recommendation Function for Id {Id}", request.Id);

        var command = _mapper.Map<UpdateRecommendationCommand>(request);

        var result = await _mediator.Send(command);

        return _mapper.Map<RecommendationMiniResponse>(result);
    }

    public override async Task<Empty> DeleteRecommendation(DeleteRecommendationRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Delete Recommendation Function for Id {Id}", request.Id);

        var command = _mapper.Map<DeleteRecommendationCommand>(request);

        var result = await _mediator.Send(command);

        if (!result)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Recommendation not found"));
        }

        return new Empty();
    }

    public override async Task<PagedRecommendationMiniResponse> GetRecommendationList(GetRecommendationListRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Recommendation List Function");

        var query = _mapper.Map<GetRecommendationListQuery>(request);

        var result = await _mediator.Send(query);

        var response = new PagedRecommendationMiniResponse
        {
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize,
        };

        response.Data.AddRange(_mapper.Map<List<RecommendationMiniResponse>>(result.Data));

        return response;
    }
    #endregion Recommendation

    #region ScoringRule
    public override async Task<ScoringRuleMiniResponse> CreateScoringRule(CreatedScoringRuleRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Create Scoring Rule Function");

        var command = _mapper.Map<CreatedScoringRuleCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<ScoringRuleMiniResponse>(result);

        return response;
    }

    public override async Task<ScoringRuleMiniResponse> UpdatedScoringRule(UpdatedScoringRuleRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Update Scoring Rule Function");

        var command = _mapper.Map<UpdatedScoringRuleCommand>(request);

        var result = await _mediator.Send(command);

        var response = _mapper.Map<ScoringRuleMiniResponse>(result);

        return response;
    }

    public override async Task<ScoringRuleDetailResponse> GetScoringRuleDetail(GetScoringRuleDetailRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get Scoring Rule Detail Function");

        var query = new GetScoringRuleDetailQuery() { Id = Guid.Parse(request.Id) };

        var result = await _mediator.Send(query);

        var response = _mapper.Map<ScoringRuleDetailResponse>(result);

        return response;
    }

    public override async Task<Empty> DeleteScoringRule(DeleteScoringRuleRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Delete Scoring Rule Function");

        var command = new DeleteScoringRuleCommand() { Id = Guid.Parse(request.Id) };

         await _mediator.Send(command);

        return new Empty();
    }

    public override async Task<PagedScoringRuleMiniResponse> GetScoringRuleList(GetScoringRuleListRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get ScoringRule List Function");

        var query = _mapper.Map<GetScoringRuleListQuery>(request);

        var result = await _mediator.Send(query);

        var response = new PagedScoringRuleMiniResponse
        {
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize,
        };

        response.Data.AddRange(_mapper.Map<List<ScoringRuleMiniResponse>>(result.Data));

        return response;
    }
    #endregion ScoringRule
}