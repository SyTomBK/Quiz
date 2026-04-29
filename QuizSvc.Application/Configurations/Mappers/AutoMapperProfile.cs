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
using QuizSvc.Application.Dtos;
using QuizSvc.Application.Queries.Campains;
using QuizSvc.Application.Queries.Dimensions;
using QuizSvc.Application.Queries.Interactions;
using QuizSvc.Application.Queries.Leads;
using QuizSvc.Application.Queries.QuizAttempts;
using QuizSvc.Application.Queries.QuizResults;
using QuizSvc.Application.Queries.Quizs;
using QuizSvc.Application.Queries.Recommendations;
using QuizSvc.Application.Queries.ScoringRules;

namespace QuizSvc.Application.Configurations.Mappers;
public class AutoMapperProfile : BaseProfile
{
    public AutoMapperProfile()
    {
        CreateMap<CreatedCampaignRequest, CreatedCampaignCommand>()
            .ForMember(dest => dest.QuizSettings, opt => opt.MapFrom(src => src.QuizSettings));
        CreateMap<CreatedCampaignCommand, Campaign>();
        CreateMap<CampaignQuizSettingProto, CampaignQuizSettingDto>(); 
        CreateMap<LeadCollectionPolicyProto, LeadCollectionPolicyDto>();
        CreateMap<LeadCollectionPolicyDto, LeadCollectionPolicy>();
        CreateMap<CheckpointConfigProto, CheckpointConfigDto>();
        CreateMap<QuizCheckpointProto, QuizCheckpointDto>();
        CreateMap<CreateCampaignQuizSettingProto, CreateCampaignQuizSettingCommand>();

        CreateMap<CheckpointTriggerProto, CheckpointTriggerDto>();
        CreateMap<CheckpointContentProto, CheckpointContentDto>();
        CreateMap<PromoContentProto, PromoContentDto>();
        CreateMap<GamificationContentProto, GamificationContentDto>();
        CreateMap<ProgressiveFormContentProto, ProgressiveFormContentDto>();
        CreateMap<GateContentProto, GateContentDto>();
        CreateMap<CheckpointConfigDto, CheckpointConfig>();
        CreateMap<QuizCheckpointDto, QuizCheckpoint>();
        CreateMap<CheckpointTriggerDto, CheckpointTrigger>();
        CreateMap<CheckpointContentDto, CheckpointContent>();
        CreateMap<PromoContentDto, PromoContent>();
        CreateMap<GamificationContentDto, GamificationContent>();
        CreateMap<ProgressiveFormContentDto, ProgressiveFormContent>();
        CreateMap<GateContentDto, GateContent>();
        CreateMap<FormFieldDto, FormField>();
        CreateMap<CampaignQuizSettingDto, CampaignQuizSetting>();
        CreateMap<Campaign, CampaignMiniResponseDto>();
        CreateMap<CampaignMiniResponseDto, CampaignMiniResponse>();
        CreateMap<GetCampaignDetailRequest, GetCampaignDetailQuery>();
        CreateMap<CampaignQuizSetting, CampaignQuizSettingDto>();
        CreateMap<Campaign, CampaignResponseDto>()
           .ForMember(dest => dest.QuizSettings, opt => opt.MapFrom(src => src.CampaignQuizSettings));
        CreateMap<CampaignQuizSettingDto, CampaignQuizSettingResponseProto>();
        CreateMap<CampaignQuizSetting, CampaignQuizSettingResponseDto>();
        CreateMap<CampaignQuizSettingResponseDto, CampaignQuizSettingResponseProto>();
        CreateMap<UpdatedCampaignQuizSettingRequest, UpdateCampaignQuizSettingCommand>();
        CreateMap<CampaignQuizSetting, CampaignQuizSettingMiniResponseDto>();
        CreateMap<CampaignQuizSettingMiniResponseDto, CampaignQuizSettingMiniResponse>();
        CreateMap<LeadCollectionPolicy, LeadCollectionPolicyDto>();
        CreateMap<CheckpointConfig, CheckpointConfigDto>();
        CreateMap<FormFieldProto, FormFieldDto>();
        CreateMap<FormFieldType, FormFieldTypeProto>();
        CreateMap<FormField, FormFieldDto>();
        CreateMap<FormFieldDto, FormFieldProto>();

        CreateMap<QuizCheckpoint, QuizCheckpointDto>();
        CreateMap<CheckpointTrigger, CheckpointTriggerDto>();
        CreateMap<CheckpointContent, CheckpointContentDto>();
        CreateMap<PromoContent, PromoContentDto>();
        CreateMap<GamificationContent, GamificationContentDto>();
        CreateMap<ProgressiveFormContent, ProgressiveFormContentDto>();
        CreateMap<GateContent, GateContentDto>();
        CreateMap<CampaignQuizSettingDto, CampaignQuizSettingProto>();
        CreateMap<LeadCollectionPolicyDto, LeadCollectionPolicyProto>();
        CreateMap<CheckpointConfigDto, CheckpointConfigProto>();
        CreateMap<QuizCheckpointDto, QuizCheckpointProto>();
        CreateMap<CheckpointTriggerDto, CheckpointTriggerProto>();
        CreateMap<CheckpointContentDto, CheckpointContentProto>();
        CreateMap<PromoContentDto, PromoContentProto>();
        CreateMap<GamificationContentDto, GamificationContentProto>();
        CreateMap<ProgressiveFormContentDto, ProgressiveFormContentProto>();
        CreateMap<GateContentDto, GateContentProto>();
        CreateMap<CampaignResponseDto, CampaignResponse>();
        CreateMap<UpdatedCampaignRequest, UpdatedCampaignCommand>();
        CreateMap<UpdatedCampaignQuizSetting, UpdatedCampaignQuizSettingDto>();
        CreateMap<UpdatedCampaignCommand, Campaign>();
        CreateMap<GetCampaignListRequest, GetCampaignListQuery>();
        CreateMap<CreatedDimensionScoreRequest, CreatedDimensionScoreRequestDto>();
        CreateMap<CreatedQuizDimensionScoreRequest, CreatedQuizDimensionScoreRequestDto>();
        CreateMap<CreatedAnswerRequest, CreatedAnswerRequestDto>();
        CreateMap<CreatedQuizAnswerRequest, CreatedQuizAnswerRequestDto>();
        CreateMap<CreatedQuestionRequest, CreatedQuestionRequestDto>();
        CreateMap<CreatedQuizQuestionRequest, CreatedQuizQuestionRequestDto>();
        CreateMap<CreatedQuizQuestionRequest, CreateQuizQuestionCommand>();
        CreateMap<CreatedDimensionRequest, CreatedDimensionRequestDto>();
        CreateMap<CreatedQuizRequest, CreatedQuizCommand>();
        CreateMap<Domain.Entities.Quiz, QuizMiniResponseDto>();
        CreateMap<QuizMiniResponseDto, QuizMiniResponse>();
        CreateMap<UpdatedQuizRequest, UpdateQuizCommand>();
        CreateMap<UpdateQuizCommand, Domain.Entities.Quiz>();
        CreateMap<GetQuizListRequest, GetQuizListQuery>();
        CreateMap<GetQuizDetailRequest, GetQuizDetailQuery>();
        CreateMap<DimensionMiniResponseDto, DimensionMiniResponse>();
        CreateMap<QuestionMiniResponseDto, QuestionMiniResponse>();
        CreateMap<AnswerMiniResponseDto, AnswerMiniResponse>();
        CreateMap<DimensionScoreMiniResponseDto, DimensionScoreMiniResponse>();
        CreateMap<QuizResponseDto, QuizResponse>();
        CreateMap<UpdatedQuestionRequest, UpdateQuestionCommand>();
        CreateMap<Question, QuestionSuccessResponseDto>();
        CreateMap<QuestionSuccessResponseDto, QuestionSuccessResponse>();
        CreateMap<UpdatedAnswerRequest, UpdatedAnswerRequestDto>();
        CreateMap<UpdatedDimensionScoreRequest, UpdatedDimensionScoreRequestDto>();
        CreateMap<CreatedNewDimensionRequest, CreatedDimensionCommand>();
        CreateMap<CreatedDimensionCommand, Dimension>();
        CreateMap<Dimension, DimensionMiniResponseDto>();
        CreateMap<UpdatedDimensionRequest, UpdatedDimensionRequestDto>();
        CreateMap<SyncDimensionRequest, SyncDimensionCommand>();
        CreateMap<Dimension, DimensionResponseDto>();
        CreateMap<GetQuizDimensionListRequest, GetQuizDimensionListQuery>();
        CreateMap<QuizDimensionMiniResponseDto, QuizDimensionMiniResponse>();

        CreateMap<DimensionResponseDto, DimensionResponse>();
        CreateMap<GetDimensionDetailRequest, GetDimensionDetailQuery>();
        CreateMap<UpdatedQuizDimensionRequest, UpdateQuizDimensionCommand>();
        CreateMap<Question, QuestionMiniResponseDto>();
        CreateMap<Answer, AnswerMiniResponseDto>();
        CreateMap<AnswerDimensionScore, DimensionScoreMiniResponseDto>();
        CreateMap<Answer, AnswerMiniResponseDto>()
            .ForMember(dest => dest.DimensionScores,
                opt => opt.MapFrom(src => src.AnswerDimensionScores));

        CreateMap<CloneQuizCommand, Domain.Entities.Quiz>();
        CreateMap<CloneQuizRequest, CloneQuizCommand>();
        CreateMap<DeepCopyQuizRequest, DeepCopyQuizCommand>();
        CreateMap<DeepCopyQuizCommand, Domain.Entities.Quiz>();
        CreateMap<CreateQuizAttemptRequest, CreateQuizAttemptCommand>();
        CreateMap<UpdateQuizAttemptRequest, UpdateQuizAttemptCommand>();
        CreateMap<QuizAttempt, QuizAttemptMiniResponseDto>();
        CreateMap<QuizAttemptMiniResponseDto, QuizAttemptMiniResponse>();
        CreateMap<GetQuizAttemptQueryRequest, GetQuizAttemptQueryQuery>();
        CreateMap<QuizAttemptResponseDto, QuizAttemptResponse>();
        CreateMap<QuizAttempt, QuizAttemptResponseDto>();
        CreateMap<UserAnswer, UserAnswerResponseDto>();
        CreateMap<UserAnswerResponseDto, UserAnswerResponse>();
        CreateMap<UserAnswerRequest, UserAnswerRequestDto>();
        CreateMap<SubmitQuizAttemptRequest, SubmitQuizAttemptCommand>();
        CreateMap<QuizResultMiniResponseDto, QuizResultMiniResponse>();
        CreateMap<UserAnswerRequestDto, UserAnswer>();
        CreateMap<QuizResult, QuizResultMiniResponseDto>();
        CreateMap<DimensionScoreResult, DimensionScoreResultDto>();
        CreateMap<DimensionScoreResultDto, DimensionScoreResultProto>();
        CreateMap<GetQuizResultDetailRequest, GetQuizResultDetailQuery>();
        CreateMap<QuizResult, QuizResultResponseDto>();
        CreateMap<QuizResultResponseDto, QuizResultResponse>();;
        CreateMap<GetQuizResultListRequest, GetQuizResultListQuery>()
            .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => ParseNullableGuid(src.LeadId)))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => ParseNullableGuid(src.UserId)))
            .ForMember(dest => dest.QuizId, opt => opt.MapFrom(src => ParseNullableGuid(src.QuizId)))
            .ForMember(dest => dest.CampaignId, opt => opt.MapFrom(src => ParseNullableGuid(src.CampaignId)));
        CreateMap<DimensionScoreResultDto, DimensionScoreResult>();
        CreateMap<CampaignQuizResultResponseDto, CampaignQuizResultResponse>();
        CreateMap<UpdateLeadRequest, UpdateLeadCommand>()
            .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => ParseGuid(src.LeadId)))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => ParseNullableGuid(src.CustomerId)))
            .ForMember(dest => dest.ReferralCode, opt => opt.MapFrom(src => src.ReferralCode))
            .ForMember(dest => dest.Tenant, opt => opt.MapFrom(src => src.Tenant))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (LeadStatus)src.Status))
            .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note));
        CreateMap<UpdateLeadCommand, Lead>();
        CreateMap<Lead, LeadMiniResponseDto>()
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<LeadMiniResponseDto, LeadMiniResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (LeadStatusProto)src.Status));
        CreateMap<GetLeadDetailRequest, GetLeadDetailQuery>();
        CreateMap<Lead, LeadResponseDto>()
             .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<LeadResponseDto, LeadResponse>()
             .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (LeadStatusProto)src.Status));
        CreateMap<GetQuizInCampaignDetailRequest, GetQuizInCampaignDetailQuery>();
        CreateMap<GetQuizInCampaignDetailResponseDto, GetQuizInCampaignDetailResponse>();

        CreateMap<GetLeadListRequest, GetLeadListQuery>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => ParseNullableGuid(src.CustomerId)))
            .ForMember(dest => dest.SortByScore, opt => opt.MapFrom(src => src.SortByScore))
            .ForMember(dest => dest.IsTopLeads, opt => opt.MapFrom(src => src.IsTopLeads));
        // Lead Interaction & Scoring
        CreateMap<LogInteractionRequest, LogInteractionCommand>()
            .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => ParseGuid(src.LeadId)))
            .ForMember(dest => dest.QuizAttemptId, opt => opt.MapFrom(src => ParseNullableGuid(src.QuizAttemptId)))
            .ForMember(dest => dest.TargetId, opt => opt.MapFrom(src => ParseNullableGuid(src.TargetId)));
        CreateMap<LogInteractionCommand, InteractionLog>();
        CreateMap<InteractionLog, InteractionLogMiniResponseDto>();
        CreateMap<InteractionLogMiniResponseDto, InteractionLogMiniResponse>();
        CreateMap<UpdateScoringRuleRequest, UpdateScoringRuleCommand>();
        CreateMap<UpdateScoringRuleCommand, ScoringRule>();
        CreateMap<ScoringRule, ScoringRuleDto>();
        CreateMap<ScoringRuleDto, ScoringRuleResponse>();
        CreateMap<ScoringRule, ScoringRuleResponse>(); // Direct mapping
        CreateMap<GetScoringRulesRequest, GetScoringRulesQuery>();
        CreateMap<LeadEngagementProfile, LeadEngagementResponseDto>();
        CreateMap<LeadEngagementResponseDto, LeadEngagementResponse>();
        CreateMap<LeadEngagementProfile, LeadEngagementResponse>(); // Direct mapping
        CreateMap<InterestAffinity, InterestAffinityDto>();
        CreateMap<InterestAffinityDto, InterestAffinityProto>();
        CreateMap<InterestAffinity, InterestAffinityProto>(); // Direct mapping
        CreateMap<GetLeadInteractionsRequest, GetLeadInteractionsQuery>();
        CreateMap<InteractionLog, InteractionLogMiniResponse>(); // Direct mapping
        CreateMap<PagedInteractionLogMiniResponseDto, PagedInteractionLogMiniResponse>();
        CreateMap<GetLeadEngagementRequest, GetLeadEngagementQuery>();

        // Recommendation
        CreateMap<GetRecommendationsRequest, GetRecommendationsQuery>();
        CreateMap<GetRecommendationDetailRequest, GetRecommendationDetailQuery>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ParseGuid(src.Id)));
        CreateMap<CreateRecommendationRequest, CreateRecommendationCommand>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (RecommendationType)src.Type));
        CreateMap<Recommendation, RecommendationMiniResponseDto>();
        CreateMap<RecommendationMiniResponseDto, RecommendationMiniResponse>();
        CreateMap<CreateRecommendationCommand, Recommendation>();
        CreateMap<UpdateRecommendationRequest, CreateRecommendationCommand>();
        CreateMap<GetRecommendationListRequest, GetRecommendationListQuery>();

        CreateMap<UpdateRecommendationRequest, UpdateRecommendationCommand>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (RecommendationType)src.Type));

        CreateMap<UpdateRecommendationCommand, Recommendation>();
        CreateMap<DeleteRecommendationRequest, DeleteRecommendationCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ParseGuid(src.Id)));
        CreateMap<Recommendation, RecommendationProto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (RecommendationTypeProto)src.Type));

        //ScoringRule
        CreateMap<CreatedScoringRuleRequest, CreatedScoringRuleCommand>();
        CreateMap<CreatedScoringRuleCommand, ScoringRule>();
        CreateMap<ScoringRule, ScoringRuleMiniResponseDto>();
        CreateMap<ScoringRuleMiniResponseDto, ScoringRuleMiniResponse>();

        CreateMap<UpdatedScoringRuleRequest, UpdatedScoringRuleCommand>();
        CreateMap<UpdatedScoringRuleCommand, ScoringRule>();
        CreateMap<ScoringRule, ScoringRuleDetailResponseDto>();
        CreateMap<ScoringRuleDetailResponseDto, ScoringRuleDetailResponse>();

        CreateMap<GetScoringRuleListRequest, GetScoringRuleListQuery>();
    }

    private static Guid ParseGuid(string? id) => Guid.TryParse(id, out var g) ? g : Guid.Empty;
    private static Guid? ParseNullableGuid(string? id) => string.IsNullOrEmpty(id) ? (Guid?)null : Guid.TryParse(id, out var g) ? g : (Guid?)null;
}
