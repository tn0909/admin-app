using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminApp.Models;
using Nest;

namespace AdminApp.Extensions
{
    public static class ElasticsearchExtensions
    {
        public static IServiceCollection AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var uri = configuration["ElasticsearchSettings:Uri"];
            var index = configuration["ElasticsearchSettings:DefaultIndex"];

            var node = new Uri(uri);
            
            var connectionSettings = new ConnectionSettings(node)
                .DefaultMappingFor<Document>(m => m.IndexName(index))
                .DefaultMappingFor<User>(m => m.IndexName(index))
                .DefaultMappingFor<Company>(m => m
                    .IndexName(index)
                    .RelationName("parent")
                );

            var client = new ElasticClient(connectionSettings);
           
           var createIndexResponse = client.Indices.Create(index, c => c
                .Index<Document>()
                .Map<Document>(m => m
                    .RoutingField(r => r.Required()) 
                    .AutoMap<Company>() 
                    .AutoMap<User>() 
                    .Properties(props => props
                        .Join(j => j 
                            .Name(p => p.JoinField)
                            .Relations(r => r
                                .Join<Company, User>()
                            )
                        )
                    )
                )
            );
            
            services.AddSingleton<IElasticClient>(client);

            return services;
        }

    }
}