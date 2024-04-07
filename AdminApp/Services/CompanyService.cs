
using AdminApp.Dtos;
using AdminApp.Models;
using AutoMapper;
using Nest;

namespace AdminApp.Services
{
    public class CompanyService : ICompanyService
    {
        private IElasticClient _elasticClient;

        private readonly IMapper _mapper;

        public CompanyService(IElasticClient elasticClient, IMapper mapper)
        {
            _elasticClient = elasticClient;
            _mapper = mapper;
        }

        public async Task<bool> Add(CompanyRequestDto companyDto)
        {
            var id = Guid.NewGuid().ToString();
            var company = _mapper.Map<Company>(companyDto);
            company.Id = id;

            var response = await _elasticClient.IndexAsync<Company>(company, idx => idx.Routing(company.Id));

            return response.IsValid;
        }

        public async Task<IEnumerable<CompanyResponseDto>> Search(SearchCompanyDto searchParams)
        {
            if (!searchParams.IncludeUsers || searchParams.UsersLimit < 1)
            {
                return await SearchWithoutUsers(searchParams);
            }
            else
            {
                return await SearchWithUsers(searchParams);
            }
        }

        private async Task<IEnumerable<CompanyResponseDto>> SearchWithUsers(SearchCompanyDto searchParams)
        {
            var response = await _elasticClient.SearchAsync<Company>(x => x
                .Query(q => q
                    .Bool(b => b
                        .Must(m => 
                            m.Term(c => c.JoinField, "parent")
                            && m.QueryString(d => d.Query('*' + searchParams.SearchTerm + '*'))
                        )
                        .Should(s => s
                            .HasChild<User>(c => c
                                .Type("user")
                                .InnerHits(i => i
                                    .Name("users")
                                    .Explain(false)
                                    .Size(searchParams.Limit)
                                )
                                .Query(q1 => q1.MatchAll())
                            )
                        )
                    )
                )
                .Size(searchParams.Limit)
            );

            var companyDtos = new List<CompanyResponseDto>();

            foreach (Hit<Company> hit in response.Hits)
            {
                var companyDto = _mapper.Map<CompanyResponseDto>(hit.Source);
                companyDto.Users = _mapper.Map<IEnumerable<UserDto>>(hit.InnerHits["users"].Documents<User>()).ToList();

                companyDtos.Add(companyDto);
            }

            return companyDtos;
        }

        private async Task<IEnumerable<CompanyResponseDto>> SearchWithoutUsers(SearchCompanyDto searchParams)
        {
            var response = await _elasticClient.SearchAsync<Company>(x => x
                                .Query(q =>
                                    q.Term(c => c.JoinField, "parent")
                                    && q.QueryString(d => d
                                        .Query('*' + searchParams.SearchTerm + '*'))
                                )
                                .Size(searchParams.Limit)
                            );

            return _mapper.Map<IEnumerable<CompanyResponseDto>>(response.Documents);
        }

        public async Task<CompanyResponseDto> GetById(string id)
        {
            var response = await _elasticClient.SearchAsync<Company>(x => x
                .Query(q =>
                    q.Term(c => c.JoinField, JoinField.Root<Company>())
                    && q.Ids(i => i.Values(id))
                )
            );

            return _mapper.Map<CompanyResponseDto>(response.Documents.FirstOrDefault());
        }

        public async Task<bool> Update(CompanyRequestDto companyDto)
        {
            var company = _mapper.Map<Company>(companyDto);
            var response = await _elasticClient.UpdateAsync<Company>(company.Id, u => u.Doc(company));

            return response.IsValid;
        }

        public async Task<bool> DeleteById(string id)
        {
            var response = await _elasticClient.DeleteByQueryAsync<Company>(x => x
                .Query(q =>
                    q.Term(c => c.JoinField, "parent")
                    && q.Term(c => c.Id, id)
                )
            );

            return response.IsValid;
        }
    }
}