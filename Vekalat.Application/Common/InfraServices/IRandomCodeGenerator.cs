namespace Vekalat.Application.Common.InfraServices
{
    public interface IRandomCodeGenerator
    {
        string GuidGenerator();
        string CustomGuidGenerator(int take);

        string DigitCodeGenerator(int digit);
    }
}