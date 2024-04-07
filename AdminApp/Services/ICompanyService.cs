using AdminApp.Dtos;

namespace AdminApp.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyResponseDto>> Search(SearchCompanyDto searchParams);
        Task<CompanyResponseDto> GetById(string id);
        Task<bool> Add(CompanyRequestDto companyDto);
        Task<bool> Update(CompanyRequestDto companyDto);
        Task<bool> DeleteById(string id);
    }
}
