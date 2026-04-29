namespace QuizSvc.Application.Contracts.Persistence;
public interface ICodeGenerator
{
    string Generate(string prefix, int length = 8);
}
