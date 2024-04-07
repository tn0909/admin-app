
using AdminApp.Dtos;
using AdminApp.Models;
using AutoMapper;
using Nest;

namespace AdminApp.Services
{
    public class UserService : IUserService
    {
        private IElasticClient _elasticClient;

        private readonly IMapper _mapper;

        public UserService(IElasticClient elasticClient, IMapper mapper)
        {
            _elasticClient = elasticClient;
            _mapper = mapper;
        }

        public async Task<bool> Add(UserDto userDto)
        {
            var id = Guid.NewGuid().ToString();
            var user = _mapper.Map<User>(userDto);
            user.Id = id;

            var response = await _elasticClient.IndexAsync<User>(user, idx => idx.Routing(userDto.CompanyId));
            return response.IsValid;
        }

        public async Task<IEnumerable<UserDto>> Search(SearchUserDto searchParams)
        {
            var searchDescriptor = new SearchDescriptor<User>()
                .Size(searchParams.Limit)
                .Query(q =>
                {
                    var queryContainer = !q.Term(t => t.JoinField, "parent");

                    if (!string.IsNullOrEmpty(searchParams.Email))
                    {
                        queryContainer &= q
                            .Term(t => t
                                .Field("email.keyword")
                                .Value(searchParams.Email));
                    }
                    
                    if (!string.IsNullOrEmpty(searchParams.Company))
                    {
                        queryContainer &= q
                            .HasParent<Company>(c => c
                                .ParentType("parent")
                                .Query(q1 => q1
                                    .QueryString(d => d
                                        .Query('*' + searchParams.Company + '*')
                                    )
                                )
                            );
                    }

                    return queryContainer;
                });

            var response = await _elasticClient.SearchAsync<User>(searchDescriptor);

            return _mapper.Map<IEnumerable<UserDto>>(response.Documents);
        }

        public async Task<UserDto> GetById(string id)
        {
            var response = await _elasticClient.SearchAsync<User>(x => x
                .Query(q =>
                    !q.Term(t => t.JoinField, "parent")
                    && q.Ids(i => i.Values(id))
                )
            );

            return _mapper.Map<UserDto>(response.Documents.FirstOrDefault());
        }

        public async Task<bool> Update(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var response = await _elasticClient.UpdateAsync<User>(user.Id, u => u.Doc(user));
            return response.IsValid;
        }

        public async Task<bool> DeleteById(string id)
        {
            var response = await _elasticClient.DeleteByQueryAsync<User>(x => x
                .Query(q =>
                    !q.Term(c => c.JoinField, "parent")
                    && q.Term(c => c.Id, id)
                )
            );

            return response.IsValid;
        }
    }
}