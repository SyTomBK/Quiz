using QuizSvc.Application.Dtos;
using QuizSvc.Share.Enums;

namespace QuizSvc.Application.Commands.Leads;

public class UpdateLeadCommand : IRequest<LeadMiniResponseDto>
{
    public required Guid LeadId { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? SchoolName { get; set; }
    public string? Avatar { get; set; }
    public string? ReferralCode { get; set; }
    public string? Tenant { get; set; }
    public Guid? CustomerId { get; set; }
    public LeadStatus Status { get; set; }
    public string? Note { get; set; }
}

public class UpdateLeadCommandHandler : IRequestHandler<UpdateLeadCommand, LeadMiniResponseDto>
{
    private readonly IMapper _mapper;
    private readonly ILeadRepository _leadRepository;
    public UpdateLeadCommandHandler(IMapper mapper, ILeadRepository leadRepository)
    {
        _mapper = mapper;
        _leadRepository = leadRepository;
    }
    public async Task<LeadMiniResponseDto> Handle(UpdateLeadCommand request, CancellationToken cancellationToken)
    {
        var lead = await _leadRepository.GetByLeadIdAsync(request.LeadId, cancellationToken);

        if(lead == null)
        {
            lead = _mapper.Map<Lead>(request);
            lead.IsActive = true;
            await _leadRepository.AddAsync(lead, cancellationToken);
        }
        else
        {
            _mapper.Map(request, lead);
        }

        await _leadRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LeadMiniResponseDto>(lead);
    }
}